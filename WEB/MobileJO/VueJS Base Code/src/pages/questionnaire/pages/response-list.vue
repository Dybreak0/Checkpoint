<template>
    <v-container class="child-body">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-layout row>
            <v-card flat>
                <h2><v-icon>description</v-icon> Response List</h2>
                <v-spacer></v-spacer>
            </v-card>
        </v-layout>
        <v-layout row>
            <v-divider></v-divider>
        </v-layout>
        <v-spacer></v-spacer>
        <v-layout row>
            <v-flex xs12 m10>
                <p><b>Search Filter</b></p>
            </v-flex>
        </v-layout>
        <v-layout row>
            <v-flex xs12 m10>
                <v-card class="search-filter-vcard" flat tile>
                    <v-layout row>
                        <v-flex xs5 md5>
                            <v-layout>
                                <v-flex
                                    xs4
                                    md4
                                    class="input-label text-xs-right"
                                >
                                    <span>Title: </span>
                                </v-flex>
                                <v-flex xs7 m7>
                                    <v-text-field
                                        v-model="searchValues.Title"
                                        prepend-inner-icon="search"
                                        single-line
                                        solo
                                        hide-details
                                        @keyup.enter="search"
                                        color="red"
                                    >
                                    </v-text-field>
                                </v-flex>
                            </v-layout>
                        </v-flex>
                        <v-flex xs5 md5>
                            <v-layout>
                                <v-flex
                                    xs4
                                    md4
                                    class="input-label text-xs-right"
                                >
                                    <span>Category: </span>
                                </v-flex>
                                <v-flex xs7 m7>
                                    <v-text-field
                                        v-model="searchValues.Category"
                                        prepend-inner-icon="search"
                                        single-line
                                        solo
                                        hide-details
                                        @keyup.enter="search"
                                        color="red"
                                    >
                                    </v-text-field>
                                </v-flex>
                            </v-layout>
                        </v-flex>
                        <v-flex xs12 md2>
                            <v-layout>
                                <v-flex md12 class="text-xs-right">
                                    <v-btn class="btn_secondary" @click="search"
                                        ><v-icon>search</v-icon>Search</v-btn
                                    >
                                    <v-btn
                                        class="btn_secondary buttonClearAdjust"
                                        @click="clear"
                                        ><v-icon>clear</v-icon>Clear</v-btn
                                    >
                                </v-flex>
                            </v-layout>
                        </v-flex>
                    </v-layout>
                    <v-layout row>
                        <v-flex xs5 md5>
                            <v-layout>
                                <v-flex
                                    xs4
                                    md4
                                    class="input-label text-xs-right"
                                >
                                    <span>Start Date: </span>
                                </v-flex>
                                <v-flex xs7 m7>
                                    <v-menu
                                        v-model="startDateResponse"
                                        transition="scale-transition"
                                        :close-on-content-click="false"
                                        lazy
                                        offset-y
                                        full-width
                                    >
                                        <template v-slot:activator="{ on }">
                                            <v-text-field
                                                v-model="searchValues.StartDate"
                                                v-on="on"
                                                prepend-icon="event"
                                                readonly
                                                color="red"
                                            >
                                            </v-text-field>
                                        </template>
                                        <v-date-picker
                                            class="customTable"
                                            v-model="searchValues.StartDate"
                                            color="red"
                                            width="395"
                                            @input="startDateResponse = false"
                                        >
                                        </v-date-picker>
                                    </v-menu>
                                </v-flex>
                            </v-layout>
                        </v-flex>
                        <v-flex xs5 md5>
                            <v-layout>
                                <v-flex
                                    xs4
                                    md4
                                    class="input-label text-xs-right"
                                >
                                    <span>End Date: </span>
                                </v-flex>
                                <v-flex xs7 m7>
                                    <v-menu
                                        v-model="endDateResponse"
                                        transition="scale-transition"
                                        :close-on-content-click="false"
                                        lazy
                                        offset-y
                                        full-width
                                    >
                                        <template v-slot:activator="{ on }">
                                            <v-text-field
                                                v-model="searchValues.EndDate"
                                                v-on="on"
                                                prepend-icon="event"
                                                readonly
                                                color="red"
                                            >
                                            </v-text-field>
                                        </template>
                                        <v-date-picker
                                            class="customTable"
                                            v-model="searchValues.EndDate"
                                            color="red"
                                            width="395"
                                            :min="searchValues.StartDate"
                                            @input="endDateResponse = false"
                                        >
                                        </v-date-picker>
                                    </v-menu>
                                </v-flex>
                            </v-layout>
                        </v-flex>
                    </v-layout>
                </v-card>
            </v-flex>
        </v-layout>
        <v-layout row v-if="dateFilterNotValid">
            <v-flex xs6 md6></v-flex>
            <v-flex xs5 md5>
                <v-flex xs12 md12 class="input-label">
                    <span class="text-warning"
                        >Date filter values are invalid. End Date should be
                        greater than Start Date.
                    </span>
                </v-flex>
            </v-flex>
        </v-layout>
        <v-layout row>
            <v-flex xs12 md12>
                <v-layout>
                    <v-flex xs12 m12 class="text-xs-right">
                        <v-btn
                            class="btn_primary"
                            @click="downloadPDF"
                            :disabled="exportDisabled"
                        >
                            <v-icon>vertical_align_bottom</v-icon>&nbsp;Export
                            to PDF
                        </v-btn>
                        <v-btn
                            class="btn_primary"
                            @click="downloadExcel"
                            :disabled="exportDisabled"
                        >
                            <v-icon>vertical_align_bottom</v-icon>&nbsp;Export
                            to Excel
                        </v-btn>
                    </v-flex>
                </v-layout>
            </v-flex>
        </v-layout>

        <v-layout row class="table-spacer" v-if="list.data !== undefined">
            <v-flex xs12 m10>
                <v-data-table
                    :headers="headers"
                    :no-data-text="defaultTableText"
                    :items="list.data.data"
                    hide-actions
                    :accesskey="table"
                >
                    <template v-slot:items="props">
                        <td class="table_id no-break-word">
                            <span class="hidden">{{ props.item.id }}</span>
                            {{ props.item.title }}
                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.company_name }} -
                            {{ props.item.branch }}
                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.category }}
                        </td>
                        <td class="text-xs-center no-break-word">
                            {{ props.item.date_submitted }}
                        </td>
                        <td class="text-xs-center no-break-word">
                            {{ props.item.submitted_by }}
                        </td>
                        <td class="text-xs-center no-break-word">
                            <span v-if="props.item.isApproved">Approved</span>
                            <span v-else>Submitted</span>
                        </td>
                        <td class="text-xs-center">
                            <v-icon
                                small
                                class="mr-2 action-ic view-ic"
                                @click="viewItem(props.item.id)"
                            >
                                remove_red_eye
                            </v-icon>
                        </td>
                    </template>
                </v-data-table>
                <div class="text-xs-center pt-2">
                    <pagination
                        v-if="pages > 1"
                        :maxVisibleButtons="5"
                        :total-pages="pages"
                        :current-page="searchValues.Page"
                        @pagechanged="onPageChange"
                    />
                </div>
            </v-flex>
        </v-layout>
        <v-layout row class="table-spacer" v-else="">
            <v-flex xs12="" m10="">
                <v-data-table
                    :headers="headers"
                    :no-data-text="defaultTableText"
                    :items="responses"
                    hide-actions=""
                    :key="table"
                >
                </v-data-table>
            </v-flex>
        </v-layout>
    </v-container>
