<template>
    <v-container class="child-body">
        <info ref="info"></info>
        <confirm ref="confirm"></confirm>
        <loading v-if="fullscreenLoading"></loading>
        <v-form>
            <v-layout row>
                <v-card flat class="search-filter-card">
                    <h2><v-icon>restore</v-icon> Revert JO Request</h2>
                    <v-spacer></v-spacer>
                </v-card>
            </v-layout>
            <v-layout row><v-divider></v-divider></v-layout>
            <v-spacer></v-spacer>
            <v-layout row>
                <v-flex xs12 m10>
                    <p><b>Search Filters</b></p>
                </v-flex>
            </v-layout>
            <v-layout row>
                <v-flex xs12 m10>
                    <v-card class="search-filter-vcard" flat tile>
                        <v-layout row>
                            <v-flex xs5 md5>
                                <v-layout>
                                    <v-flex xs4 md4 class="input-label text-xs-right">
                                        <span>JO #: </span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-text-field 
                                            v-model="searchModel.job_order_number"
                                            prepend-inner-icon="search"                                            
                                            single-line
                                            solo
                                            hide-details
                                            color="red"
                                            @keyup.enter="search">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                            <v-flex xs5 md5>
                                <v-layout>
                                    <v-flex xs4 md4 class="input-label text-xs-right">
                                        <span>Reported By: </span>
                                    </v-flex>
                                    <v-flex xs7 md7>
                                        <v-text-field 
                                            v-model="searchModel.reported_by"
                                            prepend-inner-icon="search"
                                            single-line
                                            solo
                                            hide-details
                                            color="red"
                                            @keyup.enter="search">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>                        
                        <v-layout row>
                            <v-flex xs12 md12>
                                <v-layout>
                                    <v-flex md12 class="text-xs-right">
                                        <v-btn class="btn_secondary" @click="search"><v-icon>search</v-icon>Search</v-btn>
                                        <v-btn class="btn_secondary buttonClearAdjust" @click="clearFilters"><v-icon>clear</v-icon>Clear</v-btn>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                    </v-card>
                </v-flex>
            </v-layout>
        </v-form>
        <v-spacer></v-spacer>
        <v-layout row class="table-spacer">
            <v-flex xs12 m10>
                <v-data-table 
                    :headers="headers"
                    :items="list.data"
                    :no-data-text="defaultTableText"
                    hide-actions>
                    <template v-slot:items="props">
                        <td class="text-xs-center"><a @click="view(props.item.job_order_id, props.item.id)"><u>{{ props.item.job_order_number }}</u></a></td>
                        <td class="text-xs-left">{{ props.item.application_type_name }}</td>
                        <td class="text-xs-center">{{ props.item.job_order_datetime_start | formatDate}}</td>
                        <td class="text-xs-center">{{ props.item.job_order_datetime_end | formatDate}}</td>
                        <td class="text-xs-left">{{ props.item.activity_details }}</td>
                        <td class="text-xs-left">{{ props.item.reported_by_name }}</td>
                        <td class="text-xs-center">
                            <v-icon small
                                    class="mr-2 action-ic approve-ic"
                                    @click="revertJO(props.item.job_order_id, props.item.id, true)">
                                thumb_up
                            </v-icon>
                            <v-icon small
                                    class="mr-2 action-ic deny-ic"
                                    @click="revertJO(props.item.job_order_id, props.item.id, false)">
                                thumb_down
                            </v-icon>
                        </td>
                    </template>
                </v-data-table>
                <div class="text-xs-center pt-2">
                    <pagination
                        v-if="pages > 1"
                        :maxVisibleButtons="5"
                        :total-pages="pages"
                        :current-page="searchModel.page"
                        @pagechanged="onPageChange"
                    />
                </div>
            </v-flex>
        </v-layout>
    </v-container>
</template>
<script>
    import axios from 'axios'
    import { mapGetters } from 'vuex';
    import info from '../../../common/layout/info-modal';
    import confirm from '../../../common/layout/confirm-modal';
    import loading from '../../../common/layout/progress';
    import pagination from '../../../common/components/pagination';
    import constants from '../../../common/utils/constants';

    export default {
        name: constants.revertJOPage,
        data() {
            return {
                pages: 0,
                newPage: 1,
                isPagination: false,
                menu: false,
                menu2: false,
                fullscreenLoading: false,
                defaultTableText: constants.noRecords,
                headers: [
                    { text: constants.jobOrderNumberHeader, align: constants.defaultTableAlign, sortable: false, width: "10%" },
                    { text: constants.applicationTypeHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"},
                    { text: constants.jobOrderDateStartHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"},
                    { text: constants.jobOrderDateEndHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"},           
                    { text: constants.detailsHeader, align: constants.defaultTableAlign, sortable: false, width: "35%" },      
                    { text: constants.reportedByHeader, align: constants.defaultTableAlign, sortable: false, width: "15%"},
                    { text: constants.actionHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"}
                ],
            }
        },
        created() {
            this.initialize();
            this.search();
        },
        components: {
            info,
            confirm,            
            loading,
            pagination,
            constants
        },
        methods: {
            initialize() {
                this.$store.dispatch(constants.revertJONew);           // Initialize all properties of the search model
            },
            onPageChange(page) {
                this.newPage = page;
                this.isPagination = true;
                this.search();
            },
            clearFilters() {
                this.$store.dispatch(constants.revertJOClearFilters);             // Resets the search filters to the default values
                this.search();
            },
            search() {
                this.fullscreenLoading = true;
                this.searchModel.page = (this.isPagination) ? this.newPage : 1;
                this.$store.dispatch(constants.revertJOList).then(() => {   // Searches for job order records using the search filters 
                    this.fullscreenLoading = false;                    
                    if (this.list.pagination != undefined){                        
                        this.pages = this.list.pagination.pages;
                        this.isPagination = false;
                    } else {
                        this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color });      
                    }
                });
            },

            revertJO(jobOrderId, jobOrderRevertId, isApproved){
                var confirmMessage = (isApproved? constants.approveRevert : constants.denyRevert);            
                this.$refs.confirm.open(constants.confirm, confirmMessage, { color: constants.modal_color }).then((confirm) => {
                    if (confirm) {
                        this.fullscreenLoading = true;
                        this.$store.dispatch(constants.revertJORevert, {jobOrderId, jobOrderRevertId, isApproved}).then(() => {
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
                                    this.search();
                                });  
                            }
                           
                            else {
                                this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color }).then(() => {
                                    this.search();
                                });
                            }
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

            view(jobOrderId) {
                // Views the details of a job order    
                this.$router.push({ 
                    name: constants.jobOrderDetail, 
                    params: { 
                        id: jobOrderId.toString(), 
                        prevPage: constants.revertJO
                    }
                });
            }
        },
        
        computed: {            
            ...mapGetters({
                list: constants.revertJOList,
                searchModel: constants.revertJOSearchModel,
                errorMessage: constants.revertJOErrorMessage,
                successMessage: constants.revertJOSuccessMessage,
            }),
        }
    }
</script>
