<template>
    <v-container class="child-body">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-layout row>
            <v-card flat>
                <h2><v-icon>description</v-icon> Template List</h2>
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
                </v-card>
            </v-flex>
        </v-layout>

        <v-layout row>
            <v-flex xs12 m10>
                <v-btn class="btn_primary" @click="newTemplate">
                    <v-icon>add</v-icon>
                    New Template
                </v-btn>
            </v-flex>
        </v-layout>
        <v-layout row class="table-spacer" v-if="localList.data !== undefined">
            <v-flex xs12 m10>
                <v-data-table
                    :headers="headers"
                    :no-data-text="defaultTableText"
                    :items="localList.data.data"
                    hide-actions
                    :accesskey="table"
                >
                    <template v-slot:items="props">
                        <td class="table_id no-break-word">
                            <span class="hidden">{{ props.item.id }}</span>
                            {{ props.item.title }}
                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.description }}
                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.respondents }}
                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.category }}
                        </td>
                        <td class="text-xs-center">
                            <v-icon
                                small
                                class="mr-2 action-ic view-ic"
                                @click="viewItem(props.item.id)"
                            >
                                remove_red_eye
                            </v-icon>
                            <v-icon
                                small
                                class="mr-2 action-ic edit-ic"
                                @click="editItem(props.item.id)"
                            >
                                edit
                            </v-icon>
                            <v-icon
                                small
                                class="mr-2 action-ic delete-ic"
                                @click="deleteItem(props.item.id)"
                            >
                                delete
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
        <v-layout row class="table-spacer" v-else>
            <v-flex xs12="" m10="">
                <v-data-table
                    :headers="headers"
                    :no-data-text="defaultTableText"
                    :items="accounts"
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
            defaultTableText: constants.noRecords,
            headers: [
                {
                    text: "Title",
                    align: "center",
                    sortable: false,
                    value: "name",
                    width: "15%"
                },
                {
                    text: "Description",
                    align: "center",
                    value: "calories",
                    sortable: false,
                    width: "30%"
                },
                {
                    text: "Respondents",
                    align: "center",
                    value: "contact_person",
                    sortable: false,
                    width: "20%"
                },
                {
                    text: "Category",
                    align: "center",
                    value: "contact_number",
                    sortable: false,
                    width: "15%"
                },
                {
                    text: "Actions",
                    align: "center",
                    value: "name",
                    sortable: false,
                    width: "20%"
                }
            ],
            accounts: [],
            table: 0,
            editedIndex: -1,
            editedItem: {
                id: 0
            },
            searchValues: {
                Title: "",
                Category: "",
                Page: 1,
                PageSize: 10
            },
            name: "",
            newPage: 1,
            isPagination: false,
            localList: {}
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
            this.searchValues.Name = this.name;
            this.newPage = page;
            this.isPagination = true;
            this.search();
        },

        search() {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.fullscreenLoading = true;
                this.searchValues.Page = this.isPagination ? this.newPage : 1;
                this.$store
                    .dispatch(constants.searchTemplate, this.searchValues)
                    .then(() => {
                        setTimeout(() => {
                            this.fullscreenLoading = false;
                            if (this.list.data) {
                                this.pages = this.list.data.pagination.pages;
                                this.isPagination = false;
                                this.localList = this.list;
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

        clear() {
            this.resetFilters();
            this.search();
        },

        editItem(id) {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ path: constants.templateEdit + id });
            }
        },

        viewItem(id) {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ path: constants.templateView + id });
            }
        },

        deleteItem(id) {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$refs.confirm
                    .open(constants.confirm, constants.deleteConfirm, {
                        color: constants.modal_color
                    })
                    .then(confirm => {
                        if (confirm) {
                            this.fullscreenLoading = true;
                            var oldData = this.$store.getters[
                                constants.listTemplate
                            ].data;
                            this.$store
                                .dispatch(constants.deleteTemplate, id)
                                .then(() => {
                                    this.fullscreenLoading = false;
                                    if (
                                        !this.$store.getters[
                                            constants.listTemplate
                                        ].message
                                    ) {
                                        this.$refs.info
                                            .open(
                                                constants.warning,
                                                constants.noInternet,
                                                { color: constants.error_color }
                                            )
                                            .then(() => {});
                                    } else {
                                        var message = this.$store.getters[
                                            constants.listTemplate
                                        ].message;
                                        this.$store.getters[
                                            constants.listTemplate
                                        ].data = oldData;
                                        if (message === constants.deletedUser) {
                                            this.$refs.info
                                                .open(
                                                    constants.warning,
                                                    message,
                                                    {
                                                        color:
                                                            constants.modal_error
                                                    }
                                                )
                                                .then(() => {
                                                    this.clearStore();
                                                });
                                        } else if (
                                            message === constants.notAdmin
                                        ) {
                                            this.$refs.info
                                                .open(
                                                    constants.warning,
                                                    message,
                                                    {
                                                        color:
                                                            constants.modal_error
                                                    }
                                                )
                                                .then(() => {
                                                    this.clearStore();
                                                });
                                        } else if (
                                            message === constants.recordNotExist
                                        ) {
                                            this.$refs.info
                                                .open(
                                                    constants.warning,
                                                    message,
                                                    {
                                                        color:
                                                            constants.modal_error
                                                    }
                                                )
                                                .then(() => {
                                                    this.search();
                                                });
                                        } else if (
                                            message ===
                                            constants.CannotDeleteActiveTemplate
                                        ) {
                                            this.$refs.info
                                                .open(
                                                    constants.warning,
                                                    message,
                                                    {
                                                        color:
                                                            constants.modal_error
                                                    }
                                                )
                                                .then(() => {});
                                        } else if (
                                            message ===
                                            constants.SuccesfullyDeleted
                                        ) {
                                            this.$refs.info
                                                .open(
                                                    constants.message,
                                                    message,
                                                    {
                                                        color:
                                                            constants.modal_color
                                                    }
                                                )
                                                .then(() => {
                                                    if (
                                                        this.$store.getters[
                                                            constants
                                                                .listTemplate
                                                        ].data.data.length === 1
                                                    ) {
                                                        this.resetFilters();
                                                    }
                                                    this.search();
                                                });
                                        } else {
                                            this.$refs.info
                                                .open(
                                                    constants.warning,
                                                    constants.noInternet,
                                                    {
                                                        color:
                                                            constants.modal_error
                                                    }
                                                )
                                                .then(() => {});
                                        }
                                    }
                                });
                        }
                    });
            }
        },
        resetFilters() {
            this.searchValues = {
                Title: "",
                Category: "",
                Page: 1,
                PageSize: 10
            };
        },
        newTemplate() {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ name: constants.templateForm });
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
            this.$store.dispatch(constants.clearTemplate);
            this.$router.push("/login");
        },

        handleConnectivityChange(status) {
            if (status === false) {
                this.status = false;
                this.localList.data = [];
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
            list: constants.listTemplate
        })
    }
};
</script>

<style></style>
