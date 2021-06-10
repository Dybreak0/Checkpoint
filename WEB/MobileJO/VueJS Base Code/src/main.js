import 'es6-promise/auto';
import Vue from 'vue';
import Vuex from 'vuex';
import { sync } from 'vuex-router-sync';
import VueRouter from 'vue-router';
import VueSignature from "vue-signature-pad";
import ElLoading from 'element-loading';

import './styles/main.scss';
import App from './app';
import appModule from './app-module';
import { i18n, loadLanguageAsync } from './i18n/lang';
import Vuetify from 'vuetify';
import 'vuetify/dist/vuetify.min.css';
import moment from 'moment';
import format from 'vue-format';
import "material-design-icons/iconfont/material-icons.css";
import '@fortawesome/fontawesome-free/css/all.css' 
import IdleVue from 'idle-vue'
 
const eventsHub = new Vue()

Vue.use(IdleVue, {
    eventEmitter: eventsHub,
    idleTime: 1800000
})

Vue.use(Vuetify)

Vue.use(VueRouter);

Vue.use(VueSignature);

Vue.use(Vuex);

Vue.use(ElLoading);

Vue.use(format);

Vue.filter('formatDate', function(value) {
  if (value) {
    return moment(String(value)).format('MM/DD/YYYY')
  }
});

Vue.filter('formatDateTime', function(value) {
  if (value) {
    return moment(String(value)).format('MM/DD/YYYY hh:mm A')
  }
});

// setup components, filters, directives and mixins
appModule.setupComponentsFiltersDirectivesAndMixins();

// Routing logic
const myRoute = new VueRouter({
  routes: appModule.routes,
  linkExactActiveClass: 'active',
  scrollBehavior() {
    return { x: 0, y: 0 };
  },
});

// Store logic
const myStore = new Vuex.Store({
  modules: appModule.stores,
  mutations: {
    INITIALIZE_STORE(state) {
      // check if ID exists
      if (localStorage.getItem('store')) {
        // replace the state object with the stored item in localstorage
        this.replaceState(
          Object.assign(state, JSON.parse(localStorage.getItem('store'))),
        );
      }
    },
  },
});

// subscribe to store updates
myStore.subscribe((mutation, state) => {
  // store the state object as JSON string
  localStorage.setItem('store', JSON.stringify(state));
});

Vue.config.productionTip = process.env.NODE_ENV === 'production';

// get store from local storage and set to state
myStore.commit('INITIALIZE_STORE');

// before each route, check lang
// if lang selected is not yet loaded, load file and set in i18n
myRoute.beforeEach((to, from, next) => {
  const lang = myStore.state.lang.appLocale || 'en';
  loadLanguageAsync(lang).then(() => next());
});

// sync route to store
sync(myStore, myRoute);

// eslint-disable-next-line
export const app = new Vue({
  el: '#app',
  store: myStore,
  router: myRoute,
  i18n,
  template: '<app/>',
  render: h => h(App),

});

