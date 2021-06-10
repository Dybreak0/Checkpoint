<template>
    <v-container class="child-body">
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <v-form>
            <v-layout row>
                <v-card flat class="search-filter-card">
                    <h2><v-icon>description</v-icon> JO Client Rating Report</h2>
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
                                            @keyup.enter="search"
                                            color="red">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                            <v-flex xs5 md5>
                                <v-layout>
                                    <v-flex xs4 md4 class="input-label text-xs-right">
                                        <span>Application Type: </span>
                                    </v-flex>
                                    <v-flex xs7 md7>
                                        <v-select 
                                            v-model="searchModel.application_type"
                                            :items="options.application_type"
                                            solo
                                            hide-details
                                            color="black">
                                        </v-select>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs5 md5>
                                <v-layout>
                                    <v-flex xs4 md4 class="input-label text-xs-right">
                                        <span>Case #: </span>
                                    </v-flex>
                                    <v-flex xs7 md7>
                                        <v-text-field 
                                            v-model="searchModel.case_number"
                                            prepend-inner-icon="search"                                                      
                                            single-line
                                            solo
                                            hide-details
                                            @keyup.enter="search"
                                            color="red">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>                            
                            <v-flex xs5 md5>
                                <v-layout>
                                    <v-flex xs4 md4 class="input-label text-xs-right">
                                        <span>Account Name: </span>
                                    </v-flex>
                                    <v-flex xs7 md7>
                                        <v-text-field 
                                            v-model="searchModel.account_name"
                                            prepend-inner-icon="search"                                                      
                                            single-line
                                            solo
                                            hide-details
                                            @keyup.enter="search"
                                            color="red">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
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
                                            @keyup.enter="search"
                                            color="red">
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                            <v-flex xs5 md5>
                                <v-layout>
                                    <v-flex xs4 md4 class="input-label text-xs-right">
                                        <span>JO Date From: </span>
                                    </v-flex>
                                    <v-flex xs2 md2>
                                        <v-menu 
                                            v-model="dateFromMenu"
                                            transition="scale-transition"
                                            :close-on-content-click="false"                                            
                                            lazy                                            
                                            offset-y
                                            full-width>
                                            <template v-slot:activator="{ on }">
                                                <v-text-field 
                                                    v-model="searchModel.job_order_date_from"
                                                    v-on="on"
                                                    prepend-icon="event"
                                                    readonly
                                                    color="red">
                                                </v-text-field>
                                            </template>
                                            <v-date-picker 
                                                class="customTable"
                                                v-model="searchModel.job_order_date_from" 
                                                color="red"
                                                @input="dateFromMenu = false">
                                            </v-date-picker>
                                        </v-menu>
                                    </v-flex>
                                    <v-flex xs1 md1></v-flex>
                                    <v-flex xs2 md2 class="input-label text-xs-right">
                                        <span>JO Date To: </span>
                                    </v-flex>
                                    <v-flex xs2 md2>
                                        <v-menu
                                            v-model="dateToMenu"
                                            transition="scale-transition"
                                            :close-on-content-click="false"
                                            lazy                                            
                                            offset-y
                                            full-width>
                                            <template v-slot:activator="{ on }">
                                                <v-text-field
                                                    v-model="searchModel.job_order_date_to"
                                                    v-on="on"
                                                    prepend-icon="event"
                                                    readonly
                                                    color="red">
                                                </v-text-field>
                                            </template>
                                            <v-date-picker 
                                                class="customTable"
                                                v-model="searchModel.job_order_date_to" 
                                                color="red"                                                                                                 
                                                :min="searchModel.job_order_date_from"
                                                @input="dateToMenu = false" >
                                            </v-date-picker>
                                        </v-menu>
                                    </v-flex>
                                </v-layout>
                            </v-flex>                            
                        </v-layout>
                        <v-layout row v-if="dateFilterNotValid">
                            <v-flex xs6 md6></v-flex>
                            <v-flex xs5 md5>                                
                                <v-flex xs12 md12 class="input-label">
                                    <span class="text-warning">Date filter values are invalid. JO Date To should be greater than JO Date From. </span>
                                </v-flex>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs12 md12>
                                <v-layout>
                                    <v-flex md12 class="text-xs-right">
                                        <v-btn class="btn_secondary" @click="search" :disabled="dateFilterNotValid"><v-icon>search</v-icon>Search</v-btn>
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
        <v-layout row>
            <v-flex xs12 md12>
                <v-layout>
                    <v-flex xs12 m12 class="text-xs-right">
                        <v-btn class="btn_primary" @click="download" :disabled="exportDisabled">
                            <v-icon>vertical_align_bottom</v-icon>&nbsp;Export to Excel
                        </v-btn>
                    </v-flex>
                </v-layout>
            </v-flex>
        </v-layout>
        <v-layout row class="table-spacer">
            <v-flex xs12 m10>
                <v-data-table 
                    :headers="headers"
                    :items="list.data"
                    :no-data-text="defaultTableText"
                    hide-actions>
                    <template v-slot:items="props">
                        <td class="text-xs-center"><a @click="view(props.item.id)"><u>{{ props.item.job_order_number }}</u></a></td>
                        <td class="text-xs-center">{{ props.item.case_number }}</td>
                        <td class="text-xs-left">{{ props.item.application_type_name }}</td>
                        <td class="text-xs-center">{{ props.item.job_order_datetime_start | formatDate}}</td>
                        <td class="text-xs-center">{{ props.item.job_order_datetime_end | formatDate}}</td>
                        <td class="text-xs-left">{{ props.item.reported_by_name }}</td>
                        <td class="text-xs-left">{{ props.item.account_name }}</td>
                        <td class="text-xs-right">{{ props.item.client_rating }}</td>
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
    import { mapGetters } from 'vuex';
    import axios from 'axios'    
    import info from '../../../common/layout/info-modal';
    import confirm from '../../../common/layout/confirm-modal';
    import loading from '../../../common/layout/progress';
    import pagination from '../../../common/components/pagination';
    import constants from '../../../common/utils/constants';

    export default {
        name: constants.jobOrderClientRatingReportPage,
        data() {
            return {
                pages: 0,                
                dateFromMenu: false,
                dateToMenu: false,
                fullscreenLoading: false,
                newPage: 1,
                isPagination: false,     
                listLength: 0,
                defaultTableText: constants.noRecords,
                headers: [
                    { text: constants.jobOrderNumberHeader, align: constants.defaultTableAlign, sortable: false, width: "10%" },
                    { text: constants.caseNumberHeader, align: constants.defaultTableAlign, sortable: false, width: "10%" },
                    { text: constants.applicationTypeHeader, align: constants.defaultTableAlign, sortable: false, width: "15%"},
                    { text: constants.jobOrderDateStartHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"},
                    { text: constants.jobOrderDateEndHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"},
                    { text: constants.reportedByHeader, align: constants.defaultTableAlign, sortable: false, width: "20%"},
                    { text: constants.accountNameHeader, align: constants.defaultTableAlign, sortable: false, width: "20%" },
                    { text: constants.clientRatingHeader, align: constants.defaultTableAlign, sortable: false, width: "5%"}
                ],
                currentCaseNumber: "",
                currentJobOrderNumber: "",
                currentApplicationType: 0,
                currentAccount: "",
                currentReportedBy: "",
                currentJobOrderDateFrom: "",
                currentJobOrderDateTo: "",
            }
        },
        created() {
            this.initialize();
            this.search();
        },
        components: {
            info,
            loading,
            confirm,                        
            pagination,
            constants
        },
        methods: {
            initialize() {
                this.$store.dispatch(constants.jobOrderClientRatingReportNew);          // Initialize all properties of the search model
                this.$store.dispatch(constants.jobOrderClientRatingReportLoadOptions);  // Initialize and load all dropdown options
            },
            onPageChange(page) {
                this.newPage = page;
                this.isPagination = true;
                this.search();
            },
            clearFilters() {
                this.$store.dispatch(constants.jobOrderClientRatingReportClearFilters);     // Resets the search filters to the default values
                this.search();
            },
            search() {
                this.fullscreenLoading = true;
                this.searchModel.page = (this.isPagination) ? this.newPage : 1;
                this.$store.dispatch(constants.jobOrderClientRatingReportList).then(() => {     // Searches for job order client rating records using the search filters 
                    this.fullscreenLoading = false;                    
                    if (this.errorMessage == null){                        
                        this.pages = this.list.pagination.pages;
                        this.listLength = this.list.data.length;
                        this.isPagination = false;
                        this.currentCaseNumber = this.searchModel.case_number;
                        this.currentJobOrderNumber = this.searchModel.job_order_number;
                        this.currentApplicationType = this.searchModel.application_type;
                        this.currentAccount = this.searchModel.account_name;
                        this.currentReportedBy = this.searchModel.reported_by;
                        this.currentJobOrderDateFrom = this.searchModel.job_order_date_from;
                        this.currentJobOrderDateTo = this.searchModel.job_order_date_to;
                    } else {
                        this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color });
                    }
                });
            },
            download() {
                this.fullscreenLoading = true;
                this.searchModel.case_number = this.currentCaseNumber;
                this.searchModel.job_order_number = this.currentJobOrderNumber;
                this.searchModel.application_type = this.currentApplicationType;
                this.searchModel.account_name = this.currentAccount;
                this.searchModel.reported_by = this.currentReportedBy;
                this.searchModel.job_order_date_from = this.currentJobOrderDateFrom;
                this.searchModel.job_order_date_to = this.currentJobOrderDateTo;
                this.$store.dispatch(constants.jobOrderClientRatingReportDownload).then(() => {     // Retrieves the job order client rating records and puts it into an excel file
                    this.fullscreenLoading = false;           
                });        
            },
            view(id) {
                // Views the details of a job order client rating
                this.$router.push({ 
                    name: constants.jobOrderDetail, 
                    params: { 
                        id: id.toString(), 
                        prevPage: constants.jobOrderClientRatingReport 
                    }
                });
            },
        },
        computed: {            
            ...mapGetters({
                list: constants.jobOrderClientRatingReportList,
                options: constants.jobOrderClientRatingReportOptions,
                searchModel: constants.jobOrderClientRatingReportSearchModel,
                errorMessage: constants.jobOrderClientRatingReportErrorMessage,
            }),
            dateFilterNotValid() {
                return (this.searchModel.job_order_date_to < this.searchModel.job_order_date_from)? true : false;
            },            
            exportDisabled() {
                return (this.listLength > 0 && !this.dateFilterNotValid) ? false : true;
            }
        }
    }
</script>
