import account from './account-store';

export default {
    namespaced: true,

    modules: {
        account,
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
