import user from './user-store';

export default {
  namespaced: true,

    modules: {
      user
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
