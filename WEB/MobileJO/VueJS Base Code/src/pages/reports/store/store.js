import jobOrderStore from './jobOrder-store';
import assignedCaseStore from './assignedCase-store';
import jobOrderClienRatingStore from './jobOrderClientRating-store';


export default {
  namespaced: true,

  modules: {
    jobOrderStore,
    assignedCaseStore,
    jobOrderClienRatingStore,
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
