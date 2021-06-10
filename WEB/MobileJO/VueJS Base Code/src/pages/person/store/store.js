import tab1 from './tab1-store';
import tab2 from './tab2-store';

export default {
  namespaced: true,

  modules: {
    tab1,
    tab2,
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
