import serviceUtil from "../../common/utils/main-service";

const localStorageStore = JSON.parse(localStorage.getItem("store"));
const localStorageLogin =
  localStorageStore === null ? null : localStorageStore.login;

export default {
  namespaced: true,

  state: {
    isLoggedIn:
      localStorageLogin != null ? localStorageLogin.isLoggedIn : false,
    token:
      localStorage.getItem("token") != null
        ? localStorage.getItem("token")
        : "",
    userName: "",
    fullName: "",
    id: 0,
    error: [],
    role: 0,
    branchID: 0,
    companyID: 0,
  },

  getters: {
    isLoggedIn: state => state.isLoggedIn,
    token: state => state.token,
    userName: state => state.userName,
    fullName: state => state.fullName,
    id: state => state.id,
    error: state => state.error,
    role: state => state.role,
    branchID: state => state.branchID,
    companyID: state => state.companyID,
  },

  mutations: {
    LOGIN(state) {
      state.pending = true;
    },

    LOGIN_SUCCESS(state, payload) {
      if (payload !== undefined) {
        if (payload.access_token == undefined) {
          state.error = payload;
        } else {
          console.log(payload);
          state.isLoggedIn = true;
          state.pending = false;
          state.token = payload.access_token;
          state.refreshToken = payload.refresh_token;
          localStorage.setItem("token", payload.access_token);

          const base64Url = payload.access_token.split(".")[1];
          const base64 = base64Url.replace("-", "+").replace("_", "/");
          const dataJWT = JSON.parse(window.atob(base64));
          state.userName = dataJWT["user_name"];
          state.fullName = dataJWT["full_name"];
          state.id = dataJWT["id"];
          state.role = parseInt(dataJWT["role_id"]);
          state.branchID = parseInt(dataJWT["branch_id"]);
          state.companyID = dataJWT["company_id"];
        }
      }
    },

    LOGOUT(state) {
      state.token = "";
      state.refreshToken = "";
      state.error = [];
      state.id = 0;
      state.isLoggedIn = false;
      state.userName = "";
      state.fullName = "";
      localStorage.removeItem("token");
      localStorage.removeItem("store");
    },

    CLEAR(state) {
      state.error = [];
    }
  },

  actions: {
    login({ commit }, data) {
      commit("LOGIN");
      return serviceUtil.login("api/token", data).then(loginData => {
        commit("LOGIN_SUCCESS", loginData);
      });
    },

    logout({ commit }) {
      commit("LOGOUT");
    },

    clear({ commit }) {
      commit("CLEAR");
    }
  }
};
