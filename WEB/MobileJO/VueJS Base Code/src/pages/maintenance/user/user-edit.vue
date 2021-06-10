<template>
  <v-form v-model="valid" ref="form">
    <confirm ref="confirm"></confirm>
    <info ref="info"></info>
    <loading v-if="fullscreenLoading"></loading>
    <offline @detected-condition="handleConnectivityChange"></offline>
    <v-container class="child-body" v-if="active">
      <v-layout row>
        <v-card flat>
          <h2><v-icon>person</v-icon> Edit User Details</h2>
          <v-spacer></v-spacer>
        </v-card>
      </v-layout>
      <v-layout row>
        <v-divider></v-divider>
      </v-layout>
      <v-spacer class="formsSpacer"></v-spacer>
      <v-layout row>
        <v-flex md5>
          <v-layout>
            <v-flex md4 class="input-label text-xs-right">
              <span>First Name <b>*</b></span>
            </v-flex>
            <v-flex>
              <v-text-field
                class="req"
                v-model="model.data.first_name"
                :rules="nameRules"
                single-line
                solo
                maxlength="100"
                color="black"
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
                v-model="model.data.allowed_to_login"
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
                v-model="model.data.last_name"
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
            <v-flex md4 class="input-label text-xs-right address">
              <span>Address</span>
            </v-flex>
            <v-flex md6>
              <v-textarea
                rows="2"
                :rules="[validateField]"
                v-model="model.data.address"
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
                v-model="model.data.memo"
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
                v-model="model.data.telephone_no"
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
                v-model="selectCompany"
                v-on:change="onChangeCompany"
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
                v-model="model.data.mobile_no"
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
      <v-layout row>
        <v-flex md5>
          <v-layout>
            <v-flex md4 class="input-label text-xs-right">
              <span>Email Address <b>*</b></span>
            </v-flex>
            <v-flex>
              <v-text-field
                rows="2"
                v-model="model.data.email_address"
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

      <div class="divider">
        <v-divider></v-divider>
      </div>

      <v-layout row>
        <v-flex md5>
          <v-layout>
            <v-flex md4 class="input-label text-xs-right">
              <span>Username</span>
            </v-flex>
            <v-flex>
              <v-text-field
                class="req"
                v-model="model.data.user_name"
                :rules="nameRules"
                single-line
                solo
                color="black"
                maxlength="20"
                disabled
              ></v-text-field>
            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>

      <v-layout row>
        <v-flex md5>
          <v-layout>
            <v-flex md4 class="input-label text-xs-right">
              <span>New Password</span>
            </v-flex>
            <v-flex>
              <v-text-field
                :rules="[validateNewPassword]"
                class="pw req"
                v-model="password"
                v-on:focus="onFocus"
                v-on:change="onFocus"
                v-on:keyup="onFocus"
                :type="'password'"
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

      <v-layout row v-if="passwordEdit" class="no-dt">
        <v-flex md5>
          <v-layout>
            <v-flex md4 class="input-label text-xs-right">
              <span>Confirm New Password <b>*</b></span>
            </v-flex>
            <v-flex>
              <v-text-field
                class="req"
                v-model="c_password"
                :type="'password'"
                :rules="[confirmPassword]"
                single-line
                solo
                color="black"
                maxlength="20"
                Required
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
    password: "",
    c_password: "",
    old_password: "",
    fullscreenLoading: false,
    status: true,
    select: "",
    selectUserType: "",
    selectCompany: {},
    selectBranch: {},
    loginItems: [],
    userTypeItems: [],
    companyItems: [],
    branchItems: [],
    criteria1: false,
    criteria2: false,
    criteria3: false,
    showCriterias: false,
    oldUser: "",
    passwordEdit: false,
    active: false,
    nameRules: [
      v => !!v || constants.fillRequireFieldsError,
      v => validators.textFormat.test(v) || constants.invalidInput
    ],
    usernameRules: [
      v => !!v || constants.fillRequireFieldsError,
      v => !validators.usernameFormat.test(v) || constants.invalidUsername,
      v => validators.textFormat.test(v) || constants.invalidInput
    ]
  }),
  components: {
    confirm,
    info,
    loading,
    offline
  },
  props: {
    id: String
  },
  created() {
    this.fullscreenLoading = true;
    this.$store.dispatch(constants.user, this.$props.id).then(() => {
      setTimeout(() => {
        var message = this.$store.getters[constants.userModel].message;
        if (this.$store.getters[constants.userModel] === constants.noInternet) {
          this.$refs.info
            .open(constants.warning, constants.noInternet, {
              color: constants.error_color
            })
            .then(() => {
              this.$router.push({ path: constants.userList });
            });
        } else if (message === constants.noResults) {
          this.$refs.info
            .open(constants.warning, constants.recordNotExist, {
              color: constants.error_color
            })
            .then(() => {
              this.$router.push({ path: constants.userList });
            });
        } else {
          if (this.$store.getters[constants.userModel].data.is_active) {
            this.active = true;
            this.$store.dispatch(constants.getUserRoles).then(() => {
              var roles = this.$store.getters[constants.userRoles];
              var roleID = this.$store.getters[constants.userModel].data
                .role_id;
              for (var index = 0; index < roles.length; ++index) {
                if (roles[index].value === roleID) {
                  this.select = roleID;
                }
                this.loginItems.push(roles[index]);
              }
              this.getUserItems();
            });

            this.$store.dispatch(constants.getUserTypes).then(() => {
              var userTypes = this.$store.getters[constants.userTypes];
              var userTypeID = this.$store.getters[constants.userModel].data
                .user_type_id;
              for (var index = 0; index < userTypes.length; ++index) {
                if (userTypes[index].value === userTypeID) {
                  this.selectUserType = userTypeID;
                }

                this.userTypeItems.push(userTypes[index]);
              }
              this.getUserItems();
            });

            this.$store.dispatch(constants.getCompanies).then(() => {
              var companies = this.$store.getters[constants.companies];
              var companyID = this.$store.getters[constants.userModel].data
                .company_id;
              for (var index = 0; index < companies.length; ++index) {
                if (companies[index].value === companyID) {
                  this.selectCompany = companies[index];
                }
                this.companyItems.push(companies[index]);
              }

              this.getUserItems();
            });

            var companyID = this.$store.getters[constants.userModel].data
              .company_id;
            this.$store.dispatch(constants.getBranches, companyID).then(() => {
              var branches = this.$store.getters[constants.branches];
              var branchID = this.$store.getters[constants.userModel].data
                .branch_id;
              for (var index = 0; index < branches.length; ++index) {
                if (branches[index].value === branchID) {
                  this.selectBranch = branches[index];
                }
                this.branchItems.push(branches[index]);
              }

              this.getUserItems();
            });
          } else {
            this.$refs.info
              .open(constants.warning, constants.notAvailable, {
                color: constants.modal_error
              })
              .then(() => {
                this.$router.push({ path: constants.userList });
              });
          }
        }

        this.fullscreenLoading = false;
      }, 1000);
    });
  },
  methods: {
    save() {
      this.$store.getters[constants.userModel].data.role_id = this.select;
      this.$store.getters[
        constants.userModel
      ].data.user_type_id = this.selectUserType;
      this.$store.getters[
        constants.userModel
      ].data.company_id = this.selectCompany.value;
      this.$store.getters[
        constants.userModel
      ].data.branch_id = this.selectBranch.value;
      if (
        JSON.stringify(this.$store.getters[constants.userModel].data) !==
          this.oldUser ||
        this.password !== ""
      ) {
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
                  setTimeout(() => {
                    this.addToModel();
                    this.$store.dispatch(constants.editUser).then(() => {
                      var message = this.$store.getters[constants.listUsers]
                        .message;
                      if (
                        this.$store.getters[constants.listUsers] ===
                        constants.noInternet
                      ) {
                        this.$refs.info
                          .open(constants.message, constants.noInternet, {
                            color: constants.error_color
                          })
                          .then(() => {});
                      } else if (
                        this.$store.getters[constants.listUsers].message ===
                        constants.deletedUser
                      ) {
                        this.$refs.info
                          .open(constants.warning, message, {
                            color: constants.modal_error
                          })
                          .then(() => {
                            this.clearStore();
                          });
                      } else if (
                        this.$store.getters[constants.listUsers].message ===
                        constants.notAdmin
                      ) {
                        this.$refs.info
                          .open(constants.warning, message, {
                            color: constants.error_color
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
                          if (
                            message.ModelStateErrors[i] ===
                            constants.recordNotExist
                          ) {
                            this.$refs.info
                              .open(constants.warning, constants.notAvailable, {
                                color: constants.error_color
                              })
                              .then(() => {
                                this.$router.push({ path: constants.userList });
                              });
                            break;
                          }
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
                    });
                    this.fullscreenLoading = false;
                  }, 1000);
                }
                this.showCriterias = false;
              });
          }
        }
      } else {
        this.$refs.info
          .open(constants.message, constants.saved, {
            color: constants.modal_color
          })
          .then(() => {
            this.$router.push({ path: constants.userList });
          });
      }
    },
    onFocus() {
      if (this.password !== "") {
        this.criteria1 =
          this.password.length >= constants.minPassword ? true : false;
        this.criteria2 = validators.alphabetFormat.test(this.password)
          ? true
          : false;
        this.criteria3 = validators.numericFormat.test(this.password)
          ? true
          : false;
        this.showCriterias = true;
        this.passwordEdit = true;
      } else {
        this.showCriterias = false;
        this.passwordEdit = false;
      }
    },

    onChangeCompany() {
      this.branchItems = [];
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

            if (this.branchItems.length > 0) {
              this.selectBranch = this.branchItems[0];
            }

            this.fullscreenLoading = false;
          }, 1000);
        });
    },

    confirmPassword(v) {
      if (this.password !== "") {
        if (v != this.password) {
          return constants.passordNotMatchingError;
        }
      } else {
        return false;
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
      else if (v.length > constants.maxNumber) {
        return constants.maxCharsReached;
      } else if (!validators.phoneNumberFormat.test(v)) {
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
    validateNewPassword(v) {
      this.onFocus();
      if (this.password === "") {
        return true;
      } else if (
        (this.criteria1 && this.criteria2 && this.criteria3) === false
      ) {
        return "";
      } else {
        return true;
      }
    },
    back() {
      if (this.status === false) {
        this.handleConnectivityChange(this.status);
      } else {
        this.$store.getters[constants.userModel].data.role_id = this.select;
        this.$store.getters[
          constants.userModel
        ].data.user_type_id = this.selectUserType;
        this.$store.getters[
          constants.userModel
        ].data.company_id = this.selectCompany.value;

        if (
          JSON.stringify(this.$store.getters[constants.userModel].data) !==
            this.oldUser ||
          this.password !== ""
        ) {
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

    addToModel() {
      var dt = new Date();
      var dateNow = dt.toISOString();
      var username = this.$store.getters[constants.userName];

      this.$store.getters[constants.userModel].data.role_id = this.select;
      this.$store.getters[
        constants.userModel
      ].data.user_type_id = this.selectUserType;
      this.$store.getters[
        constants.userModel
      ].data.company_id = this.selectCompany.value;
      this.$store.getters[constants.userModel].data.password = this.password;
      this.$store.getters[
        constants.userModel
      ].data.old_password = this.Old_password;
      this.$store.getters[constants.userModel].data.updated_date = dateNow;
      this.$store.getters[constants.userModel].data.updated_by = username;
    },

    getUserItems() {
      this.oldUser = JSON.stringify(
        this.$store.getters[constants.userModel].data
      );
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
      companies: constants.companies
    })
  }
};
</script>

<style></style>
