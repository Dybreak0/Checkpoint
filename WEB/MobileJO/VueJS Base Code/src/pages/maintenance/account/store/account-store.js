import serviceUtil from '../../../../common/utils/main-service';

export default {
    namespaced: true,

    state: {
        list: [],
        model: [],
    },

    getters: {
        list: state => state.list,
        model: state => state.model,
    },

    mutations: {
        LIST(state, payload) {
            state.list = payload || [];
        },

        MODEL(state, payload) {
            state.model = payload || [];
        },


        CLEAR(state) {
            state.list = [];
            state.model = [];
        },

    },

    actions: {

        list({ commit }) {
            commit('LIST', []);

            return serviceUtil
                .list('api/Account/list')
                .then((list) => {
                    commit('LIST', list);
                });
        },

        account({ commit }, id) {
            commit('MODEL', []);

            return serviceUtil
                .find(`api/AccountAPI/account?id=${id}`)
                .then((model) => {
                    commit('MODEL', model);
                });
        },

        search({ commit }, searchValues) {
            var name = encodeURIComponent(searchValues.Name);
            var page = searchValues.Page;
            var pageSize = searchValues.PageSize;

            commit('LIST', []);

            return serviceUtil
                .list('api/AccountAPI/list?Name=' + name + '&Page='+ page +'&PageSize='+ pageSize)
                .then((list) => {
                    commit('LIST', list);
                });
        },

        destroy({ commit }, id) {

            return serviceUtil
                .delete(`api/AccountAPI/delete?id=${id}`).then((list) => {
                    commit('LIST', list);
                });;
        },

        add({ commit , getters }) {
            commit('LIST', []);
            return serviceUtil
                .insert('api/AccountAPI/add', JSON.stringify(getters.model[0]))
                .then((list) => {
                    commit('LIST', list);
                });;
        },

        edit({ commit, getters }) {
            commit('LIST', []);
            return serviceUtil
                .update('api/AccountAPI/edit', JSON.stringify(getters.model.data)).then((list) => {
                    commit('LIST', list);
                });
        },

        clear({ commit }) {
            commit('CLEAR');
        },
        
    },
};
