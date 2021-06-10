<template>
    <v-container class="child-body">
        <info ref="info"></info>
        <confirm ref="confirm"></confirm>
        <loading v-if="fullscreenLoading"></loading>
        <v-layout row>
            <v-card flat class="search-filter-card">
                <h2><v-icon>description</v-icon> View JO Details</h2>
                <v-spacer></v-spacer>
            </v-card>
        </v-layout>
        <v-layout row><v-divider></v-divider></v-layout>
        <v-spacer></v-spacer>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>JO #: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.job_order_number}}</v-flex>
                </v-layout>
            </v-flex>
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>JO Date and Time Start: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.job_order_datetime_start | formatDateTime}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>JO Subject: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.job_order_subject}}</v-flex>
                </v-layout>
            </v-flex>
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>JO Date and Time End: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.job_order_datetime_end | formatDateTime}}</v-flex>
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Status: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.status_name}}</v-flex>
                </v-layout>
            </v-flex>
             <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Client Name: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.account_name}}</v-flex>
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Application Type: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.application_type_name}}</v-flex>
                </v-layout>
            </v-flex>
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Branch Name: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.branch_name}}</v-flex>
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Reported By: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.reported_by_name}}</v-flex>
                </v-layout>
            </v-flex>
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Satisfied With Service: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail" v-if="viewModel.status_name != 'Pending'">{{viewModel.is_satisfied? "Yes" : "No"}}</v-flex>     
                    <v-flex xs9 md9 class="detail" v-if="viewModel.status_name == 'Pending'"><em>No feedback yet.</em></v-flex>               
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Billed: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.is_billed? "Yes" : "No"}}</v-flex>
                </v-layout>
            </v-flex>
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Client Rating: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail" v-if="viewModel.status_name != 'Pending'">{{ viewModel.client_rating}}</v-flex>
                    <v-flex xs9 md9 class="detail" v-if="viewModel.status_name == 'Pending'"><em>No rating yet.</em></v-flex>
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row >
            <v-flex xs6 md6>
                <v-layout>           
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Billing Type: </b></span></v-flex>         
                    <v-flex xs2 md2 class="detail-3 detail-4"><v-checkbox v-model="billed.isWarranty" :label="'Warranty'" disabled></v-checkbox></v-flex>
                    <v-flex xs2 md2 class="detail-4"><v-checkbox v-model="billed.isWebPOS" :label="'WebPOS'" disabled></v-checkbox></v-flex>                    
                    <v-flex xs2 md2 class="detail-4"><v-checkbox v-model="billed.isPending" :label="'Pending'" disabled></v-checkbox></v-flex>
                    <v-flex xs2 md2 class="detail-4"><v-checkbox v-model="billed.isAPS" :label="'APS'" disabled></v-checkbox></v-flex>
                </v-layout>
            </v-flex>
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Client Signatory: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail" v-if="viewModel.client_signature != null">
                        <div class="detail-1">
                            <u><v-icon>attach_file</v-icon><a @click="download(viewModel.id, viewModel.client_signature, 'ClientSignature')">{{viewModel.client_signature}}</a></u>
                        </div>
                    </v-flex>
                        <v-flex xs9 md9 class="detail" v-if="viewModel.client_signature == null"><em>No signature yet.</em></v-flex>
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Collaterals: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.is_collaterals? "Yes" : "No"}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs6 md6>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Attachments: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail" v-if="attachments.length != 0">
                        <div class="detail-1" v-for="attachment in attachments" :key="attachment.ID">
                            <u><v-icon>attach_file</v-icon><a @click="download(attachment.JobOrderID, attachment.Filename, 'Attachments')">{{attachment.Filename}}</a></u>
                        </div>                    
                    </v-flex>
                    <v-flex xs9 md9 class="detail" v-if="attachments.length == 0"><em>No attachments.</em></v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs9 md9>
                <v-layout>
                    <v-flex xs2 md2 class="text-xs-right"><span><b>Activity Details: </b></span></v-flex>
                    <v-flex xs10 md10 class="detail pre-white-space">{{viewModel.activity_details}}</v-flex>                    
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs9 md9>
                <v-layout>
                    <v-flex xs2 md2 class="text-xs-right"><span><b>Root Cause Analysis: </b></span></v-flex>
                    <v-flex xs10 md10 class="detail pre-white-space">{{viewModel.root_cause_analysis}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs9 md9>
                <v-layout>
                    <v-flex xs2 md2 class="text-xs-right"><span><b>Next Step: </b></span></v-flex>
                    <v-flex xs10 md10 class="detail pre-white-space">{{viewModel.next_step}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs9 md9>
                <v-layout>
                    <v-flex xs2 md2 class="text-xs-right"><span><b>Preventive Action: </b></span></v-flex>
                    <v-flex xs10 md10 class="detail pre-white-space">{{viewModel.preventive_action}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>  
        <v-layout row class="detail-row2">
            <v-flex xs9 md9>
                <v-layout>
                    <v-flex xs2 md2 class="text-xs-right"><span><b>Remarks: </b></span></v-flex>
                    <v-flex xs10 md10 class="detail pre-white-space">{{viewModel.remarks}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs9 md9>
                <v-layout>
                    <v-flex xs2 md2 class="text-xs-right"><span><b>Attendees: </b></span></v-flex>
                    <v-flex xs10 md10 class="detail pre-white-space">{{viewModel.attendees}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>                     
        <v-layout row class="detail-row2">
            <v-flex class="button-field" xs12>
                <v-layout xs12 md12 class="detail-2">
                    <v-flex><v-btn color="btn_secondary" @click="back()"><v-icon>keyboard_backspace</v-icon>&nbsp;&nbsp;Back</v-btn> </v-flex>
                    <v-flex><v-btn color="btn_primary" v-if="viewModel.status_name == 'Requested For Revert'" @click="revert(true)"><v-icon>thumb_up</v-icon>&nbsp;&nbsp;Approve Revert</v-btn></v-flex>
                    <v-flex xs10 md10><v-btn color="secondary" v-if="viewModel.status_name == 'Requested For Revert'" @click="revert(false)"><v-icon>thumb_down</v-icon>&nbsp;&nbsp;Deny Revert</v-btn></v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
    </v-container>
</template>
<script>
    import { mapGetters } from 'vuex';
    import info from '../../../common/layout/info-modal';
    import confirm from '../../../common/layout/confirm-modal';    
    import loading from '../../../common/layout/progress';
    import constants from '../../../common/utils/constants';

    export default {
        data() {
            return {
                attachments : [],
                billingTypes: [],
                fullscreenLoading: false,                
                billed : {
                   isWarranty: false,
                   isWebPOS: false,
                   isAPS: false,
                   isPending: false
                },
                attachmentPath : '',
                clientSignature : ''
            }
        },
        created() {
            this.initialize();
        },
        beforeRouteLeave(to, from, next) {
            this.$store.dispatch(constants.jobOrderReportClear);
            next();
        },
        props: {
            id: String,
            prevPage : String
        },
        components: {
            info,
            confirm,            
            loading,
            constants
        },
        methods: {
            initialize() {
                this.$store.dispatch(constants.jobOrderReportView);   
                // Store previous page route to session storage

                if (this.prevPage === undefined) {
                    localStorage.setItem(constants.prevPage, constants.jobOrderReport);                    
                } else {
                    localStorage.setItem(constants.prevPage, this.prevPage);
                }

                this.fullscreenLoading = true;
                setTimeout(() => {
                    this.$store.dispatch(constants.jobOrderReportGet, this.id).then(() => {   
                    this.fullscreenLoading = false;

                        if (this.errorMessage === null) {
                            this.attachments = (this.viewModel.job_order_attachment != null) ? this.viewModel.job_order_attachment : this.attachments;
                            this.billingTypes = this.viewModel.job_order_billing_type;

                                if (this.billingTypes !== undefined) {

                                    for (var i = 0; i < this.billingTypes.length; i++) {
                                        switch (this.billingTypes[i].BillingTypeID) {
                                            case 1:
                                                this.billed.isWarranty = true;
                                                break;
                                            case 2:
                                                this.billed.isWebPOS = true;
                                                break;
                                            case 3:
                                                this.billed.isAPS = true;
                                                break;
                                            case 4:
                                                this.billed.isPending = true;
                                                break;
                                        }
                                    } 

                                }
                                               
                        } else {
                            this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color }).then(() => {
                                var prevPage = localStorage.getItem(constants.prevPage);
                                this.$router.push({ name: prevPage });                     
                            });
                        }                    
                    });
                }, 300);
            },
            revert(isApproved) {                
                var jobOrderId = this.id;
                var jobOrderRevertId = this.viewModel.job_order_revert_id;
                var confirmMessage = (isApproved? constants.approveRevert : constants.denyRevert);
                this.$refs.confirm.open(constants.confirm, confirmMessage, { color: constants.modal_color }).then((confirm) => {
                    if (confirm) {
                        this.fullscreenLoading = true;
                        this.$store.dispatch(constants.jobOrderReportRevertJO, {jobOrderId, jobOrderRevertId, isApproved}).then(() => {
                            this.fullscreenLoading = false;
                            if (this.errorMessage === constants.deletedUser) {
                                this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.modal_color }).then(() => {
                                    this.clearStore();
                                });
                            }
                            else if (this.errorMessage === constants.notAdmin) {
                                this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.modal_color }).then(() => {
                                    this.clearStore();
                                });
                            }
                            else if (this.errorMessage === null) {
                                this.$refs.info.open(constants.message, this.successMessage, { color: constants.modal_color }).then(() => {
                                    this.initialize();
                                });
                            }

                            else {
                                this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color }).then(() => {
                                    this.initialize();
                                });
                            }
                        });    
                    }
                }); 
            },
            download(jobOrderId, attachmentFileName, attachmentType) {
                this.fullscreenLoading = true;
                var attachment = {
                    jobOrderId : jobOrderId,
                    fileName : attachmentFileName,
                    type : attachmentType
                };

                this.$store.dispatch(constants.jobOrderReportDownloadAttachment, attachment).then(() => {
                    this.fullscreenLoading = false;
                    if (this.errorMessage != null) {
                        this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color }); 
                    }
                });
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
            back() {
                var prevPage = localStorage.getItem(constants.prevPage);
                this.$router.push({ name: prevPage });
            },
        },
        computed: {            
            ...mapGetters({
                viewModel: constants.jobOrderReportViewModel,
                errorMessage: constants.jobOrderReportErrorMessage,
                successMessage: constants.jobOrderReportSuccessMessage,
            }),
        }
    }
</script>
