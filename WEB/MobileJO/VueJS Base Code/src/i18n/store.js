import { app } from '../main';

// localstorage data
const localStorageStore = JSON.parse(localStorage.getItem('store'));
const localStorageLang = localStorageStore === null ? null : localStorageStore.lang;

export default {
  namespaced: true,
  state: {
    appLocale: localStorageLang != null ? localStorageLang.appLocale : 'en',
  },

  getters: {
    appLocale: state => state.appLocale,
  },

  mutations: {
    SET_LANG(state, payload) {
      app.$i18n.locale = payload;
      state.appLocale = payload;
    },
  },

  actions: {
    // will be used if onload all language files are loaded
    setLang({ commit }, payload) {
      commit('SET_LANG', payload);
    },
  },

};
