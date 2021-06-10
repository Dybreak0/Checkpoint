import serviceUtil from '../../../common/utils/main-service';


export default {
    namespaced: true,

    state: {
        list: [],
        object: [],
        message: ""
    },

    getters: {
        list: state => state.list,
        object: state => state.object,
        message: state => state.message,
    },

    mutations: {
        LIST(state, payload) {
            state.list = payload || [];
        },

        MESSAGE(state, payload) {
            state.message = payload || "";
        },

        OBJECT(state, payload) {
            state.object = payload || [];
        },


        CLEAR(state) {
            state.list = [];
            state.object = [];
        },

    },

    actions: {
        list({ commit }) {
            commit('LIST', []);
            commit('MESSAGE', "");

            return serviceUtil
                .list(`api/EmailJOAPI/list`)
                .then((list) => {
                    commit('LIST', list.data.data);
                    commit('MESSAGE', list.message);
                });
        },

        save({ commit, getters }) {
            commit('MESSAGE', "");
            return serviceUtil
                .insert('api/EmailJOAPI/save', getters.object[0])
                .then((list) => {
                    commit('MESSAGE', list.message);
                });
                
        },

        clear({ commit }) {
            commit('CLEAR');
        },
    },
};
