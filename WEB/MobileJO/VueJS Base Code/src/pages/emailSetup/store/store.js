import emailSetup from './emailSetup-store';

export default {
  namespaced: true,

  modules: {
      emailSetup,
  },

  state: {
    loading: false,
  },

  getters: {
    loading: state => state.loading,
  },

  mutations: {
    CLEAR(state) {
      state.loading = false;
    },

  },

  actions: {
    clear({ commit }) {
      commit('CLEAR');
    },

  },
};
