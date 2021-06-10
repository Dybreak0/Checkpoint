import detailsModel from '../models/person';
import serviceUtil from '../../../common/utils/main-service';

export default {
  namespaced: true,

  state: {
    list: [],
    model: detailsModel.default(),
  },

  getters: {
    list: state => state.list,
    model: state => state.model,
  },

  mutations: {
    LIST(state, payload) {
      state.list = payload || [];
    },

    CLEAR(state) {
      state.list = [];
      state.model = detailsModel.default();
    },

    MODEL(state, payload) {
      state.model = payload || detailsModel.default();
    },

  },

  actions: {
    destroy({ commit, dispatch }, id) {
      return serviceUtil
      .delete(`details/${id}`)
      .then(() => dispatch('list'));
    },

    list({ commit }) {
      commit('LIST', []);

      return serviceUtil
        .list('details')
        .then((list) => {
          commit('LIST', list);
        });
    },

    clear({ commit }) {
      commit('CLEAR');
    },

    new({ commit }) {
      commit('MODEL', null);
    },

    edit({ commit }, id) {
      return serviceUtil
      .find(`details/${id}`)
      .then((model) => {
        commit('MODEL', model);
      });
    },

    save({ getters }) {
      if (getters.model.id) {
        return serviceUtil
        .update('details', getters.model);
      }

      return serviceUtil
        .insert('details', getters.model);
    },

  },
};
