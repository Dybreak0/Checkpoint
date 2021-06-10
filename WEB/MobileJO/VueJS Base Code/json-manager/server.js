const fs = require('fs')
const bodyParser = require('body-parser')
const jsonServer = require('json-server')
const jwt = require('jsonwebtoken')
const server = jsonServer.create()
const router = jsonServer.router('./db.json')
const userdb = JSON.parse(fs.readFileSync('./db.json', 'UTF-8'))
const middlewares = jsonServer.defaults()

server.use(middlewares)
server.use(jsonServer.bodyParser)

const SECRET_KEY = '123456789'
const expiresIn = '1hr'

// helper functions
const createToken = (payload) => {
  return jwt.sign(payload, SECRET_KEY, {expiresIn})
}

const verifyToken = (token) => {
  return  jwt.verify(token, SECRET_KEY, (err, decode) => decode !== undefined ?  decode : err)
}

const isAuthenticated = ({email, password}) => {
  return userdb.users.findIndex(user => user.email === email && user.password === password) !== -1
}

// POST /auth/login endpoint
server.post('/auth/login', (req, res) => {
  const {email, password, token} = req.body
  if (isAuthenticated({email, password}) === false) {
    const status = 401
    const message = 'Incorrect email or password'
    res.status(status).json({status, message})
    return
  }

  const access_token = createToken({email, password})
  res.status(200).json({access_token})
})

// add an Express middleware that checks that the authorization header has the Bearer scheme
// then verifies if the token if valid for all routes except the previous route
// since this is the one we use to login the users
server.use(/^(?!\/auth).*$/,  (req, res, next) => {
  if (req.headers.authorization === undefined || req.headers.authorization.split(' ')[0] !== 'Bearer') {
    const status = 401
    const message = 'Bad authorization header'
    res.status(status).json({status, message})
    return
  }
  try {
     verifyToken(req.headers.authorization.split(' ')[1])
     next()
  } catch (err) {
    const status = 401
    const message = 'Error: access_token is not valid'
    res.status(status).json({status, message})
  }
})

server.use(router)

server.listen(3000, () => {
  console.log('JSON Server is running')
});
