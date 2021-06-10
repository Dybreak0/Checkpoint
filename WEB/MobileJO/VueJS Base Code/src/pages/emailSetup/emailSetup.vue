<template>
    <v-container class="child-body">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-layout row>
            <v-card flat class="search-filter-card">
                <h2> <v-icon>mail</v-icon> Email Setup</h2>
                <v-spacer></v-spacer>
            </v-card>
        </v-layout>
        <v-layout row>
            <v-divider></v-divider>
        </v-layout>
        <v-spacer></v-spacer>
        <v-container>
            <v-layout row>
                <v-flex md12>
                    <v-card class="search-filter-vcard">
                        <p class="emailSetupCard">Default email recipients:</p>
                    </v-card>
                </v-flex>
            </v-layout>

            <v-spacer></v-spacer>
            <v-form v-model="valid" ref="form">
                <v-layout row>
                    <v-flex md1 class="input-label text-xs-right">
                        <span>To <b>*</b></span>
                    </v-flex>
                    <v-flex md10>
                        <emailFormTo v-if="loaded" :key="formKey" :list=list ref="toRef" required></emailFormTo>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex md1 class="input-label text-xs-right">
                        <span>Cc</span>
                    </v-flex>
                    <v-flex md10>
                        <emailFormCC v-if="loaded" :key="formKey" :list=list ref="ccRef"></emailFormCC>
                        <emailFormCC v-else></emailFormCC>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs12 md1 class="input-label text-xs-right">
                        <span>Bcc</span>
                    </v-flex>
                    <v-flex xs12 md10>
                        <emailFormBCC v-if="loaded" :key="formKey" :list=list ref="bccRef"></emailFormBCC>
                        <emailFormBCC v-else></emailFormBCC>
                    </v-flex>
                </v-layout>


                <v-layout row class="text-xs-right">
                    <v-flex md11 class="button-field"
                            xs12>
                        <v-btn v-if="edit" @click="cancel" class="btn_secondary">
                            <v-icon>keyboard_return</v-icon>
                            Cancel
                        </v-btn>
                        <v-btn v-if="!edit" @click="enableEdit" class="btn_primary">
                            <v-icon>edit</v-icon>
                            Edit
                        </v-btn>
                        <v-btn v-else class="btn_primary" @click="save">
                            <v-icon>save</v-icon>Save
                        </v-btn>
                    </v-flex>
                </v-layout>

            </v-form>

        </v-container>
         </v-container>

</template>

<script>
    import { mapGetters } from 'vuex';
    import emailFormTo  from '../emailSetup/components/email_form_to';
    import emailFormCC from '../emailSetup/components/email_form_cc';
    import emailFormBCC from '../emailSetup/components/email_form_bcc';
    import confirm from '../../common/layout/confirm-modal';
    import info from '../../common/layout/info-modal';
    import loading from '../../common/layout/progress';
    import offline from 'v-offline';
    import constants from '../../common/utils/constants';

    export default {
        components: {
            emailFormTo,
            emailFormCC,
            emailFormBCC,
            confirm,
            info,
            loading,
            offline
        },

        created() {
            this.getEmails();
        },


        computed: {
            ...mapGetters({
                list: constants.listEmail,
                message: constants.listMessage
            }),
        },

        data() {
            return {
                valid: false,
                edit: false,
                edited: true,
                loaded: false,
                emails: constants.defaultEmail,
                fullscreenLoading: false,
                formKey: 0,
                status: true,
                oldEmails: []
            }
        },

        methods: {
            enableEdit() {
                if(this.loaded){
                    if(this.edited === true ){
                        this.edit = this.edit ? false : true;
                        this.$refs.toRef.toggleEnable();
                        this.$refs.ccRef.toggleEnable();
                        this.$refs.bccRef.toggleEnable();
                    }
                }
            },
            clearEmails() {
                this.emails = constants.defaultEmail;
            },
            getEmails() {
                this.fullscreenLoading = true;
                this.loaded = true;
                this.$store.dispatch(constants.listEmail).then(() => {
                    this.fullscreenLoading = false;
                    setTimeout(() => {
                        var message = this.$store.getters[constants.listMessage];
                        if (message !== constants.success) {
                            this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                            });
                        } else {
                            this.getOldEmails();
                        }
                    }, );
                });

            },
            save() {
                var all = (this.$refs.toRef.getValues()).concat((this.$refs.ccRef.getValues()), (this.$refs.bccRef.getValues()))
                if (JSON.stringify(all) !== this.oldEmails)
                {
                    setTimeout(() => {
                        this.$refs.form.validate();  
                        if (this.valid) {
                            if (this.status === false) {
                                this.handleConnectivityChange(this.status);
                            }
                            else {
                                this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                                    if (confirm) {
                                        this.fullscreenLoading = true;
                                        this.edited = true;
                                        this.enableEdit();
                                        this.clearEmails();
                                        var all = (this.$refs.toRef.getValues()).concat((this.$refs.ccRef.getValues()), (this.$refs.bccRef.getValues()))
                                        this.emails.email = all;
                                        this.$store.getters[constants.emailModel][0] = (JSON.stringify(this.emails));
                                        this.$store.dispatch(constants.saveEmails).then(() => {
                                            var message = this.$store.getters[constants.listMessage];

                                            if (message === constants.maxEmail) {
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.getEmails();
                                                });
                                            }
                                            else if (message === constants.deletedUser) {
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.clearStore();
                                                });
                                            }
                                            else if (message === constants.notAdmin) {
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.clearStore();
                                                });
                                            }
                                            else if (message === constants.saved) {
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.getEmails();
                                                });
                                            }
                                            else {
                                                this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                                    //this.loaded = true;
                                                    //this.getEmails();
                                                });
                                            }
                                            this.fullscreenLoading = false;
                                        });
                                     }
                                })
                            }
                        }
                    }, 300);
                }
                else if (!this.emailFormTo)
                {
                    this.$refs.info.open(constants.message, constants.fillRequireFieldsError, { color: constants.modal_color }).then(() => { });                   
                }
                else {
                    this.$refs.info.open(constants.message, constants.saved, { color: constants.modal_color }).then(() => {});
                    this.enableEdit();
                }  
            },


            cancel() {
                var all = (this.$refs.toRef.getValues()).concat((this.$refs.ccRef.getValues()), (this.$refs.bccRef.getValues()))
                if (JSON.stringify(all) === this.oldEmails) {
                    this.formKey++;
                    this.edit = false;
                }
                else {
                    this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                        if (confirm) {
                            this.formKey++;
                            this.edit = false;
                        }
                    })
                }

            },
            getOldEmails() {
                if(this.$store.getters[constants.listEmail]){
                    this.oldEmails = JSON.stringify(this.$store.getters[constants.listEmail]);
                    this.formKey++;
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
            },

        },

    };
</script>




