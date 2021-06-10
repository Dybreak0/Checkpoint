<template>
    <div id="login">
        <info ref="info"></info>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <loading v-if="fullscreenLoading"></loading>
        <confirm ref="confirm"></confirm>
        <v-container class="loginContainer">
            <v-layout row justify-center>
                <v-flex flat md4 >
                    <v-layout row>
                        <v-spacer class="formsSpacer"></v-spacer>
                    </v-layout>
                    <v-layout row class="loginDialog">
                        <v-flex>
                            <v-container justify-center class="change-password">
                                <v-layout row>
                                    <v-flex>
                                        <v-img :src="require('../../assets/images/checkpoint-logo.jpg')" aspect-ratio="1.5" contain></v-img>
                                    </v-flex>
                                </v-layout>
                                <v-flex class="forgot-flex">
                                    <span class="forgot-label">Forgot Password</span> 
                                </v-flex>
                                <v-layout row v-if="isValid">
                                        <v-form v-model="valid" ref="form" class="changePassword-form">
                                            <v-layout row>
                                                <v-flex>
                                                    <v-text-field :rules="passwordRules"
                                                                        prepend-inner-icon="lock"
                                                                        v-model="password"
                                                                        placeholder="New Password"
                                                                        v-on:focus="onFocus"
                                                                        v-on:change="onFocus"
                                                                        v-on:keyup="onFocus"
                                                                        maxlength="20"
                                                                        class="loginInput"
                                                                        type="password"
                                                                        solo
                                                                        required>
                                                        </v-text-field>
                                                </v-flex>
                                            </v-layout>
                                            <v-layout row>
                                                    <transition name="fade">
                                                        <v-flex v-if="showCriterias"
                                                                class="criterias criterias-password">
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
                                            <v-flex>
                                                <v-text-field :rules="[confirmPassword]"
                                                                prepend-inner-icon="lock"
                                                                v-model="retype_password"
                                                                v-on:change="checkConfirmPassword"
                                                                v-on:focus="checkConfirmPassword"
                                                                v-on:keyup="checkConfirmPassword"
                                                                placeholder="Re-type New Password"
                                                                maxlength="100"
                                                                class="loginInput"
                                                                type="password"
                                                                solo
                                                                required>
                                                </v-text-field>
                                            </v-flex>
                                            <v-spacer></v-spacer>
                                            <v-flex class="text-xs-right">
                                                <v-btn @click="changePassword" block class="loginBtn">Reset Password</v-btn>
                                            </v-flex>
                                        </v-form>
                                </v-layout>
                            </v-container>
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>
    </v-container>
    </div>
</template>

<script>
    import loading from '../../common/layout/progress';
    import info from '../../common/layout/info-modal';
    import offline from 'v-offline';
    import { mapGetters } from 'vuex';
    import constants from '../../common/utils/constants';
    import validators from '@/common/utils/form/validators';
    import confirm from '../../common/layout/confirm-modal';

    export default {
        name: 'app-login',
        data() {
            return {
                valid: false,
                retype_password: "",
                password: "",
                status: true,
                error: constants.fillRequireFieldsError,
                passwordRules: [
                v => ((v).length >= constants.minPassword) || '',
                v => ((validators.alphabetFormat).test(v)) || '',
                v => ((validators.numericFormat).test(v)) || '',                
                ],
                fullscreenLoading: false,
                isValid: false,
                criteria1: false,
                criteria2: false,
                criteria3: false,
                showCriterias: true,
                confirmPassword: true,
                requestModel: {}
            };
        },

        components: {
            loading,
            info,
            offline,
            constants,
            confirm
        },
        created() {
            this.checkValidity();
        },
        methods: {
            onFocus() {
                this.criteria1 = ((this.password).length >= constants.minPassword) ? true : false;
                this.criteria2 = ((validators.alphabetFormat).test(this.password)) ? true : false;
                this.criteria3 = ((validators.numericFormat).test(this.password)) ? true : false;
                this.confirmPassword = (this.retype_password !== this.password) ? constants.passordNotMatchingError : true;

            },
            checkConfirmPassword() {
                if (this.retype_password.length === 0) {
                    this.confirmPassword = constants.fillRequireFieldsError;
                }
                else if (this.retype_password !== this.password) {
                    this.confirmPassword = constants.passordNotMatchingError;
                }
                else {
                    this.confirmPassword = true;
                }
            },
            checkValidity(){
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.fullscreenLoading = true;
                    var params = this.$route.params;
                    this.$store.dispatch(constants.checkValidity, params).then(() => {
                        setTimeout(() => {
                            var data = this.$store.getters[constants.sendList];
                            if(!data.message){
                                this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                });
                            }
                            else{
                                if(null != data.data && constants.success == data.message){
                                    this.isValid = true;
                                    this.requestModel = data.data;
                                }
                                else if(constants.recordNotExist == data.message){
                                    this.$refs.info.open(constants.warning, data.message, { color: constants.error_color }).then(() => {
                                        this.$router.push('/');
                                    });
                                }
                            }
                        }, 1000);
                    })
                    this.fullscreenLoading = false;
                }
            },
            changePassword() {
                this.$refs.form.validate();
                if (this.valid) {
                    if (this.status === false) {
                        this.handleConnectivityChange(this.status)
                    }
                    else {
                        this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.fullscreenLoading = true;
                                this.addToModel();
                                this.$store.dispatch(constants.resetPassword).then(() => {
                                    setTimeout(() => {
                                        var message = this.$store.getters[constants.sendList].message;
                                        if(!message){
                                            this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                            });
                                        }
                                        else{
                                            if(message == constants.saved){
                                                 this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.$router.push('/');
                                                });
                                            }
                                            else if(message == constants.recordNotExist){
                                                this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                                });
                                            }
                                            else{
                                                this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.modal_error }).then(() => {
                                                });
                                            }
                                        }
                                        
                                        this.fullscreenLoading = false;
                                    }, 1000); 

                                });
                            }
                        })
                    }
                }

            },
            addToModel(){
                var data = this.requestModel;
                this.$store.getters[constants.sendRequestModel][0] = {
                    "id": data.ID,
                    "userId": data.UserID,
                    "token": data.ResetToken,
                    "newPassword": this.password
                }
            },
            handleConnectivityChange(status) {
                if (status === false) {
                    this.status = false;
                    this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                        
                    });
                }
                else {
                    this.status = true;
                }
            }

        }
    };
</script>

<style>
</style>
