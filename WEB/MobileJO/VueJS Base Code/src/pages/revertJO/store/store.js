import revertJOStore from './revertJO-store';


export default {
  namespaced: true,

  modules: {
    revertJOStore
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
