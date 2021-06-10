import axios from 'axios';
import error from '@/pages/error';
import constants from '@/common/utils/constants';

// url to be used (will get from dev, prod or test env config file)
axios.defaults.baseURL = process.env.BASE_URL;

axios.interceptors.request.use(function (config) {
    var token = localStorage.getItem('token') || '';

    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
});

// Add a response interceptor
axios.interceptors.response.use(function (response) {
 
    if (response.status === 200) {
        
    // interceptor response
    // if 200 on
    // but errorcode = true
    //   redirect to error page
    //   set error msg(popup)
    }

  return response;
}, function (error) {
    switch (error.response.status) {
        case 401:
            if(localStorage.getItem('token')){
                var refreshToken = JSON.parse(localStorage.getItem('store')).login.refreshToken || '';
                var userName = JSON.parse(localStorage.getItem('store')).login.userName || '';
                var data = {
                    "refresh_token": refreshToken,
                    "username": userName
                };

                return axios.post('api/refresh', data).then((response) => {
                    localStorage.setItem('token', response.data.access_token);
                    return axios.request(error.config)
                })
            }else{
                window.location = '#/login';
            }
            break;
        case 404:
            window.location = '/error/404';
            break;
        // add other errors here
        default:
        // default error view
    }
    
    return Promise.reject(error);
});

const mainService = {
  list(url) {
    return new Promise((resolve) => {
      axios.get(url)
      .then((response) => {
        resolve(response.data);
      }).catch((error) => {
        if ((error.message).includes(constants.undefined)) {
          resolve(constants.networkError)
        } else {
          resolve(error.response.data);
        }
      });
    });
  },

  find(url) {
    return new Promise((resolve) => {
      axios.get(url)
      .then((response) => {
        resolve(response.data);
      }).catch((error) => {
        if (((error.message).includes(constants.undefined))) {
          resolve(constants.networkError)
        } else {
          resolve(error.response.data);
        }
      });
    });
  },

  delete(url) {
    return new Promise((resolve) => {
      axios.delete(url)
      .then((response) => {
        resolve(response.data);
      }).catch((error) => {
          if ((error.message).includes(constants.undefined)){
              resolve(constants.networkError)
          } else {
              resolve(error.response.data);
          }
      });
    });
  },

  update(url, data) {
    return new Promise((resolve) => {
      axios.put(
        url,
        data,
        {
          headers: {
            'Accept': 'application/json',
            'Content-type': 'application/json',
          },
        },
      )
      .then((response) => {
        resolve(response.data);
      }).catch((error) => {
          if ((error.message).includes(constants.undefined)){
              resolve(constants.networkError)
          } else {
              resolve(error.response.data);
          }
      });
    });
  },

  insert(url, data) {
    return new Promise((resolve) => {
      axios.post(
        url,
        data,
        {
          headers: {
            'Accept': 'application/json',
            'Content-type': 'application/json',
          },
        },
      )
      .then((response) => {
        resolve(response.data);
      }).catch((error) => {
        if ((error.message).includes(constants.undefined)) {
          resolve(constants.networkError)
        } else {
          resolve(error.response.data);
        }
      });
    });
  },

  login(url, data) {
    return new Promise((resolve) => {
      axios.post(url
        , data
        , {
          headers: {
            'Content-type': 'application/x-www-form-urlencoded',
          },
        },
      )
      .then((response) => {
          resolve(response.data);
      }).catch((response) => {
        if ((response.message).includes(constants.undefined)) {
          resolve(constants.networkError)
        } else {
          resolve(response.response.data);
        }
      });
    });
  },
  
  many(url_array) {
    return new Promise((resolve) => {
      axios.all(url_array)
      .then(axios.spread((...responses) => {
        resolve(responses);
      }));
    });
  },

  download(url) {
    return new Promise((resolve) => {
      axios.get(url, {
        responseType: 'arraybuffer',
      })
      .then((response) => {
        resolve(response.data);
      }).catch((error) => {
        if ((error.message).includes(constants.undefined)) {
          resolve(constants.networkError)
        } else {
            resolve(constants.fileNotFound);
        }
      });
    });
  },
};

export default mainService;
