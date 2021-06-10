<template>
    <v-container class="child-body">
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <v-layout row>
            <v-card flat class="search-filter-card">
                <h2><v-icon>description</v-icon>View Case Details</h2>
                <v-spacer></v-spacer>
            </v-card>
        </v-layout>
        <v-layout row>
            <v-divider></v-divider>
        </v-layout>
        <v-spacer></v-spacer>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Case #: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.case_number}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Status: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.status}}</v-flex>
                </v-layout>
            </v-flex>         
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Application Type: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.application_type_name}}</v-flex>
                </v-layout>
            </v-flex>        
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Subject: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.case_subject}}</v-flex>
                </v-layout>
            </v-flex>          
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Priority: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.priority}}</v-flex>
                </v-layout>
            </v-flex>        
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Account Name: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.account_name}}</v-flex>
                </v-layout>
            </v-flex>          
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Description: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.description}}</v-flex>                    
                </v-layout>
            </v-flex>            
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Assigned To: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.assigned_to_name}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>   
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Date and Time Modified: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.datetime_modified | formatDateTime}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="detail-row2">
            <v-flex xs7 md7>
                <v-layout>
                    <v-flex xs3 md3 class="text-xs-right"><span><b>Date and Time Created: </b></span></v-flex>
                    <v-flex xs9 md9 class="detail">{{viewModel.datetime_created | formatDateTime}}</v-flex>
                </v-layout>
            </v-flex>
        </v-layout>                              
        <v-layout row class="detail-row2">
            <v-flex class="button-field" xs12>
                <v-layout xs12 md12 class="detail-2">
                    <v-flex class="buttonAdjust">
                        <v-btn color="btn_secondary" @click="back()"><v-icon>keyboard_backspace</v-icon>&nbsp;&nbsp;Back</v-btn>
                    </v-flex>                   
                </v-layout>
            </v-flex>
        </v-layout>
    </v-container>
</template>
<script>
    import { mapGetters } from 'vuex';
    import confirm from '../../../common/layout/confirm-modal';
    import info from '../../../common/layout/info-modal';
    import loading from '../../../common/layout/progress';
    import constants from '../../../common/utils/constants';

    export default {
        data() {
            return {
                fullscreenLoading: false
            }
        },
        created() {
            this.initialize();
        },
        beforeRouteLeave(to, from, next) {
            this.$store.dispatch(constants.assignedCaseReportClear);
            next();
        },
        props: {
            id: String,
        },
        components: {
            confirm,
            info,
            loading
        },
        methods: {
            initialize() {
                this.fullscreenLoading = true;
                this.$store.dispatch(constants.assignedCaseReportGet, this.id).then(() => {   
                    this.fullscreenLoading = false;
                    if (this.errorMessage !== null) {
                        this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color }).then(() => {
                                //this.$router.push({ name: constants.assignedCaseReport });
                            this.clearStore();
                        });
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
                this.$router.push({ name: constants.assignedCaseReport });
            },
        },
        computed: {            
            ...mapGetters({
                viewModel: constants.assignedCaseReportViewModel,
                errorMessage: constants.assignedCaseReportErrorMessage,
            }),
        }
    }
</script>
