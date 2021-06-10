import serviceUtil from '../../../../common/utils/main-service';

export default {
    namespaced: true,

    state: {
        list: [],
        model: [],
        roles: {},
        types: {},
        companies: {},
        branches: {},
    },

    getters: {
        list: state => state.list,
        model: state => state.model,
        roles: state => state.roles,
        types: state => state.types,
        companies: state => state.companies,
        branches: state => state.branches,
    },

    mutations: {
        LIST(state, payload) {
            state.list = payload || [];
        },

        MODEL(state, payload) {
            state.model = payload || [];
        },

        ROLES(state, payload) {
            state.roles = payload || [];
        },

        TYPES(state, payload) {
            state.types = payload || [];
        },

        COMPANIES(state, payload) {
            state.companies = payload || [];
        },

        BRANCHES(state, payload) {
            state.branches = payload || [];
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
                .list('api/UserAPI/list')
                .then((list) => {
                    commit('LIST', list);
                });
        },

        user({ commit }, id) {
            commit('MODEL', []);

            return serviceUtil
                .find(`api/UserAPI/user?id=${id}`)
                .then((model) => {
                    commit('MODEL', model);
                });
        },

        search({ commit }, searchValues) {
            var userName = encodeURIComponent(searchValues.UserName);
            var roleID = encodeURIComponent(searchValues.RoleID);
            var companyID = encodeURIComponent(searchValues.CompanyID);
            var page = searchValues.Page;
            var pageSize = searchValues.PageSize;
            commit('LIST', []);

            var data = 'api/UserAPI/list?UserName=' + userName;

            if (roleID !== "all") {
                data += '&RoleID=' + roleID;
            }

            if (companyID !== "all") {
                data += '&CompanyID=' + companyID;
            }

            data += '&Page=' + page;
            data += '&PageSize=' + pageSize;

            return serviceUtil
                .list(data)
                .then((list) => {
                    commit('LIST', list);
                });

        },

        destroy({ commit }, id) {
            commit('LIST', []);
            return serviceUtil
                .delete(`api/UserAPI/delete?id=${id}`)
                .then((list) => {
                    commit('LIST', list);
                });
        },

        add({ commit, getters }) {
            commit('LIST', []);
            return serviceUtil
                .insert('api/UserAPI/add', JSON.stringify(getters.model[0]))
                .then((list) => {
                    commit('LIST', list);
                });
        },

        edit({ commit, getters }) {
            commit('LIST', []);
            return serviceUtil
                .update('api/UserAPI/edit', JSON.stringify(getters.model.data))
                .then((list) => {
                    commit('LIST', list);
                });

        },

        clear({ commit }) {
            commit('CLEAR');
        },

        getRoles({ commit }) {
            commit('ROLES', []);
            return serviceUtil
                .list('api/DropdownAPI/getRoles')
                .then((list) => {
                    commit('ROLES', list);
                });
        },

        getUserTypes({ commit }) {
            commit('TYPES', []);
            return serviceUtil
                .list('api/DropdownAPI/getUserTypes')
                .then((list) => {
                    commit('TYPES', list);
                });
        },

        getCompanies({ commit }) {
            commit('COMPANIES', []);
            return serviceUtil
                .list('api/DropdownAPI/getCompanies')
                .then((list) => {
                    commit('COMPANIES', list);
                });
        },

        getBranches({ commit }, selectedCompanyID) {
            commit('BRANCHES', []);
            return serviceUtil
                .list('api/DropdownAPI/getBranches?selectedCompanyID=' + selectedCompanyID)
                .then((list) => {
                    commit('BRANCHES', list);
                });
        }

    },
};
