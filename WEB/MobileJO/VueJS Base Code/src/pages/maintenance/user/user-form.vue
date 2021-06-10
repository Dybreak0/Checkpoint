<template>
  <v-container class="child-body">
    <v-form v-model="valid" ref="form">
      <confirm ref="confirm"></confirm>
      <info ref="info"></info>
      <loading v-if="fullscreenLoading"></loading>
      <offline @detected-condition="handleConnectivityChange"></offline>
      <v-layout row>
        <v-card flat>
          <h2><v-icon>person</v-icon> New User</h2>
          <v-spacer></v-spacer>
        </v-card>
      </v-layout>
      <v-layout row>
        <v-divider></v-divider>
      </v-layout>
      <v-spacer class="formsSpacer"></v-spacer>
      <v-container>
        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>First Name <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-text-field
                  class="req"
                  v-model="user.first_name"
                  :rules="nameRules"
                  single-line
                  solo
                  color="black"
                  maxlength="100"
                  required
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Allowed To Login</span>
              </v-flex>
              <v-flex md6>
                <v-checkbox
                  color="red"
                  v-model="user.allowed_to_login"
                ></v-checkbox>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Last Name <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-text-field
                  class="req"
                  v-model="user.last_name"
                  :rules="nameRules"
                  single-line
                  color="black"
                  solo
                  maxlength="100"
                  required
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right address">
                <span>Address</span>
              </v-flex>
              <v-flex md6>
                <v-textarea
                  rows="2"
                  :rules="[validateField]"
                  v-model="user.address"
                  color="black"
                  maxlength="250"
                  solo
                >
                </v-textarea>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Memo</span>
              </v-flex>
              <v-flex>
                <v-text-field
                  v-model="user.memo"
                  :rules="[validateField]"
                  single-line
                  color="black"
                  maxlength="50"
                  solo
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Telephone Number</span>
              </v-flex>
              <v-flex md6>
                <v-text-field
                  class="nreq"
                  v-model="user.telephone_number"
                  single-line
                  color="black"
                  :rules="[validateNumbers]"
                  maxlength="20"
                  solo
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Company <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-combobox
                  class="req"
                  v-on:change="onChangeCompany"
                  v-model="selectCompany"
                  :items="companyItems"
                  :rules="[validateRole]"
                  single-line
                  color="black"
                  solo
                  required
                ></v-combobox>
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Phone/Mobile Number</span>
              </v-flex>
              <v-flex md6>
                <v-text-field
                  class="nreq"
                  v-model="user.mobile_number"
                  :rules="[validateNumbers]"
                  single-line
                  color="black"
                  maxlength="20"
                  solo
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Branch <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-combobox
                  class="req"
                  v-model="selectBranch"
                  :items="branchItems"
                  :rules="[validateRole]"
                  single-line
                  color="black"
                  solo
                  required
                ></v-combobox>
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Role <b>*</b></span>
              </v-flex>
              <v-flex md6>
                <v-select
                  v-model="select"
                  :items="loginItems"
                  :rules="[validateRole]"
                  solo
                  color="black"
                ></v-select>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Email Address <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-text-field
                  rows="2"
                  v-model="user.email_address"
                  :rules="[validateEmail]"
                  color="black"
                  maxlength="64"
                  solo
                >
                </v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>User Type <b>*</b></span>
              </v-flex>
              <v-flex md6>
                <v-select
                  v-model="selectUserType"
                  :items="userTypeItems"
                  :rules="[validateRole]"
                  solo
                  color="black"
                ></v-select>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>

        <div class="divider">
          <v-divider></v-divider>
        </div>

        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Username <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-text-field
                  class="req"
                  v-model="user.user_name"
                  :rules="usernameRules"
                  single-line
                  solo
                  color="black"
                  maxlength="20"
                  required
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>

        <v-layout row>
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Password <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-text-field
                  class="pw req"
                  :rules="passwordRules"
                  v-model="user.password"
                  v-on:focus="onFocus"
                  v-on:change="onFocus"
                  v-on:keyup="onFocus"
                  :type="'password'"
                  single-line
                  solo
                  color="black"
                  maxlength="20"
                  required
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
        <v-layout row>
          <transition name="fade">
            <v-flex v-if="showCriterias" offset-xs2 class="criterias">
              <v-layout row>
                <v-flex md12 class="criteria-icon">
                  <v-icon v-if="criteria1" small color="green">check</v-icon>
                  <v-icon v-else small color="#ff5252">clear</v-icon>
                  <span> Must be 4-20 characters</span>
                </v-flex>
              </v-layout>
              <v-layout row>
                <v-flex md12 class="criteria-icon">
                  <v-icon v-if="criteria2" small color="green">check</v-icon>
                  <v-icon v-else small color="#ff5252">clear</v-icon>
                  <span> Contains at least one capital letter</span>
                </v-flex>
              </v-layout>
              <v-layout row class="passwordCriteria">
                <v-flex md12 class="criteria-icon">
                  <v-icon v-if="criteria3" small color="green">check</v-icon>
                  <v-icon v-else small color="#ff5252">clear</v-icon>
                  <span> Contains at least one numeric character</span>
                </v-flex>
              </v-layout>
            </v-flex>
          </transition>
        </v-layout>

        <v-layout row class="no-dt">
          <v-flex md5>
            <v-layout>
              <v-flex md4 class="input-label text-xs-right">
                <span>Confirm Password <b>*</b></span>
              </v-flex>
              <v-flex>
                <v-text-field
                  class="req"
                  v-model="user.c_password"
                  :type="'password'"
                  :rules="[confirmPassword]"
                  v-on:change="checkConfirmPassword"
                  v-on:focus="checkConfirmPassword"
                  v-on:keyup="checkConfirmPassword"
                  single-line
                  solo
                  color="black"
                  maxlength="20"
                  required
                ></v-text-field>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>

        <v-layout row>
          <v-flex class="button-field" xs12 offset-xs9>
            <v-layout>
              <v-flex offset-xs2>
                <v-btn @click="back" class="btn_secondary">
                  <v-icon>keyboard_return</v-icon>
                  Cancel
                </v-btn>
              </v-flex>
              <v-flex md10>
                <v-btn @click="save" class="btn_primary">
                  <v-icon>save</v-icon>
                  Save
                </v-btn>
              </v-flex>
            </v-layout>
          </v-flex>
        </v-layout>
      </v-container>
    </v-form>
  </v-container>
