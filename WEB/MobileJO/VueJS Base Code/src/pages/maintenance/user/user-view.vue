<template>
    <v-container class="child-body">
        <v-form>
            <confirm ref="confirm"></confirm>
            <info ref="info"></info>
            <loading v-if="fullscreenLoading"></loading>
            <offline @detected-condition="handleConnectivityChange"></offline>
            <v-container v-if="active">
                <v-layout row>
                    <v-card flat>
                        <h2>
                            <v-icon>person</v-icon> View User Details
                        </h2>
                        <v-spacer></v-spacer>
                    </v-card>
                </v-layout>
                <v-layout row>
                    <v-divider></v-divider>
                </v-layout>
                <v-spacer class="formsSpacer"></v-spacer>

                <v-layout row>
                    <v-flex xs3                         
                            class="detail-row1">
                        <v-layout>
                            <v-flex md3 class="text-xs-right ">
                                <span><b>First Name: </b></span>
                            </v-flex>
                            <v-flex md12 class="detail">
                                {{model.data.first_name.substring(0,60)}}
                                {{model.data.first_name.substring(60,100)}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs12
                            md4>
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Allowed To Login: </b></span>
                            </v-flex>
                            <v-flex xs12 md6 class="detail form_cb">
                                <v-checkbox v-model="model.data.allowed_to_login" disabled hide-details></v-checkbox>
                            </v-flex>
                        </v-layout>

                    </v-flex>
                </v-layout>
                <v-layout row>
                    <v-flex xs4
                            md5
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md3 class="text-xs-right">
                                <span><b>Last Name:</b></span>
                            </v-flex>
                            <v-flex md12 class="detail">
                                {{model.data.last_name.substring(0,60)}}
                                {{model.data.last_name.substring(60,100)}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs12
                            md4>
                        <v-layout>
                            <v-flex xs4 md4 class=" text-xs-right ">
                                <span><b>Address:</b></span>
                            </v-flex>
                            <v-flex xs12 md6  class="detail">
                                {{model.data.address.substring(0,60)}}
                                {{model.data.address.substring(60,120)}}
                                {{model.data.address.substring(120,180)}}
                                {{model.data.address.substring(180,240)}}
                                {{model.data.address.substring(240,250)}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>
                <v-layout row>
                    <v-flex xs4
                            md5
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md3 class="text-xs-right">
                                <span><b>Memo:</b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                {{model.data.memo}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs12
                            md4>
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Telephone Number:</b></span>
                            </v-flex>
                            <v-flex xs12 md6>
                                <v-flex xs12 class="detail">
                                    {{model.data.telephone_no}}
                                </v-flex>
                            </v-flex>
                        </v-layout>

                    </v-flex>
                </v-layout>
                <v-layout row>
                    <v-flex xs4
                            md5
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md3 class="text-xs-right">
                                <b>Company:</b>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                {{companyName}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs12
                            md4>
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Phone/Mobile Number:</b></span>
                            </v-flex>
                            <v-flex xs12 md6>
                                <v-flex xs12 class="detail">
                                    {{model.data.mobile_no}}
                                </v-flex>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>
                <v-layout row>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <b>Branch:</b>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                {{branchName}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <b>User Type:</b>
                            </v-flex>
                            <v-flex xs12 md6 class="detail">
                                {{userTypeName}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>
                <v-layout row>
                    <v-flex xs4
                            md5
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md3 class=" text-xs-right ">
                                <span><b>Email Address:</b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                {{model.data.email_address}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex xs12
                            md4>
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Role: </b></span>
                            </v-flex>
                            <v-flex xs12 md6 class="detail">
                                {{roleName}}

                            </v-flex>
                        </v-layout>

                    </v-flex>
                </v-layout>
                <div class="divider">
                    <v-divider></v-divider>
                </div>
                <v-layout row>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Username: </b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                {{model.data.user_name}}
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Password:</b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                ********
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Confirm Password:</b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                ********
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>


                <div class="divider">
                    <v-divider></v-divider>
                </div>
                <v-layout row>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Created:</b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                <span v-if="!!model.data.created_by"> {{model.data.created_by}} ({{this.created_date}})</span>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex xs4
                            md4
                            offset-xs1
                            class="detail-row1">
                        <v-layout>
                            <v-flex xs4 md4 class="text-xs-right">
                                <span><b>Updated:</b></span>
                            </v-flex>
                            <v-flex xs12 class="detail">
                                <span v-if="!!model.data.updated_by"> {{model.data.updated_by}} ({{this.updated_date}})</span>
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
                                    <v-icon>close</v-icon>
                                    Delete
                                </v-btn>
                            </v-flex>
                            <v-flex md10>
                                <v-btn @click="editItem" class="btn_primary">
                                    <v-icon>edit</v-icon>
                                    Edit
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
    import { mapGetters } from 'vuex';
    import offline from 'v-offline';
    import confirm from '../../../common/layout/confirm-modal';
    import info from '../../../common/layout/info-modal';
    import loading from '../../../common/layout/progress';
    import constants from '../../../common/utils/constants';
    import moment from 'moment';

    export default {
        data: () => ({
            checkbox2: false,
            roleName: '',
            userTypeName: '',
            companyName: '',
            branchName:'',
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
            this.$store.dispatch(constants.user, this.$props.id).then(() => {
                setTimeout(() => {
                    var message = this.$store.getters[constants.userModel].message;
                    if (message === constants.noResults) {
                        this.$refs.info.open(constants.warning, constants.recordNotExist, { color: constants.error_color }).then(() => {
                            this.$router.push({ path: constants.userList });
                        });
                    }
                    //else if (message === constants.recordNotExist) {
                    //    this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                    //        this.$router.push({ path: constants.userList });
                    //    });
                    //}
                    else {
                        if(this.$store.getters[constants.userModel] === constants.noInternet){
                            this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                this.$router.push({ path: constants.userList });
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
                        else if (this.$store.getters[constants.userModel].data.is_active) {
                            this.active = true;
                            this.$store.dispatch(constants.getUserRoles).then(() => {
                                var roleID = this.model.data.role_id;

                                for (var index = 0; index < this.roles.length; ++index) {
                                    if (this.roles[index].value === roleID) {
                                        this.roleName = this.roles[index].text;
                                    }

                                }
                                this.created_date = moment(this.$store.getters[constants.userModel].data.created_date).format('MM/DD/YYYY hh:mm:ss');
                                this.updated_date = moment(this.$store.getters[constants.userModel].data.updated_date).format('MM/DD/YYYY hh:mm:ss');
                            })


                            this.$store.dispatch(constants.getUserTypes).then(() => {
                                var userTypeID = this.model.data.user_type_id;

                                for (var index = 0; index < this.userTypes.length; ++index) {
                                    if (this.userTypes[index].value === userTypeID) {
                                        this.userTypeName = this.userTypes[index].text;
                                    }

                                }
                            })

                            this.$store.dispatch(constants.getCompanies).then(() => {
                                var companyID = this.model.data.company_id;

                                for (var index = 0; index < this.companies.length; ++index) {
                                    if (this.companies[index].value === companyID) {
                                        this.companyName = this.companies[index].text;
                                    }

                                }
                            })

                            var companyID = this.model.data.company_id;
                            this.$store.dispatch(constants.getBranches, companyID).then(() => {
                                var branchID = this.model.data.branch_id;

                                for (var index = 0; index < this.branches.length; ++index) {
                                    if (this.branches[index].value === branchID) {
                                        this.branchName = this.branches[index].text;
                                    }
                                }
                            })

                        } else {
                            this.$refs.info.open(constants.warning, constants.notAvailable, { color: constants.error_color}).then(() => {
                                this.$router.push({ path: constants.userList })
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
                model: constants.userModel,
                roles: constants.userRoles,
                userTypes: constants.userTypes,
                companies: constants.companies,
                branches: constants.branches,
            })
        },
        methods: {
            back() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.userList });
                }
            },
            editItem() {
                 if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                } else {
                    this.$router.push({ path: constants.userEdit +  this.$props.id });
                }
            },

            viewItem(id) {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.userView + id });
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
                            this.$store.dispatch(constants.destroyUser, this.$props.id).then(() => {
                            setTimeout(() => {
                                    this.fullscreenLoading = false;
                                    var message = this.$store.getters[constants.listUsers].message;
                                    if(!this.$store.getters[constants.listUsers].message){
                                        this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                            this.$router.push({ path: constants.userList });
                                        });
                                    }
                                    else if(message.ModelStateErrors !== undefined){
                                        for(let i=0; i < message.ModelStateErrors.length; i++){
                                            if(message.ModelStateErrors[i] === constants.recordNotExist){
                                                this.$refs.info.open(constants.warning, constants.notAvailable, { color: constants.error_color }).then(() => {
                                                    this.$router.push({ path: constants.userList });
                                                });
                                                break;
                                            }
                                            this.$refs.info.open(constants.warning, message.ModelStateErrors[i], { color: constants.error_color }).then(() => {});
                                        }
                                    }
                                    else{
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

