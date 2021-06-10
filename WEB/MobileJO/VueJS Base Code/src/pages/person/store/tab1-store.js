import personModel from '../models/person';
import roles from '../person-enumerators/person-role';
import serviceUtil from '../../../common/utils/main-service';

export default {
  namespaced: true,

  state: {
    list: [],
    model: personModel.default(),
    options: {},
  },

  getters: {
    list: state => state.list,
    model: state => state.model,
    options: state => state.options,
  },

  mutations: {
    LIST(state, payload) {
      state.list = payload || [];
    },

    MODEL(state, payload) {
      state.model = payload || personModel.default();
    },

    CLEAR(state) {
      state.list = [];
      state.model = personModel.default();
      state.options = {};
    },

    ADD_OPTIONS(state, payload) {
      state.options = Object.assign({}, state.options, { [payload.key]: payload.options });
    },
  },

  actions: {
    new({ commit }) {
      commit('MODEL', null);
    },

    list({ commit }) {
      commit('LIST', []);

      return serviceUtil
          .list(`api/EmailJOAPI/list`)
        .then((list) => {
          commit('LIST', list);
        });
    },

    edit({ commit }, id) {
      return serviceUtil
      .find(`list/${id}`)
      .then((model) => {
        commit('MODEL', model);
      });
    },

    destroy({ commit, dispatch }, id) {
      return serviceUtil
        .delete(`list/${id}`)
        .then(() => dispatch('list'));
    },

    save({ getters }) {
      if (getters.model.id) {
        return serviceUtil
        .update('list', getters.model);
      }

      return serviceUtil
      .insert('list', getters.model);
    },

    clear({ commit }) {
      commit('CLEAR');
    },

    loadOptions({ commit }) {
      commit('ADD_OPTIONS', { key: 'roles', options: roles.list() });
    },
  },
};
