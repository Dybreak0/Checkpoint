import installment from './installment-store';
import template from './template-store';

export default {
    namespaced: true,

    modules: {
        installment,
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
