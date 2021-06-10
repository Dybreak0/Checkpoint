import axios from "axios";
import moment from "moment";
import { saveAs } from "file-saver";
import viewModel from "../models/viewJobOrder";
import searchModel from "../models/searchJobOrder";
import serviceUtil from "../../../common/utils/main-service";
import constants from "../../../common/utils/constants";

export default {
  namespaced: true,

  components: {
    constants
  },

  state: {
    list: [],
    searchModel: searchModel.default(),
    viewModel: viewModel.default(),
    options: {},
    errorMessage: null,
    successMessage: null
  },

  getters: {
    list: state => state.list,
    searchModel: state => state.searchModel,
    viewModel: state => state.viewModel,
    options: state => state.options,
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

    VIEWMODEL(state, payload) {
      state.viewModel = payload || viewModel.default();
    },

    CLEAR_FILTERS(state) {
      state.searchModel = searchModel.default();
    },

    CLEAR(state) {
      state.list = [];
      state.searchModel = searchModel.default();
    },

    ADD_OPTIONS(state, payload) {
      state.options = Object.assign({}, state.options, {
        [payload.key]: payload.options
      });
    }
  },

  actions: {
    new({ commit }) {
      commit(constants.SEARCHMODEL, null);
      commit(constants.LIST, []);
    },
    view({ commit }) {
      commit(constants.ERROR, null);
      commit(constants.SUCESSS_MESSAGE, null);
    },
    clearFilters({ commit }) {
      commit(constants.CLEAR_FILTERS);
    },
    clear({ commit }) {
      commit(constants.CLEAR);
    },
    loadOptions({ commit }) {
      var url_array = [
        axios.get("/api/DropdownAPI/getApplicationType"),
        axios.get("/api/DropdownAPI/getStatus")
      ];
      return serviceUtil.many(url_array).then(response => {
        response[0].data.unshift({ value: 0, text: constants.all });
        response[1].data.unshift({ value: 0, text: constants.all });

        commit(constants.ADD_OPTIONS, {
          key: constants.applicationType,
          options: response[0].data
        });
        commit(constants.ADD_OPTIONS, {
          key: constants.status,
          options: response[1].data
        });
      });
    },
    list({ commit, getters }) {
      commit(constants.ERROR, null);
      var url =
        "api/ReportAPI/getJobOrderReport?page=" +
        getters.searchModel.page +
        "&pageSize=" +
        getters.searchModel.page_size +
        "&caseNumber=" +
        getters.searchModel.case_number +
        "&jobOrderNumber=" +
        getters.searchModel.job_order_number +
        "&applicationType=" +
        getters.searchModel.application_type +
        "&status=" +
        getters.searchModel.status +
        "&reportedBy=" +
        getters.searchModel.reported_by +
        "&jobOrderDateFrom=" +
        getters.searchModel.job_order_date_from +
        "&jobOrderDateTo=" +
        getters.searchModel.job_order_date_to;
      return serviceUtil.list(url).then(response => {
        // Check if response has pagination data
        if (response.pagination === undefined) {
          commit(constants.ERROR, response);
        } else {
          commit(constants.LIST, response);
        }
      });
    },
    get({ commit }, id) {
      commit(constants.VIEWMODEL, null);
      commit(constants.ERROR, null);

      return serviceUtil
        .find("api/ReportAPI/getJobOrder?id=" + id)
        .then(response => {
          // Check if response has job order data
          if (response.id === undefined) {
            commit(constants.ERROR, response);
          } else {
            commit(constants.VIEWMODEL, response);
          }
        });
    },
    download({ commit, getters }) {
      commit(constants.ERROR, null);
      var url =
        "api/ReportAPI/downloadJobOrderReport?page=" +
        getters.searchModel.page +
        "&pageSize=" +
        getters.searchModel.page_size +
        "&caseNumber=" +
        getters.searchModel.case_number +
        "&jobOrderNumber=" +
        getters.searchModel.job_order_number +
        "&applicationType=" +
        getters.searchModel.application_type +
        "&status=" +
        getters.searchModel.status +
        "&reportedBy=" +
        getters.searchModel.reported_by +
        "&jobOrderDateFrom=" +
        getters.searchModel.job_order_date_from +
        "&jobOrderDateTo=" +
        getters.searchModel.job_order_date_to;
      return serviceUtil.list(url).then(response => {
        if (!response.includes(constants.networkError)) {
          var file = new Blob([response], {
            type: constants.octetApplicationType
          });
          var formattedDateTime = moment(new Date()).format(
            constants.dateFormat
          );
          saveAs(
            file,
            constants.jobOrderReportExcel + formattedDateTime + constants.xls
          );
        } else {
          commit(constants.ERROR, response);
        }
      });
    },
    downloadAttachment({ commit }, attachment) {
      commit(constants.ERROR, null);
      var url =
        "api/AttachmentAPI/downloadAttachment?id=" +
        attachment.jobOrderId +
        "&fileName=" +
        attachment.fileName +
        "&attachmentType=" +
        attachment.type;
      return serviceUtil.download(url).then(response => {
        if (typeof response !== "string") {
          var file = new Blob([response], {
            type: constants.octetApplicationType
          });
          saveAs(file, attachment.fileName);
        } else {
          commit(constants.ERROR, response);
        }
      });
    },
    revertJO({ commit }, { jobOrderId, jobOrderRevertId, isApproved }) {
      commit(constants.SUCESSS_MESSAGE, null);
      commit(constants.ERROR, null);
      var url = "api/RevertJOAPI/revertJO";
      return serviceUtil
        .insert(url, {
          job_order_id: jobOrderId,
          job_order_revert_id: jobOrderRevertId,
          is_approved: isApproved
        })
        .then(response => {
          if (response.message != undefined) {
            // check if response has pagination data
            commit(constants.SUCESSS_MESSAGE, response.message);
          } else {
            commit(constants.ERROR, response.errorMessage);
          }
        });
    }
  }
};
