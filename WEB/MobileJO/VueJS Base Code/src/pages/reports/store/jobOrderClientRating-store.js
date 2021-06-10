import axios from 'axios'
import moment from 'moment'
import {saveAs} from 'file-saver'
import searchModel from '../models/searchJobOrderClientRating';
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
        options: {},
        errorMessage: '',
    },

    getters: {
        list: state => state.list,
        searchModel: state => state.searchModel,
        options: state => state.options,
        errorMessage: state => state.errorMessage
    },

    mutations: {
        LIST(state, payload) {
            state.list = payload || [];
        },

        ERROR(state, payload) {
            state.errorMessage = payload || null;
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

        ADD_OPTIONS(state, payload) {
            state.options = Object.assign({}, state.options, { [payload.key]: payload.options });
        },
    },

    actions: {
        new({ commit }) {
            commit(constants.SEARCHMODEL, null);
            commit(constants.LIST, []);
        },
        clearFilters({ commit }) {
            commit(constants.CLEAR_FILTERS);
        },
        clear({ commit }) {
            commit(constants.CLEAR);
        },
        loadOptions({ commit }) {
            var url_array = [
                axios.get('/api/DropdownAPI/getApplicationType'),
                axios.get('/api/DropdownAPI/getAccounts')
            ];
            return serviceUtil
                .many(url_array)
                .then((response) => {
                    response[0].data.unshift({value: 0, text: constants.all});

                    commit(constants.ADD_OPTIONS, { key: constants.applicationType, options: response[0].data });
                });            
        },
        list({ commit, getters }) {
            commit(constants.ERROR, null);
            var url = 'api/ReportAPI/getJobOrderClientRatingReport?page=' + getters.searchModel.page +
                                                             '&pageSize=' + getters.searchModel.page_size +
                                                           '&caseNumber=' + getters.searchModel.case_number +
                                                       '&jobOrderNumber=' + getters.searchModel.job_order_number +
                                                      '&applicationType=' + getters.searchModel.application_type +
                                                          '&accountName=' + getters.searchModel.account_name +
                                                           '&reportedBy=' + getters.searchModel.reported_by +
                                                     '&jobOrderDateFrom=' + getters.searchModel.job_order_date_from +
                                                       '&jobOrderDateTo=' + getters.searchModel.job_order_date_to ;
            return serviceUtil
                .list(url)
                .then((response) => {
                    if (response.pagination != undefined) { // check if response has pagination data
                        commit(constants.LIST, response);
                    } else {
                        commit(constants.ERROR, response);
                    }               
                });
        },
        download({ commit, getters }) {
            commit(constants.ERROR, null);
            var url = 'api/ReportAPI/downloadJobOrderClientRatingReport?page=' + getters.searchModel.page +
                                                                  '&pageSize=' + getters.searchModel.page_size +
                                                                '&caseNumber=' + getters.searchModel.case_number +
                                                            '&jobOrderNumber=' + getters.searchModel.job_order_number +
                                                           '&applicationType=' + getters.searchModel.application_type +
                                                                 '&accountName=' + getters.searchModel.account_name +
                                                                '&reportedBy=' + getters.searchModel.reported_by +
                                                          '&jobOrderDateFrom=' + getters.searchModel.job_order_date_from +
                                                            '&jobOrderDateTo=' + getters.searchModel.job_order_date_to ;
            return serviceUtil
                .list(url)
                .then((response) => {
                    if (!(response.includes(constants.networkError))) {
                        var file = new Blob([response], { type: constants.excelApplicationType });
                        var formattedDateTime = moment(new Date()).format(constants.dateFormat);
                        saveAs(file, constants.jobOrderClientRatingReportExcel + formattedDateTime + constants.xls);
                    } else {
                        commit(constants.ERROR, response);
                    }               
                });
        },
    },
};
