import searchModel from '../models/searchRevertJO';
import serviceUtil from '../../../common/utils/main-service';
import constants from '../../../common/utils/constants';

export default {
    namespaced: true,

    components: {
        constants
    },

    state: {
        list: [],
        searchModel: searchModel.default(),
        errorMessage: '',
        successMessage: '',
    },

    getters: {
        list: state => state.list,
        searchModel: state => state.searchModel,
        errorMessage: state => state.errorMessage,
        successMessage: state => state.successMessage
    },

    mutations: {
        LIST(state, payload) {
            state.list = payload || [];
        },

        ERROR(state, payload) {
            state.errorMessage = payload || null;
        },

        SUCESSS_MESSAGE(state, payload) {
            state.successMessage = payload || null;
        },
        SEARCHMODEL(state, payload) {
            state.searchModel = payload || searchModel.default();
        },

        CLEAR_FILTERS(state) {
            state.searchModel = searchModel.default();
        },

        CLEAR(state) {
            state.list = [];
            state.searchModel = searchModel.default();
        },
    },

    actions: {
        new({ commit }) {
            commit(constants.SEARCHMODEL, null);
            commit(constants.LIST, []);
            commit(constants.SUCESSS_MESSAGE, null);
            commit(constants.ERROR, null);
        },
        clearFilters({ commit }) {
            commit(constants.CLEAR_FILTERS);
        },
        clear({ commit }) {
            commit(constants.CLEAR);
        },
        list({ commit, getters }) {
            commit(constants.ERROR, null);
            var url = 'api/RevertJOAPI/getRevertJOList?page=' + getters.searchModel.page +
                                                 '&pageSize=' + getters.searchModel.page_size +
                                           '&jobOrderNumber=' + getters.searchModel.job_order_number +
                                               '&reportedBy=' + getters.searchModel.reported_by;
            return serviceUtil
                .list(url)
                .then((response) => {
                    if (response.pagination === undefined) { // check if response has pagination data
                        commit(constants.ERROR, response);                        
                    } else {
                        commit(constants.LIST, response);
                    }               
                });
        },  
        revertJO({ commit }, { jobOrderId, jobOrderRevertId, isApproved }) {
            commit(constants.SUCESSS_MESSAGE, null);
            commit(constants.ERROR, null);
            var url = 'api/RevertJOAPI/revertJO';
            return serviceUtil
                .insert(url, {
                    job_order_id : jobOrderId,
                    job_order_revert_id : jobOrderRevertId,
                    is_approved : isApproved
                })
                .then((response) => {
                    if (response.message != undefined) { // check if response has pagination data
                        commit(constants.SUCESSS_MESSAGE, response.message);                        
                    }
                    else {
                        commit(constants.ERROR, response.errorMessage);
                    }               
                });
        }     
    },
};
