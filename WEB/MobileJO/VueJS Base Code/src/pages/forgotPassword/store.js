import serviceUtil from '../../common/utils/main-service';


export default {
    namespaced: true,

    state: {
        list: [],
        model: []
    },

    getters: {
        list: state => state.list,
        model: state => state.model
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
        sendRequest({ commit }, email) {
           var emailModel = {};
           emailModel = {
               "email_address" : email
           }
           
            commit('LIST', []);
            return serviceUtil
                .insert('api/ForgotPasswordAPI/add', JSON.stringify(emailModel))
                .then((list) => {
                    commit('LIST', list);
                });
        },
        checkValidity({ commit }, params) {
            commit('LIST', []);
            return serviceUtil
                .insert('api/ForgotPasswordAPI/check_validity', JSON.stringify(params))
                .then((list) => {
                    commit('LIST', list);
                });
        },

        resetPassword({ commit, getters }) {
            commit('LIST', []);
            return serviceUtil
                .insert('api/ForgotPasswordAPI/reset_password', JSON.stringify(getters.model[0]))
                .then((list) => {
                    commit('LIST', list);
                });
        }
    },
};
