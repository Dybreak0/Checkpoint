<template>
    <v-container class="child-body" >
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-layout row>
            <v-card flat>
                <h2> <v-icon>people</v-icon> Account List</h2>
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
        <v-card class="search-filter-vcard">
            <v-layout row>
                <v-flex md6>
                    <v-layout>

                        <v-flex md3 class="input-label text-xs-right">
                            <span>Name</span>
                        </v-flex>
                        <v-text-field prepend-inner-icon="search"
                                        v-model="searchValues.Name"
                                        single-line
                                        solo
                                        color="red"
                                        hide-details
                                        @keyup.enter="search"></v-text-field>

                    </v-layout>
                </v-flex>
                <v-flex md10>
                    <v-layout>
                        <v-flex md12 class=" text-xs-right">
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

        <v-layout row>
            <v-flex xs12 m10>
                <v-btn class="btn_primary" @click="newAccount">
                    <v-icon>add</v-icon>
                    New Account
                </v-btn>
            </v-flex>
        </v-layout>

        <v-layout row class="table-spacer" v-if="list.data !== undefined">
            <v-flex xs12 m10>
                <v-data-table :headers="headers"
                              :no-data-text="defaultTableText"
                              :items="list.data.data"
                              hide-actions
                              :accesskey="table">
                    <template v-slot:items="props">
                        <td class="table_id no-break-word">
                            <span class="hidden">{{ props.item.id }}</span>
                            {{ props.item.name }}

                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.address }}
                        </td>
                        <td class="text-xs-left no-break-word">
                            {{ props.item.contact_person }}
                        </td>
                        <td class="text-xs-left no-break-word">{{ props.item.contact_number  }}</td>
                        <td class="text-xs-center">
                                <v-icon small
                                        class="mr-2 action-ic view-ic"
                                        @click="viewItem(props.item.id)">
                                    remove_red_eye
                                </v-icon>
                                <v-icon small
                                        class="mr-2 action-ic edit-ic"
                                        @click="editItem(props.item.id)">
                                    edit
                                </v-icon>
                                <v-icon small
                                        class="mr-2 action-ic delete-ic"
                                        @click="deleteItem(props.item.id)">
                                    delete
                                </v-icon>
                        </td>
                    </template>
                </v-data-table>
                <div class="text-xs-center pt-2">
                    <pagination v-if="pages > 1"
                                :maxVisibleButtons="5"
                                :total-pages="pages"
                                :current-page="searchValues.Page"
                                @pagechanged="onPageChange" />
                </div>
            </v-flex>
        </v-layout>
        <v-layout row class="table-spacer" v-else="">
            <v-flex xs12="" m10="">
                <v-data-table :headers="headers"
                              :no-data-text="defaultTableText"
                              :items="accounts"
                              hide-actions=""
                              :key="table">
                </v-data-table>
            </v-flex>
        </v-layout>

    </v-container>

</template>

<script>
    import { mapGetters } from 'vuex';
    import offline from 'v-offline';
    import confirm from '../../../common/layout/confirm-modal';
    import info from '../../../common/layout/info-modal';
    import loading from '../../../common/layout/progress';
    import constants from '../../../common/utils/constants';
    import pagination from '../../../common/components/pagination';

    export default {
        data() {
            return {
                fullscreenLoading: false,
                status: true,
                pages: 0,
                defaultTableText: constants.noRecords,
                headers: [
                    { text: 'Name', align: 'center', sortable: false, value: 'name', width: "15%"},
                    { text: 'Address', align: 'center', value: 'calories', sortable: false, width: "30%" },
                    { text: 'Contact Person', align: 'center', value: 'contact_person', sortable: false, width: "20%" },
                    { text: 'Contact Number', align: 'center', value: 'contact_number', sortable: false, width: "15%" },
                    { text: 'Actions', align: 'center', value: 'name', sortable: false, width: "20%" }
                ],
                accounts: [],
                table: 0,
                editedIndex: -1,
                editedItem: {
                    id: 0,
                },
                searchValues: {
                    Name: '',
                    Page: 1,
                    PageSize: 10,
                },
                name: '',
                newPage: 1,
                isPagination: false,
            }
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
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.fullscreenLoading = true;
                    this.name = this.searchValues.Name;
                    this.searchValues.Page = (this.isPagination) ? this.newPage : 1;
                    this.$store.dispatch(constants.searchAccount, this.searchValues).then(() => {
                        setTimeout(() => {
                            this.fullscreenLoading = false;
                            if (this.list.data) {
                                this.pages = this.list.data.pagination.pages;
                                this.isPagination = false;
                            } else {
                                this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                });
                            }
                        }, 1000);
                    })
                    this.table++;
                }
            },

            clear() {
                this.resetFilters()
                this.search();
            },

            editItem(id) {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                } else {
                    this.$router.push({ path: constants.accountEdit + id });
                }
            },

            viewItem(id) {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.accountView + id })
                }
            },

            deleteItem(id) {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$refs.confirm.open(constants.confirm, constants.deleteConfirm, { color: constants.modal_color }).then((confirm) => {
                        if (confirm) {
                            this.fullscreenLoading = true;
                            var oldData = this.$store.getters[constants.listAccount].data;
                            this.$store.dispatch(constants.destroyAccount, id).then(() => {
                                this.fullscreenLoading = false;
                                if(!this.$store.getters[constants.listAccount].message){
                                    this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                    });
                                }
                                else{
                                    var message = this.$store.getters[constants.listAccount].message;
                                    this.$store.getters[constants.listAccount].data = oldData;

                                    if (message === constants.deletedUser) {
                                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                            this.clearStore();
                                        });
                                    }
                                    else if (message === constants.notAdmin) {
                                        //stateError = message.ModelStateErrors;
                                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                            this.clearStore();
                                        });
                                    }

                                    else if(message.ModelStateErrors !== undefined){
                                        var stateError = "";
                                        for(let i=0; i < message.ModelStateErrors.length; i++){
                                            if(message.ModelStateErrors[i] == constants.recordNotExist){
                                                stateError = constants.notAvailable
                                            }
                                            else {
                                                stateError = message.ModelStateErrors[i];
                                            }
                                            this.$refs.info.open(constants.warning, stateError, { color: constants.modal_error }).then(() => {
                                                this.search();
                                            });
                                        }
                                    }
                                    else{
                                         this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                            if((this.$store.getters[constants.listAccount].data.data.length) === 1){
                                                this.resetFilters();
                                            }
                                            this.search();
                                        });
                                    }
                                }

                            });
                        }
                    })
                }
            },
            resetFilters(){
                this.searchValues = {
                    Name: '',
                    Page: 1,
                    PageSize: 10,
                }
                
            },
            newAccount() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.accountForm });
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
        },
        computed: {
            ...mapGetters({
                list: constants.listAccount,
            }),
        }


    }



</script>

<style>
</style>

