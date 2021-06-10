**Vue, Vuex with Element CRUD application in memory**

Built with:
- [Vuejs](https://vuejs.org/)
- [Vuex](https://vuex.vuejs.org/en/)
- [Element](http://element.eleme.io/)
- [Json-sever] (https://github.com/typicode/json-server)

**Getting Started**
- Download and install [Node](https://nodejs.org/en/) LTS version
- Run `npm install` to install the dependencies
- Run `npm start` to start the app
- Set current directory to `json-manager`
  Run `node server.js` to start the server

**Features**
- Implements i18n using lazy loading
- Intercept request and response
- Implements routing
- Implements axios
- Implements localstorage

**HTTP requests through axios**
HTTP requests found in `main-service.js`.
Each function returns a promise after calling axios API and resolving the result.
Base url used depends on what environment process the application is running at.
  axios.defaults.baseURL = process.env.BASE_URL;
    - BASE_URL is determined in one of the following config files: prod.env, dev.env, test.env

**Back-end server uses json-server**
JSON sample data used in this code is in `db.json` file under `json-manager` folder.
Configuration of the server is found in `server.js` file.

**Interceptors**
Request and response interceptors are found in `main-service.js`.

Request Interceptor
  - if `token` is available in localstorage, it is added in the request headers authorization
Response Interceptor
  - id response status is 200, errorCode is checked and managed
  - if response status is not 200, error response status is managed depending on the status
  and promise is rejected

**Routing**
Declaration of all routes is in `app-module.js`
For each parent path a main layout component is set.
Each child route will be displayed within the assigned main component for each parent path.
  const routes = [
    {
      path: '/login',
      component: LoginView,
      children: Object.keys(modules)
      .filter(key => !!modules[key].routes)
      .map(key => modules[key].routes)
      .reduce((a, b) => a.concat(b), []),
    },
    {
      path: '/',
      component: MainLayout,
      children: Object.keys(mainModules)
      .filter(key => !!mainModules[key].routes)
      .map(key => mainModules[key].routes)
      .reduce((a, b) => a.concat(b), []),
    },
    {
      path: '*',
      component: ErrorLayout,
      children: Object.keys(errorModules)
      .filter(key => !!errorModules[key].routes)
      .map(key => errorModules[key].routes)
      .reduce((a, b) => a.concat(b), []),
    },
  ];

**i18n using lazy loading**
Localization is implemented through vue-i18n library and using lazy loading implementation.
Language files are not loaded all at once during application start, but are loaded only when it is the language selected.
Language related files are in `src\i18n` directory.
  - `~.json` files are the language files
  - `lang.js` file contains the configuration of i18n and the instantiation of VueI18n
  - `store.js` file contains the state, actions and mutations related to language loading
In `main.js`, for each route language currently selected is checked.
If language is not yet loaded, it is loaded and used.
  myRoute.beforeEach((to, from, next) => {
    const lang = myStore.state.lang.appLocale || 'en';
    loadLanguageAsync(lang).then(() => next());
  });

**LocalStorage**
Setting of `store` key in localStorage
  - `store` key in localStorage is saved when update in store occurs.
  This is done through the code below in `main.js`.
    // subscribe to store updates
    myStore.subscribe((mutation, state) => {
      // store the state object as JSON string
      localStorage.setItem('store', JSON.stringify(state));
    });
Retrieval of `store` key from localStorage
  - on load application, if `store` key exist in localStorage, current state is replaced with what is saved in `store` key in localStorage
    // check if ID exists
    if (localStorage.getItem('store')) {
      // replace the state object with the stored item in localstorage
      this.replaceState(
        Object.assign(state, JSON.parse(localStorage.getItem('store'))),
      );
    }

**Build front-end code through webpack and place built files to back-end**
[Assumed folder structure between front-end and back-end codes]
---------------
Front-end code
---------------
VUE.js/git/
	| - /build
		| - build.js
		| - utils.js
		| - webpack.base.conf.js
		| - webpack.dev.conf.js
		| - webpack.prod.conf.js
		| - webpack.test.conf.js
	| - /config
		| - dev.env.js
		| - index.js
		| - prod.env.js
		| - test.env.js
	| - /dist
	| - /json-manager
	| - /src
	| - Index.cshtml
	| - index.html

---------------
Back-end code
---------------
VUE.js/git-netcore/
	| - /ASP.Net Core Base Code
		| -/BaseCode.API
			| -/src
				| -/BaseCode.API
					| -/View
						| -/Home
							| - Index.cshtml
						| -/Error
					| -/wwwroot
						| -/swagger
						| -/static
							| -/css
							| -/fonts
							| -/js
			| -BaseCode.sln

[Steps]
1. Add Index.cshtml file in front-end code.
	This will serve as a template of the resulting cshtml file after webpack build.
2. In webpack.prod.conf.js, do the following:
	a. Set template to 'Index.cshtml'.
		This will lookup for Index.cshtml file in the project's root directory.
	b. Set value for minify.
		It can be set to false, no minification.
		If set to true, can set options like below.
			minify: { // can also set minify to false
				removeComments: true,
				collapseWhitespace: true,
				removeAttributeQuotes: true
				// more options:
				// https://github.com/kangax/html-minifier#options-quick-reference
			}
3. In config/index.js, do the following:
	a. Change the value of build.index to expected location of Index.cshtml file in back-end.
		e.g. index: path.resolve(__dirname, '../../git-netcore/ASP.Net Core Base Code/src/BaseCode.API/View/Home/Index.cshtml'),
		- During webpack build, Index.cshtml will be created with the injected script tags in the specified location in 'build.index'
	b. Change the value of build.assetsRoot to wwwroot folder in back-end.
		e.g. assetsRoot: path.resolve(__dirname, '../../git-netcore/ASP.Net Core Base Code/src/BaseCode.API/wwwroot'),
		- During webpack build, static folder containing js and css files will be created in the specified location in 'build.assetsRoot'
