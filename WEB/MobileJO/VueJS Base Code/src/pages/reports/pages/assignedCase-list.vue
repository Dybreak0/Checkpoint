<template>
    <v-container class="child-body">
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <v-form>
            <v-layout row>
                <v-card flat class="search-filter-card">
                    <h2><v-icon>description</v-icon> Assigned Cases Report</h2>
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
                                        <span>Case #: </span>
                                    </v-flex>
                                    <v-flex xs7 m7>
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
        <v-layout row>
            <v-flex xs12 md12>
                <v-layout>
                    <v-flex xs12 m12 class="text-xs-right">
                        <v-btn class="btn_primary" @click="sync">
                            <v-icon>loop</v-icon>&nbsp;Sync w/ 3rd Party
                        </v-btn>
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
                        <td class="text-xs-center"><a @click="view(props.item.id)"><u>{{ props.item.case_number }}</u></a></td>
                        <td class="text-xs-left">{{ props.item.case_subject }}</td>
                        <td class="text-xs-left">{{ props.item.account_name }}</td>
                        <td class="text-xs-left">{{ props.item.application_type_name }}</td>
                        <td class="text-xs-left">{{ props.item.status }}</td>
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
    import axios from 'axios';    
    import info from '../../../common/layout/info-modal';
    import confirm from '../../../common/layout/confirm-modal';
    import loading from '../../../common/layout/progress';
    import pagination from '../../../common/components/pagination';
    import constants from '../../../common/utils/constants';

    export default {
        name: constants.assignedCaseReportPage,
        data() {
            return {
                pages: 0,
                fullscreenLoading: false,
                exportDisabled: true,
                newPage: 1,
                isPagination: false,    
                defaultTableText: constants.noRecords,
                currentApplicationType: 0,
                currentCaseNumber: '',
                headers: [
                    { text: constants.caseNumberHeader, align: constants.defaultTableAlign, sortable: false, width: "5%" },
                    { text: constants.caseSubjectHeader, align: constants.defaultTableAlign, sortable: false, width: "30%" },
                    { text: constants.accountNameHeader, align: constants.defaultTableAlign, sortable: false, width: "15%"},
                    { text: constants.applicationTypeHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"},
                    { text: constants.statusHeader, align: constants.defaultTableAlign, sortable: false, width: "10%"}
                ],
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
                this.$store.dispatch(constants.assignedCaseReportNew);           // Initialize all properties of the search model
                this.$store.dispatch(constants.assignedCaseReportLoadOptions);   // Initialize and load all dropdown options
            },
            onPageChange(page) {
                this.newPage = page;
                this.isPagination = true;
                this.search();
            },
            clearFilters() {
                this.$store.dispatch(constants.assignedCaseReportClearFilters);     // Resets the search filters to the default values
                this.search();
            },
            search() {
                this.fullscreenLoading = true;
                this.searchModel.page = (this.isPagination) ? this.newPage : 1;                   
                this.$store.dispatch(constants.assignedCaseReportList).then(() => {     // Searches for job order records using the search filters 
                    this.fullscreenLoading = false;                    
                    if (this.errorMessage == null){                        
                        this.pages = this.list.pagination.pages;
                        this.exportDisabled = (this.list.data.length > 0) ? false : true;
                        this.isPagination = false;
                        this.currentApplicationType = this.searchModel.application_type;
                        this.currentCaseNumber = this.searchModel.case_number;
                    } else {
                        this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color });      
                    }
                });   
            },
            download() {
                this.fullscreenLoading = true;
                this.searchModel.case_number = this.currentCaseNumber;
                this.searchModel.application_type = this.currentApplicationType;
                this.$store.dispatch(constants.assignedCaseReportDownload).then(() => {     // Retrieves the job order records and puts it into an excel file
                    this.fullscreenLoading = false;
		});        
            },
            sync() {
                this.fullscreenLoading = true;
                this.$store.dispatch(constants.assignedCaseSyncData).then(() => {     // Retrieves all the data from TFS Server
                    this.fullscreenLoading = false;
                    this.search();
                    if (this.errorMessage != null) {
                        this.$refs.info.open(constants.warning, this.errorMessage, { color: constants.error_color });
                    } 
                });        
            },
            view(id) {
                // Views the details of an assigned case   
                this.$router.push({ 
                    name: constants.assignedCaseDetail, 
                    params: { 
                        id: id.toString(),
                    }
                });                
            }
        },
        computed: {            
            ...mapGetters({
                list: constants.assignedCaseReportList,
                options: constants.assignedCaseReportOptions,
                searchModel: constants.assignedCaseReportSearchModel,
                errorMessage: constants.assignedCaseReportErrorMessage,
            }),
        }
    }
</script>
