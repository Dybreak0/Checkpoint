<template>
    <v-form>
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <v-container class="child-body" v-if="active">
            <v-layout row>
                <v-card flat>
                    <h2>
                        <v-icon>person</v-icon> View Account Details
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
                        md12
                        offset-xs1
                         class="detail-row1">
                    <v-layout>
                        <v-flex md1 class="text-xs-right ">
                            <span><b>Name: </b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            {{model.data.name}}
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout row>
                <v-flex md12
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class="text-xs-right">
                            <span><b>Memo:</b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            {{model.data.memo}}
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout row>
                <v-flex md12
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class=" text-xs-right">
                            <span><b>Email Address:</b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            {{model.data.email_address}}
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout row>
                <v-flex md12
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class=" text-xs-right">
                            <span><b>Contact Person:</b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            {{model.data.contact_person}}
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout row>
                <v-flex md12
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class=" text-xs-right">
                            <span><b>Contact Number:</b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            {{model.data.contact_number}}
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout row>
                <v-flex md10
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class=" text-xs-right">
                            <span><b>Address:</b></span>
                        </v-flex>
                        <v-flex md10 class="detail">
                            {{model.data.address}}
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <div class="divider">
                <v-divider></v-divider>
            </div>
            <v-layout row>
                <v-flex md12
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class="text-xs-right">
                            <span><b>Created:</b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            <span v-if="!!model.data.created_by_name"> {{model.data.created_by_name}} ({{this.created_date}})</span>

                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout>
                <v-flex md12
                        offset-xs1
                        class="detail-row1">
                    <v-layout>
                        <v-flex md1 class="text-xs-right">
                            <span><b>Updated:</b></span>
                        </v-flex>
                        <v-flex md12 class="detail">
                            <span v-if="!!model.data.updated_by_name"> {{model.data.updated_by_name}} ({{this.updated_date}})</span>
                        </v-flex>
                    </v-layout>
                </v-flex>
            </v-layout>

            <v-layout row class="detail-row">
                <v-flex class="button-field"
                        xs12
                        offset-xs9>
                    <v-layout>
                        <v-flex>
                            <v-btn @click="back" class="btn_secondary">
                                <v-icon>keyboard_return</v-icon>
                                Back
                            </v-btn>
                        </v-flex>
                        <v-flex>
                            <v-btn @click="deleteItem" class="btn_primary">
                                <v-icon>close</v-icon>Delete
                            </v-btn>
                        </v-flex>
                        <v-flex md10>
                            <v-btn @click="editItem" class="btn_primary">
                                <v-icon>save</v-icon>Edit
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
    import constants from '../../../common/utils/constants';
    import moment from 'moment';
    
    export default {
        data: () => ({
            fullscreenLoading: false,
            active: false,
            status: true,
            created_date: '',
            updated_date: ''
        }),
        components: {
            confirm,
            info,
            loading,
            offline
        },
        created() {
            this.fullscreenLoading = true;
            this.$store.dispatch(constants.account, this.$props.id).then(() => {
                setTimeout(() => {
                    var message = this.$store.getters[constants.accountModel].message;
                    if(this.$store.getters[constants.accountModel].message === constants.noResults){
                        this.$refs.info.open(constants.warning, constants.recordNotExist, { color: constants.error_color }).then(() => {
                                        this.$router.push({ path: constants.accountList })
                            });
                    }
                    else{
                        if(this.$store.getters[constants.accountModel] === constants.noInternet){
                            this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                this.$router.push({ path: constants.accountList });
                            });
                        }
                        else if (message === constants.deletedUser) {
                            this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                this.clearStore();
                            });
                        }
                        else if (message === constants.notAdmin) {
                            this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                this.clearStore();
                            });
                        }
                        else if (this.$store.getters[constants.accountModel].data.is_active) {
                            this.active = true;
                            this.created_date = moment(this.$store.getters[constants.accountModel].data.created_date).format('MM/DD/YYYY hh:mm:ss');
                            this.updated_date = moment(this.$store.getters[constants.accountModel].data.updated_date).format('MM/DD/YYYY hh:mm:ss');
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
        props: {
            id: String
        },
        computed: {
            ...mapGetters({
                model: constants.accountModel,
            }),
        },
        methods: {
            editItem() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                } else {
                    this.$router.push({ path: constants.accountEdit + this.$props.id })
                }
            },
            deleteItem() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$refs.confirm.open(constants.confirm, constants.deleteConfirm, { color: constants.modal_color }).then((confirm) => {
                        if (confirm) {
                            this.fullscreenLoading = true;
                            this.$store.dispatch(constants.destroyAccount, this.$props.id).then(() => {
                                setTimeout(() => {
                                    this.fullscreenLoading = false;
                                    var message = this.$store.getters[constants.listAccount].message;
                                    if(!this.$store.getters[constants.listAccount].message){
                                        this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                            this.$router.push({ path: constants.accountList });
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
                                    }else{
                                        this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                            this.back();
                                        });
                                     }
                                   
                                }, 1000);
                            });
                        }
                    })
                }
               
            },
            back() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.accountList });
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
            }

        }
    }
</script>

<style>
</style>
