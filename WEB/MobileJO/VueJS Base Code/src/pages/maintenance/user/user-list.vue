<template>
    <v-container class="child-body">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-form>
            <v-layout row>
                <v-card flat>
                    <h2><v-icon>people</v-icon> User List</h2>
                    <v-spacer></v-spacer>
                </v-card>
            </v-layout>
            <v-layout row>
                <v-divider></v-divider>
            </v-layout>
            <v-spacer></v-spacer>
            <v-layout row>
                <v-flex xs12 m10>
                    <p><b>Search Filters</b></p>
                </v-flex>
            </v-layout>
            <v-card class="search-filter-vcard">
                <v-layout row>
                    <v-flex md6>
                        <v-layout>
                            <v-flex md3 class="input-label text-xs-right">
                                <span>Username</span>
                            </v-flex>
                            <v-flex md6>
                                <v-text-field
                                    prepend-inner-icon="search"
                                    v-model="searchValues.UserName"
                                    single-line
                                    solo
                                    color="red"
                                    hide-details
                                    @keyup.enter="search"
                                ></v-text-field>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex md9>
                        <v-layout>
                            <v-flex md2 class="input-label text-xs-right">
                                <span>Role</span>
                            </v-flex>

                            <v-flex md4>
                                <v-select
                                    v-model="selectRole"
                                    :items="roleItems"
                                    solo
                                    color="black"
                                    hide-details
                                    @keyup.enter="search"
                                ></v-select>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>

                <v-layout row>
                    <v-flex md12>
                        <v-layout>
                            <v-flex md2 class="input-label text-xs-right">
                                <span>Company</span>
                            </v-flex>
                            <v-flex md4>
                                <v-combobox
                                    v-model="selectCompany"
                                    :items="companyItems"
                                    single-line
                                    solo
                                    color="red"
                                    hide-details
                                    @keyup.enter="search"
                                ></v-combobox>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                    <v-flex md4>
                        <v-layout>
                            <v-flex md11 class=" text-xs-right">
                                <v-btn class="btn_secondary" @click="search">
                                    <v-icon>search</v-icon>
                                    Search
                                </v-btn>
                                <v-btn class="btn_secondary" @click="clear">
                                    <v-icon>clear</v-icon>
                                    Clear
                                </v-btn>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>
            </v-card>
        </v-form>

        <v-layout row>
            <v-flex xs12 m10>
                <v-btn class="btn_primary" @click="newUser">
                    <v-icon>add</v-icon>
                    New User
                </v-btn>
            </v-flex>
        </v-layout>

        <v-layout row class="table-spacer" v-if="list.data !== undefined">
            <v-flex xs12 m10>
                <v-data-table
                    :headers="headers"
                    :no-data-text="defaultTableText"
                    :items="list.data.data"
                    hide-actions
                    :key="table"
                >
                    <template v-slot:items="props">
                        <td>
                            <span class="hidden">{{ props.item.id }}</span>
                            {{ props.item.user_name }}
                        </td>
                        <td class="text-xs-left">
                            {{ props.item.first_name }}
                            {{ props.item.last_name }}
                        </td>
                        <td class="text-xs-left">
                            {{ props.item.company_name }}
                        </td>
                        <td class="text-xs-left">
                            <span v-if="props.item.role_id == admin"
                                >Administrator</span
                            >
                            <span v-else>User</span>
                        </td>

                        <td class="text-xs-center">
                            <span>
                                <v-icon
                                    small
                                    class="mr-2 action-ic view-ic"
                                    @click="viewItem(props.item.id)"
                                >
                                    remove_red_eye
                                </v-icon>
                            </span>
                            <span>
                                <v-icon
                                    small
                                    class="mr-2 action-ic edit-ic"
                                    @click="editItem(props.item.id)"
                                >
                                    edit
                                </v-icon>
                            </span>
                            <span>
                                <v-icon
                                    small
                                    class="mr-2 action-ic delete-ic"
                                    @click="deleteItem(props.item.id)"
                                >
                                    delete
                                </v-icon>
                            </span>
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
        <v-layout row="" class="table-spacer" v-else>
            <v-flex xs12="" m10="">
                <v-data-table
                    :headers="headers"
                    :no-data-text="defaultTableText"
                    :items="users"
                    hide-actions=""
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
            admin: 1,
            userName: "",
            selectRole: constants.all,
            selectCompany: constants.all,
            roleItems: [],
            companyItems: [],
            pages: 0,
            newPage: 1,
            isPagination: false,
            fullscreenLoading: false,
            status: true,
            defaultTableText: constants.noRecords,
            headers: [
                {
                    text: "Username",
                    align: "center",
                    sortable: false,
                    value: "user_name",
                    width: "10%"
                },
                {
                    text: "Name",
                    align: "center",
                    value: "first_name",
                    sortable: false,
                    width: "40%"
                },
                {
                    text: "Company",
                    align: "center",
                    value: "company",
                    sortable: false,
                    width: "25%"
                },
                {
                    text: "Role",
                    align: "center",
                    value: "role",
                    sortable: false,
                    width: "10%"
                },
                {
                    text: "Actions",
                    align: "center",
                    value: "actions",
                    sortable: false,
                    width: "15%"
                }
            ],
            users: [],
            table: 0,
            editedIndex: -1,
            editedItem: {
                id: 0
            },
            searchValues: {
                UserName: "",
                RoleID: "",
                CompanyID: "",
                Page: 1,
                PageSize: 10
            }
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
        this.fullscreenLoading = true;
        this.$store.dispatch(constants.getUserRoles).then(() => {
            setTimeout(() => {
                if (
                    this.$store.getters[constants.userRoles] ===
                    constants.noInternet
                ) {
                    this.$refs.info
                        .open(constants.warning, constants.noInternet, {
                            color: constants.error_color
                        })
                        .then(() => {
                            this.$router.push({ path: constants.userList });
                        });
                } else { 
                    var roles = this.$store.getters[constants.userRoles];
                    this.roleItems.push(constants.all);
                    for (var index = 0; index < roles.length; ++index) {
                        this.roleItems.push(roles[index]);
                    }
                }
                this.fullscreenLoading = false;
            }, 1000);
        });

        this.$store.dispatch(constants.getCompanies).then(() => {
            setTimeout(() => {
                if (
                    this.$store.getters[constants.companies] ===
                    constants.noInternet
                ) {
                    this.$refs.info
                        .open(constants.warning, constants.noInternet, {
                            color: constants.error_color
                        })
                        .then(() => {
                            this.$router.push({ path: constants.userList });
                        });
                } else {
                    var companies = this.$store.getters[constants.companies];
                    this.companyItems.push(constants.all);
                    for (var index = 0; index < companies.length; ++index) {
                        this.companyItems.push(companies[index]);
                    }
                }
                this.fullscreenLoading = false;
            }, 1000);
        });

        this.search();
    },

    methods: {
        onPageChange(page) {
            this.searchValues.UserName = this.userName;
            this.searchValues.RoleID = this.selectRole;
            this.searchValues.CompanyID = this.selectCompany;
            this.newPage = page;
            this.isPagination = true;
            this.search();
        },

        search() {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.fullscreenLoading = true;
                this.userName = this.searchValues.UserName;

                this.searchValues.Page = this.isPagination ? this.newPage : 1;

                if (this.selectRole == constants.all) {
                    this.searchValues.RoleID = "all";
                } else {
                    this.searchValues.RoleID = this.selectRole;
                }

                if (this.selectCompany == constants.all) {
                    this.searchValues.CompanyID = "all";
                } else {
                    this.searchValues.CompanyID = this.selectCompany.value;
                }

                this.$store
                    .dispatch(constants.searchUser, this.searchValues)
                    .then(() => {
                        setTimeout(() => {
                            this.fullscreenLoading = false;
                            if (this.list.data) {
                                this.pages = this.list.data.pagination.pages;
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

        clear() {
            this.resetFilters();
            this.search();
        },

        editItem(id) {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ path: constants.userEdit + id });
            }
        },

        viewItem(id) {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ path: constants.userView + id });
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
                                constants.listUsers
                            ].data;
                            this.$store
                                .dispatch(constants.destroyUser, id)
                                .then(() => {
                                    this.fullscreenLoading = false;
                                    if (
                                        !this.$store.getters[
                                            constants.listUsers
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
                                            constants.listUsers
                                        ].message;
                                        this.$store.getters[
                                            constants.listUsers
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
                                            message.ModelStateErrors !==
                                            undefined
                                        ) {
                                            for (
                                                let i = 0;
                                                i <
                                                message.ModelStateErrors.length;
                                                i++
                                            ) {
                                                var stateError = "";
                                                if (
                                                    message.ModelStateErrors[
                                                        i
                                                    ] ==
                                                    constants.recordNotExist
                                                ) {
                                                    stateError =
                                                        constants.notAvailable;
                                                } else if (
                                                    message.ModelStateErrors[
                                                        i
                                                    ] ===
                                                    constants.recordInvalid
                                                ) {
                                                    stateError =
                                                        message
                                                            .ModelStateErrors[
                                                            i
                                                        ];
                                                } else if (
                                                    message.ModelStateErrors[
                                                        i
                                                    ] ==
                                                    constants.recordHasPendingJOs
                                                ) {
                                                    stateError =
                                                        message
                                                            .ModelStateErrors[
                                                            i
                                                        ];
                                                }
                                                this.$refs.info
                                                    .open(
                                                        constants.warning,
                                                        stateError,
                                                        {
                                                            color:
                                                                constants.modal_error
                                                        }
                                                    )
                                                    .then(() => {
                                                        this.search();
                                                    });
                                            }
                                        } else {
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
                                                            constants.listUsers
                                                        ].data.data.length === 1
                                                    ) {
                                                        this.resetFilters();
                                                    }
                                                    this.search();
                                                });
                                        }
                                    }
                                });
                        }
                    });
            }
        },
        resetFilters() {
            this.searchValues = {
                UserName: "",
                RoleID: "",
                CompanyID: "",
                Page: 1,
                PageSize: 10
            };
            this.selectRole = constants.all;
            this.selectCompany = constants.all;
        },
        newUser() {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                this.$router.push({ path: constants.userForm });
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
            list: constants.listUsers,
            roles: constants.userRoles,
            companies: constants.companies
        })
    }
};
</script>

<style></style>
