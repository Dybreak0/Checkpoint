import moment from 'moment'
import serviceUtil from '../../../common/utils/main-service';
import { saveAs } from 'file-saver'
import constants from '../../../common/utils/constants';
import jsPDF from 'jspdf';
import JsPDFAutotable from 'jspdf-autotable'

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

        find({ commit }, responseID) {
            commit('MODEL', []);
            return serviceUtil
                .find(`api/ResponseAPI/find?responseID=${responseID}`)
                .then((model) => {
                    commit('MODEL', model);
                });
        },

         search({ commit }, searchValues) {
            var title = encodeURIComponent(searchValues.Title);
            var category = encodeURIComponent(searchValues.Category);
            var startDate = searchValues.StartDate;
            var endDate = searchValues.EndDate;
            var page = searchValues.Page;
            var pageSize = searchValues.PageSize;
            commit('LIST', []);

            var data = ``;

            var data = 'api/ResponseAPI/list?Title=' + title + '&Category=' + category;

            if (startDate != "") {
                data += '&StartDate=' + startDate;
            }

            if (endDate != "") {
                data += '&EndDate=' + endDate;
            }

            data += '&Page=' + page;
            data += '&PageSize=' + pageSize;

            return serviceUtil
                .list(data)
                .then((list) => {
                    commit('LIST', list);
                });
        },

        downloadExcel({ commit }, searchValues) {

            commit(constants.ERROR, null);
            var title = encodeURIComponent(searchValues.Title);
            var category = encodeURIComponent(searchValues.Category);
            var startDate = searchValues.StartDate;
            var endDate = searchValues.EndDate;
            var page = searchValues.Page;
            var pageSize = searchValues.PageSize;

            var data = 'api/ResponseAPI/downloadResponseSummary?Title=' + title + '&Category=' + category;

            if (startDate != "") {
                data += '&StartDate=' + startDate;
            }

            if (endDate != "") {
                data += '&EndDate=' + endDate;
            }

            data += '&Page=' + page;
            data += '&PageSize=' + pageSize;

            return serviceUtil
                .list(data)
                .then((response) => {

                    if (!(response.includes(constants.networkError))) {
                        var file = new Blob([response], { type: constants.octetApplicationType });
                        var formattedDateTime = moment(new Date()).format(constants.dateFormat);
                        saveAs(file, constants.responseReportFileName + formattedDateTime + constants.xls);
                    } else {
                        commit(constants.ERROR, response);
                    }

                });
        },

        downloadPDF({ commit }, searchValues) {

            var title = encodeURIComponent(searchValues.Title);
            var category = encodeURIComponent(searchValues.Category);
            var startDate = searchValues.StartDate;
            var endDate = searchValues.EndDate;
            var page = searchValues.Page;
            var pageSize = searchValues.PageSize;
            commit('LIST', []);

            var data = 'api/ResponseAPI/downloadResponseSummaryPDF?Title=' + title + '&Category=' + category;

            if (startDate != "") {
                data += '&StartDate=' + startDate;
            }

            if (endDate != "") {
                data += '&EndDate=' + endDate;
            }

            data += '&Page=' + page;
            data += '&PageSize=' + pageSize;

            return serviceUtil
                .list(data)
                .then((list) => {
                    var responses = list.data.data;
                    var columns = ["Title", "Company-Branch", "Category", "Date Submitted", "Submitted By", "Status"];
                    var rows = [];

                    if (null == responses || responses.length == 0) {
                        commit(constants.ERROR, list.message);
                    } else {
                        commit('LIST', list);
                        for (let i = 0; i < responses.length; i++) {
                            var rowContent = [];
                            rowContent.push(responses[i].title);
                            rowContent.push(responses[i].company_name + " - " + responses[i].branch);
                            rowContent.push(responses[i].category);
                            rowContent.push(responses[i].date_submitted);
                            rowContent.push(responses[i].submitted_by);
                            rowContent.push(responses[i].isApproved ? "Approved" : "Submitted");

                            rows.push(rowContent);
                        }

                        var formattedDateTime = moment(new Date()).format(constants.dateFormat);

                        const doc = new jsPDF('l', 'pt', 'a4');

                        var header = function (data) {
                            doc.setFontSize(18);
                            doc.setTextColor(40);
                            doc.setFontStyle('normal');
                            doc.text("Response Report", data.settings.margin.left, 50);
                        };

                        doc.autoTable(columns, rows, { margin: { top: 80 }, didDrawPage: header });
                        doc.save(constants.responseReportFileName + formattedDateTime + ".pdf");
                    }
                });
        },

        editResponse({ commit, getters }) {
            commit('LIST', []);
            return serviceUtil
                .update('api/ResponseAPI/editResponse', JSON.stringify(getters.model.data.data))
                .then((list) => {
                    commit('LIST', list);
                });

        },

        clear({ commit }) {
            commit('CLEAR');
        },

    },
};