</template>

<script>
import { mapGetters } from "vuex";
import offline from "v-offline";
import confirm from "../../../common/layout/confirm-modal";
import info from "../../../common/layout/info-modal";
import loading from "../../../common/layout/progress";
import validators from "@/common/utils/form/validators";
import constants from "../../../common/utils/constants";

export default {
  data: () => ({
    valid: false,
    user: {
      first_name: "",
      last_name: "",
      memo: "",
      telephone_number: "",
      mobile_number: "",
      email_address: "",
      address: "",
      user_name: "",
      password: "",
      c_password: "",
      company_name: "",
      branch: "",
      user_type: "",
      role: "",
      allowed_to_login: true,
      maxName: constants.maxName,
      maxPassword: constants.maxPassword
    },
    fullscreenLoading: false,
    select: constants.defaultSelect,
    selectUserType: constants.defaultSelect,
    selectCompany: constants.defaultSelect,
    selectBranch: constants.defaultSelect,
    loginItems: [constants.defaultSelect],
    userTypeItems: [constants.defaultSelect],
    companyItems: [constants.defaultSelect],
    branchItems: [constants.defaultSelect],
    passwordRules: [
      v => v.length >= constants.minPassword || "",
      v => validators.alphabetFormat.test(v) || "",
      v => validators.numericFormat.test(v) || ""
    ],
    usernameRules: [
      v => !!v || constants.fillRequireFieldsError,
      v => !validators.usernameFormat.test(v) || constants.invalidUsername,
      v => validators.textFormat.test(v) || constants.invalidInput
    ],
    nameRules: [
      v => !!v || constants.fillRequireFieldsError,
      v => validators.textFormat.test(v) || constants.invalidInput
    ],
    criteria1: false,
    criteria2: false,
    criteria3: false,
    showCriterias: true,
    status: true,
    confirmPassword: true
  }),
  components: {
    confirm,
    info,
    loading,
    offline
  },
  created() {
    this.fullscreenLoading = true;
    this.$store.dispatch(constants.getUserRoles).then(() => {
      setTimeout(() => {
        if (this.$store.getters[constants.userRoles] === constants.noInternet) {
          this.$refs.info
            .open(constants.warning, constants.noInternet, {
              color: constants.error_color
            })
            .then(() => {
              this.$router.push({ path: constants.userList });
            });
        } else {
          var roles = this.$store.getters[constants.userRoles];
          for (var index = 0; index < roles.length; ++index) {
            this.loginItems.push(roles[index]);
          }
        }
        this.fullscreenLoading = false;
      }, 1000);
    });

    this.$store.dispatch(constants.getUserTypes).then(() => {
      setTimeout(() => {
        if (this.$store.getters[constants.userTypes] === constants.noInternet) {
          this.$refs.info
            .open(constants.warning, constants.noInternet, {
              color: constants.error_color
            })
            .then(() => {
              this.$router.push({ path: constants.userList });
            });
        } else {
          var userTypes = this.$store.getters[constants.userTypes];
          for (var index = 0; index < userTypes.length; ++index) {
            this.userTypeItems.push(userTypes[index]);
          }
        }
        this.fullscreenLoading = false;
      }, 1000);
    });

    this.$store.dispatch(constants.getCompanies, 0).then(() => {
      setTimeout(() => {
        if (this.$store.getters[constants.companies] === constants.noInternet) {
          this.$refs.info
            .open(constants.warning, constants.noInternet, {
              color: constants.error_color
            })
            .then(() => {
              this.$router.push({ path: constants.userList });
            });
        } else {
          var companies = this.$store.getters[constants.companies];
          for (var index = 0; index < companies.length; ++index) {
            this.companyItems.push(companies[index]);
          }
        }
        this.fullscreenLoading = false;
      }, 1000);
    });

    this.$store.dispatch(constants.getBranches).then(() => {
      setTimeout(() => {
        if (this.$store.getters[constants.branches] === constants.noInternet) {
          this.$refs.info
            .open(constants.warning, constants.noInternet, {
              color: constants.error_color
            })
            .then(() => {
              this.$router.push({ path: constants.userList });
            });
        } else {
          var branches = this.$store.getters[constants.branches];
          for (var index = 0; index < branches.length; ++index) {
            this.branchItems.push(branches[index]);
          }
        }
        this.fullscreenLoading = false;
      }, 1000);
    });
  },
  methods: {
    save() {
      this.$refs.form.validate();
      if (this.valid) {
        if (this.status === false) {
          this.handleConnectivityChange(this.status);
        } else {
          this.$refs.confirm
            .open(constants.confirm, constants.saveConfirm, {
              color: constants.modal_color
            })
            .then(confirm => {
              if (confirm) {
                this.fullscreenLoading = true;
                this.$store.dispatch(constants.clearUser);
                this.addToModel();
                this.$store.dispatch(constants.AddUser).then(() => {
                  setTimeout(() => {
                    var message = this.$store.getters[constants.listUsers]
                      .message;
                    if (
                      this.$store.getters[constants.listUsers] ===
                      constants.noInternet
                    ) {
                      this.$refs.info
                        .open(constants.warning, constants.noInternet, {
                          color: constants.error_color
                        })
                        .then(() => {});
                    } else if (message === constants.failedSave) {
                      this.$refs.info
                        .open(constants.warning, message, {
                          color: constants.error_color
                        })
                        .then(() => {});
                    } else if (message === constants.deletedUser) {
                      this.$refs.info
                        .open(constants.warning, message, {
                          color: constants.modal_error
                        })
                        .then(() => {
                          this.clearStore();
                        });
                    } else if (message === constants.notAdmin) {
                      this.$refs.info
                        .open(constants.warning, message, {
                          color: constants.modal_error
                        })
                        .then(() => {
                          this.clearStore();
                        });
                    } else if (message.ModelStateErrors !== undefined) {
                      for (
                        let i = 0;
                        i < message.ModelStateErrors.length;
                        i++
                      ) {
                        this.$refs.info
                          .open(
                            constants.warning,
                            message.ModelStateErrors[i],
                            { color: constants.error_color }
                          )
                          .then(() => {});
                      }
                    } else {
                      this.$refs.info
                        .open(constants.message, message, {
                          color: constants.modal_color
                        })
                        .then(() => {
                          this.$router.push({ path: constants.userList });
                        });
                    }
                    this.fullscreenLoading = false;
                  }, 1000);
                });
              }
            });
        }
      }
    },
    onFocus() {
      this.criteria1 =
        this.user.password.length >= constants.minPassword ? true : false;
      this.criteria2 = validators.alphabetFormat.test(this.user.password)
        ? true
        : false;
      this.criteria3 = validators.numericFormat.test(this.user.password)
        ? true
        : false;
      this.confirmPassword =
        this.user.c_password !== this.user.password
          ? constants.passordNotMatchingError
          : true;
    },

    onChangeCompany() {
      this.branchItems = [];
      this.selectBranch = constants.defaultSelect;
      this.$store
        .dispatch(constants.getBranches, this.selectCompany.value)
        .then(() => {
          setTimeout(() => {
            if (
              this.$store.getters[constants.branches] === constants.noInternet
            ) {
              this.$refs.info
                .open(constants.warning, constants.noInternet, {
                  color: constants.error_color
                })
                .then(() => {
                  this.$router.push({ path: constants.userList });
                });
            } else {
              var branches = this.$store.getters[constants.branches];
              for (var index = 0; index < branches.length; ++index) {
                this.branchItems.push(branches[index]);
              }
            }
            this.fullscreenLoading = false;
          }, 1000);
        });

      if (this.branchItems.length > 0) {
        this.selectBranch = this.branchItems[0];
      }
    },

    checkConfirmPassword() {
      if (this.user.c_password.length === 0) {
        this.confirmPassword = constants.fillRequireFieldsError;
      } else if (this.user.c_password !== this.user.password) {
        this.confirmPassword = constants.passordNotMatchingError;
      } else {
        this.confirmPassword = true;
      }
    },
    validateField(v) {
      if (!v) return true;
      else if (!validators.textFormat.test(v)) {
        return constants.invalidInput;
      } else {
        return true;
      }
    },
    validateNumbers(v) {
      if (!v) return true;
      else if (!validators.phoneNumberFormat.test(v)) {
        return constants.invalidPhoneNoError;
      } else {
        return true;
      }
    },
    validateEmail(v) {
      if (!v) {
        return constants.fillRequireFieldsError;
      } else if (!validators.emailtextFormat.test(v)) {
        return constants.invalidInput;
      } else if (!validators.emailFormat.test(v)) {
        return constants.invalidEmailError;
      } else {
        this.hasErrors = false;
        return true;
      }
    },
    validateRole(v) {
      if (v == constants.defaultSelect) {
        return constants.fillRequireFieldsError;
      } else {
        return true;
      }
    },

    back() {
      if (this.status === false) {
        this.handleConnectivityChange(this.status);
      } else {
        if (JSON.stringify(this.user) !== validators.emptyUser) {
          this.$refs.confirm
            .open(constants.confirm, constants.cancelConfirm, {
              color: constants.modal_color
            })
            .then(confirm => {
              if (confirm) {
                this.$router.push({ path: constants.userList });
              }
            });
        } else {
          this.$router.push({ path: constants.userList });
        }
      }
    },

    addToModel() {
      this.$store.getters[constants.userModel][0] = {
        user_name: this.user.user_name,
        password: this.user.password,
        first_name: this.user.first_name,
        last_name: this.user.last_name,
        role_id: this.select,
        email_address: this.user.email_address,
        allowed_to_login: this.user.allowed_to_login,
        memo: this.user.memo,
        telephone_no: this.user.telephone_number,
        mobile_no: this.user.mobile_number,
        address: this.user.address,
        company_id: this.selectCompany.value,
        user_type_id: this.selectUserType,
        branch_id: this.selectBranch.value
      };
    },

    clearStore() {
      this.$store.dispatch(constants.clearLogin);
      this.$store.dispatch(constants.clearUsers);
      this.$store.dispatch(constants.clearAccounts);
      this.$store.dispatch(constants.clearEmails);
      this.$store.dispatch(constants.clearJobOrders);
      this.$store.dispatch(constants.clearCases);
      this.$store.dispatch(constants.clearRating);
      this.$router.push("/login");
    },

    handleConnectivityChange(status) {
      if (status === false) {
        this.status = false;
        this.$refs.info
          .open(constants.message, constants.noInternet, {
            color: constants.error_color
          })
          .then(() => {});
      } else {
        this.status = true;
      }
    }
  },
  computed: {
    ...mapGetters({
      model: constants.userModel,
      roles: constants.userRoles,
      userTypes: constants.userTypes,
      companies: constants.companies,
      branches: constants.branches
    })
  }
};
</script>

<style></style>
