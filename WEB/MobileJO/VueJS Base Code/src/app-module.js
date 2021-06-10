import Vue from 'vue';

// layouts
import MainLayout from './common/layout/main-layout';
import ErrorLayout from './common/layout/error-layout';
import LoginView from './pages/login/login';
import ForgotPasswordView from './pages/forgotPassword/forgot-password';
import ChangePasswordView from './pages/forgotPassword/change-password';

// modules
import person from './pages/person/index';
import common from './common/index';
import login from './pages/login/index';
import forgotPassword from './pages/forgotPassword/index';
import error from './pages/error/index';
import lang from './i18n/index';

import user from './pages/maintenance/user/index';
import account from './pages/maintenance/account/index';
import emailSetup from './pages/emailSetup/index';
import reports from './pages/reports/index';
import revertJO from './pages/revertJO/index';

import questionnaire from './pages/questionnaire/index';

import application from './pages/application/index'
// main modules to be loaded during login
const modules = {
  common,
  login,
  lang
};

// main modules
const mainModules = {
    person,
    user,
    account,
    emailSetup,
    reports,
    revertJO,
    forgotPassword,
    error,
    questionnaire,
    application
};

// error page modules
const errorModules = {
  error,
};

// start - boilerplate code
const exists = el => !!el;

// setup all components, directives, filters and mixins
function setupComponentsFiltersDirectivesAndMixins() {
  // components
  Object.keys(modules)
    .map(key => modules[key].components)
    .filter(exists)
    .forEach((components) => {
      components.forEach((component) => {
        Vue.component(component.name, component);
      });
    });

  Object.keys(mainModules)
    .map(key => mainModules[key].components)
    .filter(exists)
    .forEach((components) => {
      components.forEach((component) => {
        Vue.component(component.name, component);
      });
    });

  Object.keys(errorModules)
    .map(key => errorModules[key].components)
    .filter(exists)
    .forEach((components) => {
      components.forEach((component) => {
        Vue.component(component.name, component);
      });
    });

  // filters
  Object.keys(mainModules)
    .map(key => mainModules[key].filters)
    .filter(exists)
    .forEach((components) => {
      components.forEach((filter) => {
        Vue.filter(filter.name, filter.implementation);
      });
    });

  // directives
  // Object.keys(modules)
  //   .map(key => modules[key].directives)
  //   .filter(exists)
  //   .forEach((directives) => {
  //     directives.forEach((directive) => {
  //       Vue.directive(directive.name, directive.implementation);
  //     });
  //   });

  Object.keys(mainModules)
    .map(key => mainModules[key].mixins)
    .filter(exists)
    .forEach((mixins) => {
      mixins.forEach((mixin) => {
        Vue.mixin(mixin);
      });
    });
}

// declare all routes
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
    path: '/forgotPassword',
    component: ForgotPasswordView,
    children: Object.keys(modules)
    .filter(key => !!modules[key].routes)
    .map(key => modules[key].routes)
    .reduce((a, b) => a.concat(b), []),
  },
  {
    path: '/changePassword/:userId/:token',
    component: ChangePasswordView,
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

// declare all stores
const buildStores = () => {
  const output = {};
  Object.keys(modules)
    .filter(key => !!modules[key].store)
    .forEach((key) => {
      output[key] = modules[key].store;
    });

  Object.keys(mainModules)
    .filter(key => !!mainModules[key].store)
    .forEach((key) => {
      output[key] = mainModules[key].store;
    });

  return output;
};

export default {
  setupComponentsFiltersDirectivesAndMixins,
  routes,
  stores: buildStores(),
};

// end - boilerplate code
