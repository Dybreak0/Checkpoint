import response from './response-store';
import template from './template-store';

export default {
    namespaced: true,

    modules: {
        response,
        template,
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