</template>

<script>
import { mapGetters } from "vuex";
import offline from "v-offline";
import confirm from "../../../common/layout/confirm-modal";
import info from "../../../common/layout/info-modal";
import loading from "../../../common/layout/progress";
import constants from "../../../common/utils/constants";
import pagination from "../../../common/components/pagination";

export default {
    data() {
        return {
            fullscreenLoading: false,
            status: true,
            pages: 0,
            listLength: 0,
            endDateResponse: false,
            startDateResponse: false,
            responses: [],
            defaultTableText: constants.noRecords,
            headers: [
                {
                    text: "Title",
                    align: "center",
                    sortable: false,
                    value: "title",
                    width: "20%"
                },
                {
                    text: "Company - Branch",
                    align: "center",
                    value: "company",
                    sortable: false,
                    width: "15%"
                },
                {
                    text: "Category",
                    align: "center",
                    value: "category",
                    sortable: false,
                    width: "15%"
                },
                {
                    text: "Date Submitted",
                    align: "center",
                    value: "date",
                    sortable: false,
                    width: "15%"
                },
                {
                    text: "Submitted By",
                    align: "center",
                    value: "submitted",
                    sortable: false,
                    width: "15%"
                },
                {
                    text: "Status",
                    align: "center",
                    value: "status",
                    sortable: false,
                    width: "10%"
                },
                {
                    text: "Actions",
                    align: "center",
                    value: "actions",
                    sortable: false,
                    width: "10%"
                }
            ],
            table: 0,
            editedIndex: -1,
            editedItem: {
                id: 0
            },
            searchValues: {
                Title: "",
                Category: "",
                StartDate: new Date().toISOString().slice(0, 10),
                EndDate: new Date().toISOString().slice(0, 10),
                Page: 1,
                PageSize: 10
            },
            title: "",
            category: "",
            startDate: "",
            endDate: "",
            newPage: 1,
            isPagination: false
        };
    },

    components: {
        confirm,
        info,
        loading,
        offline,
        pagination
    },

    created() {
        this.search();
    },

    methods: {
        onPageChange(page) {
            this.searchValues.Title = this.title;
            this.searchValues.Category = this.category;
            this.searchValues.StartDate = this.startDate;
            this.searchValues.EndDate = this.endDate;
            this.newPage = page;
            this.isPagination = true;
            this.search();
        },

        search() {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.fullscreenLoading = true;
                this.title = this.searchValues.Title;
                this.category = this.searchValues.Category;
                this.startDate = this.searchValues.StartDate;
                this.endDate = this.searchValues.EndDate;

                this.searchValues.Page = this.isPagination ? this.newPage : 1;
                this.$store
                    .dispatch(constants.searchResponse, this.searchValues)
                    .then(() => {
                        setTimeout(() => {
                            this.fullscreenLoading = false;
                            if (this.list.data) {
                                this.pages = this.list.data.pagination.pages;
                                this.listLength = this.list.data.pagination.size;
                                this.isPagination = false;
                            } else {
                                this.$refs.info
                                    .open(
                                        constants.warning,
                                        constants.noInternet,
                                        { color: constants.error_color }
                                    )
                                    .then(() => {});
                            }
                        }, 1000);
                    });
                this.table++;
            }
        },

        downloadExcel() {
            this.fullscreenLoading = true;
            this.searchValues.Title = this.title;
            this.searchValues.Category = this.category;
            this.searchValues.StartDate = this.startDate;
            this.searchValues.EndDate = this.endDate;

            this.$store
                .dispatch(constants.responseDownloadExcel, this.searchValues)
                .then(() => {
                    this.fullscreenLoading = false;
                });
        },

        downloadPDF() {
            this.fullscreenLoading = true;
            this.$store
                .dispatch(constants.responseDownloadPDF, this.searchValues)
                .then(() => {
                    this.fullscreenLoading = false;
                });
        },

        clear() {
            this.resetFilters();
            this.search();
        },

        viewItem(id) {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ path: constants.responseView + id });
            }
        },

        resetFilters() {
            this.searchValues = {
                Title: "",
                Category: "",
                StartDate: "",
                EndDate: "",
                Page: 1,
                PageSize: 10
            };
        },

        clearStore() {
            this.$store.dispatch(constants.clearLogin);
            this.$store.dispatch(constants.clearUsers);
            this.$store.dispatch(constants.clearAccounts);
            this.$store.dispatch(constants.clearEmails);
            this.$store.dispatch(constants.clearJobOrders);
            this.$store.dispatch(constants.clearCases);
            this.$store.dispatch(constants.clearRating);
            this.$store.dispatch(constants.clearResponse);
            this.$router.push("/login");
        },

        handleConnectivityChange(status) {
            if (status === false) {
                this.status = false;
                this.$refs.info
                    .open(constants.message, constants.noInternet, {
                        color: constants.error_color
                    })
                    .then(() => {});
            } else {
                this.status = true;
            }
        }
    },
    computed: {
        ...mapGetters({
            list: constants.responseLists
        }),
        dateFilterNotValid() {
            return this.searchValues.EndDate < this.searchValues.StartDate
                ? true
                : false;
        },
        exportDisabled() {
            return this.listLength > 0 && !this.dateFilterNotValid
                ? false
                : true;
        }
    }
};
</script>

<style></style>
