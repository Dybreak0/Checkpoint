<template>
  <div id="login" class="my-auto">
    <info ref="info"></info>
    <offline @detected-condition="handleConnectivityChange"></offline>
    <loading v-if="fullscreenLoading"></loading>
    <v-container class="loginContainer">
      <v-layout row justify-center>
        <v-flex flat xs12 sm8 md6 lg4>
          <v-layout row>
            <v-flex>
              <v-container justify-center>
                <v-layout column>
                  <v-flex px-5 mb-3>
                    <v-img
                      :src="require('../../assets/images/logo-addessa-lg.png')"
                      aspect-ratio="1"
                      style="width: 60%"
                      class="mx-auto"
                    ></v-img>
                  </v-flex>
                  <v-flex xs5 mb-5>
                    <h2 class="text-xs-center display-1">Addessa Checkpoint</h2>
                  </v-flex>
                </v-layout>
                <v-form
                  v-model="valid"
                  ref="form"
                  @submit.prevent="login({ username, password })"
                >
                  <v-flex>
                    <v-chip
                      v-if="withError"
                      class="loginError"
                      color="red"
                      text-color="white"
                      >{{ error }}</v-chip
                    >
                  </v-flex>
                  <v-flex>
                    <v-text-field
                      :rules="fieldRules"
                      prepend-inner-icon="person"
                      name="Username"
                      v-model="username"
                      placeholder="Username"
                      typeof="text"
                      class="loginInput"
                      color="red"
                      solo
                      hide-details
                      required
                    ></v-text-field>
                  </v-flex>
                  <v-flex>
                    <v-text-field
                      :rules="fieldRules"
                      prepend-inner-icon="lock"
                      name="Password"
                      v-model="password"
                      placeholder="Password"
                      type="password"
                      class="loginInput"
                      color="red"
                      solo
                      hide-details
                      required
                    ></v-text-field>
                  </v-flex>
                  <v-spacer></v-spacer>
                  <v-flex class="text-xs-right">
                    <v-btn type="submit" block class="loginBtn">{{
                      $t("login")
                    }}</v-btn>
                  </v-flex>
                  <v-flex class="forgot-flex">
                    <span @click="forgotPassword" class="forgot-text"
                      >Forgot Password?</span
                    >
                  </v-flex>
                </v-form>
              </v-container>
            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>
    </v-container>
  </div>
</template>

<script>
import loading from "../../common/layout/progress";
import info from "../../common/layout/info-modal";
import offline from "v-offline";
import { mapGetters } from "vuex";
import constants from "../../common/utils/constants";

export default {
  name: "app-login",
  data() {
    return {
      valid: false,
      withError: false,
      username: "",
      password: "",
      status: true,
      error: constants.fillRequireFieldsError,
      fieldRules: [(v) => !!v || ""],
      fullscreenLoading: false,
    };
  },

  components: {
    loading,
    info,
    offline,
    constants,
  },
  created() {
    this.redirect();
  },

  methods: {
    login(data) {
      this.$refs.form.validate();
      if (this.valid) {
        if (this.status === false) {
          this.handleConnectivityChange(this.status);
        } else {
          this.fullscreenLoading = true;
          this.$store.dispatch(constants.login, data).then(() => {
            if (this.$store.getters[constants.isLoggedIn]) {
              this.fullscreenLoading = false;
              this.$router.push(constants.home);
            } else {
              var error = this.$store.getters[constants.loginError];
              this.$refs.info
                .open(constants.warning, error, {
                  color: constants.error_color,
                })
                .then(() => {});
              this.$store.dispatch(constants.loginClear).then(() => {
                this.fullscreenLoading = false;
              });
            }
          });
        }
        this.withError = false;
      } else {
        this.withError = true;
      }
    },
    handleConnectivityChange(status) {
      if (status === false) {
        this.status = false;
        this.$refs.info
          .open(constants.warning, constants.noInternet, {
            color: constants.error_color,
          })
          .then(() => {
            this.$router.push("/");
          });
      } else {
        this.status = true;
      }
    },
    redirect() {
      if (this.isLoggedIn) {
        this.$router.push(constants.home);
      } else {
        this.$router.push(constants.loginRoute);
      }
    },
    forgotPassword() {
      this.$router.push(constants.forgotPassword);
    },
  },
  computed: {
    ...mapGetters({
      isLoggedIn: "login/isLoggedIn",
    }),
  },
};
</script>

<style>
</style>
