<template>
    <div id="login">
        <info ref="info"></info>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <loading v-if="fullscreenLoading"></loading>
        <v-container class="loginContainer">
            <v-layout row justify-center>
                <v-flex flat md4 >
                    <v-layout row>
                        <v-spacer class="formsSpacer"></v-spacer>
                    </v-layout>
                    <v-layout row class="loginDialog">
                        <v-flex>
                            <v-container justify-center>
                                <v-layout row>
                                    <v-flex>
                                        <v-img :src="require('../../assets/images/checkpoint-logo.jpg')" aspect-ratio="1.5" contain></v-img>
                                    </v-flex>
                                </v-layout>
                                <v-form v-model="valid" ref="form">
                                    <v-flex>
                                        <v-text-field :rules="[validateEmail]"
                                                        prepend-inner-icon="person"
                                                        v-model="username"
                                                        placeholder="Email"
                                                        maxlength="100"
                                                        class="loginInput"
                                                        solo
                                                        required>
                                        </v-text-field>
                                    </v-flex>
                                    <v-spacer></v-spacer>
                                    <v-flex class="text-xs-right">
                                        <v-btn @click="save" block class="loginBtn">Submit Request</v-btn>
                                    </v-flex>
                                    <v-flex class="forgot-flex">
                                        <span @click="back" class="forgot-text">Back</span>
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
    import loading from '../../common/layout/progress';
    import info from '../../common/layout/info-modal';
    import offline from 'v-offline';
    import { mapGetters } from 'vuex';
    import constants from '../../common/utils/constants';
    import validators from '@/common/utils/form/validators';

    export default {
        name: 'app-login',
        data() {
            return {
                valid: false,
                withError: false,
                username: "",
                password: "",
                status: true,
                error: constants.fillRequireFieldsError,
                fieldRules: [
                    v => !!v || '',
                ],
                fullscreenLoading: false,
            };
        },

        components: {
            loading,
            info,
            offline,
            constants
        },
        created() {
            this.redirect();
        },

        methods: {
            validateEmail(v) {
                if (!v) {
                    return constants.fillRequireFieldsError;
                }
                else if (!(validators.emailtextFormat).test(v)) {
                    return constants.invalidInput;
                }
                else if (!(validators.emailFormat).test(v)) {
                    return constants.invalidEmailError;
                }
                else {
                    this.hasErrors = false;
                    return true;
                }
            },
            save() {
                this.$refs.form.validate();
                if (this.valid) {
                    if (this.status === false) {
                        this.handleConnectivityChange(this.status)
                    }
                    else {
                        this.fullscreenLoading = true;
                        this.$store.dispatch(constants.sendRequest, this.username.trim()).then(() => {
                        setTimeout(() => {
                            this.fullscreenLoading = false;
                            if(!this.$store.getters[constants.sendList].message){
                                this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                });
                            }
                            else{
                                var message = this.$store.getters[constants.sendList].message;

                                if(message == constants.recordNotExist){
                                   this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                     });
                                }
                                else if(message == constants.forgetPasswordSent){
                                     this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                        this.$router.push('/');
                                    });
                                }
                                else{
                                    this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.modal_error }).then(() => {
                        
                                    });
                                }
                            }
                            
                            }, 1000)
                        })
                        
                    }
                    this.withError = false;
                } else {
                    this.withError = true; 
                }

            },
            handleConnectivityChange(status) {
                if (status === false) {
                    this.status = false;
                    this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                        this.$router.push('/');
                    });
                }
                else {
                    this.status = true;
                }
            },
            redirect() {
                
            },
            back(){
                this.$router.push('./')
            }

        }
    };
</script>

<style>
</style>
