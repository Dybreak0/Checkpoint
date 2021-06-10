import axios from "axios";
import moment from "moment";
import { saveAs } from "file-saver";
import serviceUtil from "../../../common/utils/main-service";
import searchListModel from "../models/searchInstallmentApplication";
import searchPendingModel from "../models/searchPendingApplication";
import constants from "../../../common/utils/constants";

export default {
  namespaced: true,
  components: {
    constants
  },
  state: {
    list: [],
    model: [],
    branches: {},
    regions: {},
    cities: {},

    searchListModel: searchListModel.default(),
    searchPendingModel: searchPendingModel.default(),
    errorMessage: null,
    successMessage: null
  },

  getters: {
    list: state => state.list,
    model: state => state.model,
    branches: state => state.branches,
    regions: state => state.regions,
    cities: state => state.cities,

    searchListModel: state => state.searchListModel,
    searchPendingModel: state => state.searchPendingModel,
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
    SEARCH_LIST_MODEL(state, payload) {
      state.searchListModel = payload || searchListModel.default();
    },
    SEARCH_PENDING_MODEL(state, payload) {
      state.searchPendingModel = payload || searchPendingModel.default();
    },
    MODEL(state, payload) {
      state.model = payload || [];
    },
    BRANCHES(state, payload) {
      state.branches = payload || [];
    },
    REGIONS(state, payload) {
      state.regions = payload || [];
    },
    CITIES(state, payload) {
      state.cities = payload || [];
    },
    CLEAR_FILTERS(state) {
      state.searchListModel = searchListModel.default();
      state.searchPendingModel = searchPendingModel.default();
    },
    CLEAR(state) {
      state.list = [];
      state.model = [];
      state.searchListModel = searchListModel.default();
      state.searchPendingModel = searchPendingModel.default();
    }
  },

  actions: {
    getBranches({ commit }) {
      commit("BRANCHES", []);
      return serviceUtil
        .list("api/DropdownAPI/getBranchesByCompanyID")
        .then(list => {
          commit("BRANCHES", list);
        });
    },
    getRegion({ commit }) {
      commit("REGIONS", []);
      return serviceUtil.list("api/DropdownAPI/getAllRegion").then(list => {
        commit("REGIONS", list);
      });
    },
    getCity({ commit }, id) {
      commit("CITIES", []);
      return serviceUtil
        .list(`api/DropdownAPI/getCityByRegionID?id=${id}`)
        .then(list => {
          commit("CITIES", list);
        });
    },
    createLoan({ commit, getters }) {
      commit("LIST", []);
      return serviceUtil
        .insert("api/LoanAPI/createLoanApplication", JSON.stringify(getters.model[0]))
        .then(list => {
          commit("LIST", list);
        });
    },

    new({ commit }) {
      commit("SEARCH_LIST_MODEL", null);
      commit("LIST", []);
    },
    view({ commit }) {
      commit("ERROR", null);
      commit("SUCESSS_MESSAGE", null);
    },
    clearFilters({ commit }) {
      commit("CLEAR_FILTERS");
    },
    listLoan({ commit, getters }) {
      commit("ERROR", null);
      commit("LIST", []);
      var url =
        "api/LoanAPI/listLoanApplication?Page=" +
        getters.searchListModel.page +
        "&PageSize=" +
        getters.searchListModel.page_size +
        "&ApplicationNo=" +
        getters.searchListModel.application_no +
        "&CreatedBy=" +
        getters.searchListModel.created_by_name +
        "&Status=" +
        getters.searchListModel.status +
        "&ClientName=" +
        getters.searchListModel.client_name +
        "&DateFrom=" +
        getters.searchListModel.date_from +
        "&DateTo=" +
        getters.searchListModel.date_to;
      return serviceUtil.list(url).then(response => {
        // Check if response has pagination data
        if (response.pagination === undefined) {
          commit("ERROR", response);
        } else {
          commit("LIST", response);
        }
      });
    },
    listPendingLoan({ commit, getters }) {
      commit("ERROR", null);
      commit("LIST", []);
      var url =
        "api/LoanAPI/listPendingLoanApplication?Page=" +
        getters.searchPendingModel.page +
        "&PageSize=" +
        getters.searchPendingModel.page_size +
        "&ApplicationNo=" +
        getters.searchPendingModel.application_no +
        "&RoleID=" +
        getters.searchPendingModel.role +
        "&BranchID=" +
        getters.searchPendingModel.branch_id +
        "&ClientName=" +
        getters.searchPendingModel.client_name +
        "&CreatedBy=" +
        getters.searchPendingModel.created_by_name +
        "&DateFrom=" +
        getters.searchPendingModel.date_from +
        "&DateTo=" +
        getters.searchPendingModel.date_to;

      return serviceUtil.list(url).then(response => {
        // Check if response has pagination data
        if (response.pagination === undefined) {
          commit("ERROR", response);
        } else {
          commit("LIST", response);
        }
      });
    },
    getLoan({ commit }, id) {
      commit("ERROR", null);
      commit("MODEL", null);
      return serviceUtil
        .find("api/LoanAPI/getLoanApplication?id=" + id)
        .then(response => {
          // Check if response has job order data
          console.log(response);
          if (response.loan_id === undefined) {
            commit("ERROR", response);
          } else {
            commit("MODEL", response);
          }
        });
    },

    updateLoan({ commit, getters }) {
      console.log(JSON.stringify(getters.model));
      commit('LIST', []);
      return serviceUtil
          .update("api/LoanAPI/updateLoanApplication", JSON.stringify(getters.model))
          .then((list) => {
              commit('LIST', list);
          });
    },
    deleteLoan({ commit }, id) {
      commit('LIST', []);
      return serviceUtil
          .delete(`api/LoanAPI/deleteLoanApplication?id=${id}`)
          .then((list) => {
              commit('LIST', list);
          });
    },
    updateLoanStatus({ commit }, { loanID, isApproved }) {
      commit(constants.SUCESSS_MESSAGE, null);
      commit(constants.ERROR, null);
      var url = "api/LoanAPI/updateLoanStatus";
      return serviceUtil
        .update(url, {
          list_loan_id: loanID,
          loan_status: isApproved,
        })
        .then(response => {
          if (response.message != undefined) {
            commit(constants.SUCESSS_MESSAGE, response.message);
          } else {
            commit(constants.ERROR, response.errorMessage);
          }
        });
    },
    download({ commit, getters }) {
      commit(constants.ERROR, null);
      var url =
        "api/LoanAPI/loanListToExcel?Page=" +
        getters.searchListModel.page +
        "&PageSize=" +
        getters.searchListModel.page_size +
        "&ApplicationNo=" +
        getters.searchListModel.application_no +
        "&CreatedBy=" +
        getters.searchListModel.created_by_name +
        "&Status=" +
        getters.searchListModel.status +
        "&ClientName=" +
        getters.searchListModel.client_name +
        "&DateFrom=" +
        getters.searchListModel.date_from +
        "&DateTo=" +
        getters.searchListModel.date_to;
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
            constants.loanApplicationExcel + formattedDateTime + constants.xls
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
        attachment.loanID +
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
    clear({ commit }) {
      commit("CLEAR");
    }
  }
};
