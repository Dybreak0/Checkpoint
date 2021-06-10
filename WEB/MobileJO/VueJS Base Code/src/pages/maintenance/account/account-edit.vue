<template>
    <v-form v-model="valid" ref="form">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
            <v-container class="child-body" v-if="active">
                <v-layout row>
                    <v-card flat>
                        <h2>
                            <v-icon>person</v-icon> Edit Account Details
                        </h2>
                        <v-spacer></v-spacer>
                    </v-card>
                </v-layout>
                <v-layout row>
                    <v-divider></v-divider>
                </v-layout>
                <v-spacer class="formsSpacer"></v-spacer>
                <v-layout row>
                    <v-flex xs12
                            md4
                            offset-xs1>
                        <v-layout>
                            <v-flex xs4 md4 class="input-label text-xs-right">
                                <span>Name</span>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field class="req"
                                              v-model="model.data.name"
                                              single-line
                                              solo
                                              color="black"
                                              maxlength="100"
                                              disabled></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs12
                            md4
                            offset-xs1>
                        <v-layout>
                            <v-flex xs4 md4 class="input-label text-xs-right">
                                <span>Memo</span>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field v-model="model.data.memo"
                                              :rules="[validateMemo]"
                                              single-line
                                              color="black"
                                              maxlength="50"
                                              solo></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs12
                            md4
                            offset-xs1>
                        <v-layout>
                            <v-flex xs4 md4 class="input-label text-xs-right">
                                <span>Email Address <b>*</b></span>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field class="req"
                                              rows="2"
                                              v-model="model.data.email_address"
                                              :rules="[validateEmail]"
                                              solo
                                              color="black"
                                              maxlength="64"
                                              required>
                                </v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs12
                            md4
                            offset-xs1>
                        <v-layout>
                            <v-flex xs4 md4 class="input-label text-xs-right">
                                <span>Contact Person <b>*</b></span>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field class="req"
                                              v-model="model.data.contact_person"
                                              :rules="nameRules"
                                              single-line
                                              solo
                                              color="black"
                                              maxlength="100"
                                              required></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs12
                            md4
                            offset-xs1>
                        <v-layout>
                            <v-flex xs4 md4 class="input-label text-xs-right">
                                <span>Contact Number <b>*</b></span>
                            </v-flex>
                            <v-flex xs12>
                                <v-text-field class="req"
                                              v-model="model.data.contact_number"
                                              :rules="[validateNumbers]"
                                              single-line
                                              solo
                                              color="black"
                                              maxlength="20"
                                              required></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>
                
                <v-layout row>
                    <v-flex xs12
                            md4
                            offset-xs1>
                        <v-layout>
                            <v-flex xs4 md4 class="input-label text-xs-right address">
                                <span>
                                    Address <b>*</b>
                                </span>
                            </v-flex>
                            <v-flex xs12>
                                <v-textarea class="req"
                                            rows="2"
                                            v-model="model.data.address"
                                            :rules="[validateField]"
                                            solo
                                            color="black"
                                             maxlength="250"
                                            required>
                                </v-textarea>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex class="button-field"
                            xs12
                            offset-xs9>
                        <v-layout>
                            <v-flex offset-xs2>
                                <v-btn @click="cancel" class="btn_secondary">
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
    import { mapGetters } from 'vuex';
    import offline from 'v-offline';
    import confirm from '../../../common/layout/confirm-modal';
    import info from '../../../common/layout/info-modal';
    import loading from '../../../common/layout/progress';
    import validators from '@/common/utils/form/validators';
    import constants from '../../../common/utils/constants';
    export default {
        data: () => ({
            valid: false,
            fullscreenLoading: false,
            active: false,
            nameRules: [
                v => !!v || constants.fillRequireFieldsError,
                v =>  ((validators.textFormat).test(v)) || constants.invalidInput
            ],
            oldAccount: "",
        }),
        props: {
            id: String
        },
        components: {
            confirm,
            info,
            loading,
            offline
        },
        computed: {
            ...mapGetters({
                model: constants.accountModel,
            }),
        },
        created() {
            this.fullscreenLoading = true;
            this.$store.dispatch(constants.account, this.$props.id).then(() => {
                setTimeout(() => {
                    var message = this.$store.getters[constants.accountModel].message;
                    if(this.$store.getters[constants.accountModel] === constants.noInternet){
                        this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                            this.$router.push({ path: constants.accountList });
                        });
                    }
                    else if (message === constants.noResults) {
                        this.$refs.info.open(constants.warning, constants.recordNotExist, { color: constants.error_color }).then(() => {
                            this.$router.push({ path: constants.accountList });
                        });
                    }
                    else {
                        if (this.$store.getters[constants.accountModel].data.is_active) {
                            this.active = true;
                            this.getAccountItems();
                        } else {
                            this.$refs.info.open(constants.warning, constants.notAvailable, { color: constants.error_color }).then(() => {
                                this.$router.push({ path: constants.accountList })
                            });
                        }
                    }
                    this.fullscreenLoading = false;
                }, 1000);
            });
           
        },
        methods: {
            save() {
                if (JSON.stringify(this.$store.getters[constants.accountModel].data) !== this.oldAccount) {
                    this.$refs.form.validate()
                    if (this.valid) {
                        if (this.status === false) {
                            this.handleConnectivityChange(this.status)
                        }
                        else {
                            this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                                if (confirm) {
                                    this.fullscreenLoading = true;
                                    setTimeout(() => {
                                        this.setAdditionalItems();
                                        this.$store.dispatch(constants.editAccount).then(() => {
                                            var message = this.$store.getters[constants.listAccount].message;
                                            if(this.$store.getters[constants.listAccount] === constants.noInternet){
                                                this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                                });
                                            }
                                            else if (message === constants.deletedUser) {
                                                this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                                    this.clearStore();
                                                });
                                            }
                                            else if (message === constants.notAdmin) {
                                                this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                                    this.clearStore();
                                                });
                                            }
                                            else if(message.ModelStateErrors !== undefined){
                                                 for(let i=0; i < message.ModelStateErrors.length; i++){
                                                         if(message.ModelStateErrors[i] === constants.recordNotExist){
                                                             this.$refs.info.open(constants.warning, constants.notAvailable, { color: constants.error_color }).then(() => {
                                                                 this.$router.push({ path: constants.accountList });
                                                         });
                                                         break;
                                                     }
                                                     this.$refs.info.open(constants.warning, message.ModelStateErrors[i], { color: constants.error_color }).then(() => {});
                                                 }
                                            }
                                            else{
                                                if(message === constants.recordNotExist){
                                                    color = constants.error_color;
                                                }
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.$router.push({ path: constants.accountList });
                                                });
                                            }
                                            this.fullscreenLoading = false;
                                        });
                                    }, 1000);

                                }
                            })
                        }
                    }
                }
                else {
                    this.$refs.info.open(constants.message, constants.saved, { color: constants.modal_color }).then(() => {
                        this.$router.push({ path: constants.accountList });
                    });
                }  
            },
            validateMemo(v) {
                if (!v) return true;
                else if (!(validators.textFormat).test(v)) {
                    return constants.invalidInput;
                }
                else {
                    return true;
                }
            },
            validateField(v) {
                if (!v) {
                    return constants.fillRequireFieldsError;
                }
                else if (!(validators.textFormat).test(v)) {
                    return constants.invalidInput;
                }
                else {
                    return true;
                }
            },
            validateNumbers(v) {
                if (!v) {
                    return constants.fillRequireFieldsError;
                }
                else if (v.length > constants.maxNumber) {
                    return constants.maxCharsReached;
                }
                else if (!((validators.phoneNumberFormat).test(v))) {
                    return constants.invalidPhoneNoError;
                }
                else {
                    return true;
                }
            },
            validateEmail(v) {
                if (!v) {
                    return constants.fillRequireFieldsError;;
                }
                else if (!(validators.emailtextFormat).test(v)) {
                    return constants.invalidInput;
                }
                else if (!(validators.emailFormat).test(v)) {
                    return constants.invalidEmailError;;
                }
                else {
                    return true;
                }
            },
            cancel() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    if (JSON.stringify(this.$store.getters[constants.accountModel].data) !== this.oldAccount) {
                        this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.$router.push({ path: constants.accountList });
                            }
                        });
                    }
                    else {
                        this.$router.push({ path: constants.accountList });
                    }
                }
            },
            getAccountItems() {
                this.oldAccount = JSON.stringify(this.$store.getters[constants.accountModel].data);
            },
            setAdditionalItems() {
                var dt = new Date();
                var dateNow = dt.toISOString();
                var userID = this.$store.getters[constants.userID];
                this.$store.getters[constants.accountModel].data.updated_date = dateNow;
                this.$store.getters[constants.accountModel].data.updated_by = userID;
            },
            clearStore() {
                this.$store.dispatch(constants.clearLogin);
                this.$store.dispatch(constants.clearUsers);
                this.$store.dispatch(constants.clearAccounts);
                this.$store.dispatch(constants.clearEmails);
                this.$store.dispatch(constants.clearJobOrders);
                this.$store.dispatch(constants.clearCases);
                this.$store.dispatch(constants.clearRating);
                this.$router.push('/login');
            },
            handleConnectivityChange(status) {
                if (status === false) {
                    this.status = false;
                    this.$refs.info.open(constants.message, constants.noInternet, { color: constants.error_color }).then(() => { });
                }
                else {
                    this.status = true;
                }
            }

        }

    }
</script>

<style>
</style>


