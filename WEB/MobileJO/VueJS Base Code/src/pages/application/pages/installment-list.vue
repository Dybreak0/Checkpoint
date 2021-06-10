<template>
  <v-container class="child-body" fluid pa-0 ma-0>
    <confirm ref="confirm"></confirm>
    <info ref="info"></info>
    <loading v-if="fullscreenLoading"></loading>
    <offline @detected-condition="handleConnectivityChange"></offline>
    <v-layout row>
      <v-card flat>
        <h2 class="d-flex align-center">
          <v-icon x-large>description</v-icon>List Installment Application
        </h2>
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
    <v-card class="search-filter-vcard" flat tile>
      <v-layout row wrap>
        <v-flex xs12 sm6 md5>
          <v-layout>
            <v-flex xs4 md4 class="input-label text-xs-right">
              <span>App: </span>
            </v-flex>
            <v-flex xs7 m7>
              <v-text-field
                v-model="searchListModel.application_no"
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
          <v-layout>
            <v-flex xs4 md4 class="input-label text-xs-right">
              <span>Created By: </span>
            </v-flex>
            <v-flex xs7 m7>
              <v-text-field
                v-model="searchListModel.created_by_name"
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
          <v-layout>
            <v-flex xs4 md4 class="input-label text-xs-right">
              <span>Client Name: </span>
            </v-flex>
            <v-flex xs7 m7>
              <v-text-field
                v-model="searchListModel.client_name"
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
        <v-flex xs12 sm6 md5>
          <v-layout>
            <v-flex xs4 md4 class="input-label text-xs-right">
              <span>Status:</span>
            </v-flex>

            <v-flex xs7 m7>
              <v-select
                v-model="searchListModel.status"
                :items="applicationStatusItems"
                solo
                color="black"
                hide-details
                @keyup.enter="search"
              ></v-select>
            </v-flex>
          </v-layout>
          <v-layout>
            <v-flex xs4 md4 class="input-label text-xs-right">
              <span>Created From: </span>
            </v-flex>
            <v-flex xs7 m7>
              <v-menu
                v-model="dateFromMenu"
                transition="scale-transition"
                :close-on-content-click="false"
                lazy
                offset-y
                full-width
              >
                <template v-slot:activator="{ on }">
                  <v-text-field
                    v-model="searchListModel.date_from"
                    v-on="on"
                    append-icon="event"
                    solo
                    readonly
                    color="red"
                    hide-details
                  >
                  </v-text-field>
                </template>
                <v-date-picker
                  class="customTable"
                  v-model="searchListModel.date_from"
                  color="red"
                  width="395"
                  @input="dateFromMenu = false"
                >
                </v-date-picker>
              </v-menu>
            </v-flex>
          </v-layout>
          <v-layout>
            <v-flex xs4 md4 class="input-label text-xs-right">
              <span>Created To: </span>
            </v-flex>
            <v-flex xs7 m7>
              <v-menu
                v-model="dateToMenu"
                transition="scale-transition"
                :close-on-content-click="false"
                lazy
                offset-y
                full-width
              >
                <template v-slot:activator="{ on }">
                  <v-text-field
                    v-model="searchListModel.date_to"
                    v-on="on"
                    append-icon="event"
                    solo
                    readonly
                    color="red"
                    hide-details
                  >
                  </v-text-field>
                </template>
                <v-date-picker
                  class="customTable"
                  v-model="searchListModel.date_to"
                  color="red"
                  width="395"
                  :min="searchListModel.date_from"
                  @input="dateToMenu = false"
                >
                </v-date-picker>
              </v-menu>
            </v-flex>
          </v-layout>
        </v-flex>
        <v-flex xs12 md2 d-flex align-end>
          <v-layout>
            <v-flex md12 class="text-xs-center">
              <v-btn
                class="btn_secondary"
                @click="search"
                :disabled="dateFilterNotValid"
                ><v-icon>search</v-icon>Search</v-btn
              >
              <v-btn class="btn_secondary" @click="clearFilters"
                ><v-icon>clear</v-icon>Clear</v-btn
              >
            </v-flex>
          </v-layout>
        </v-flex>
      </v-layout>
    </v-card>
    <v-layout row>
      <v-flex class="input-label">
        <span class="text-warning" v-if="dateFilterNotValid"
          >Date filter values are invalid. End Date should be greater than
          Start Date.
        </span>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-flex xs12 m12 class="text-xs-center text-sm-right">
        <v-btn class="btn_primary" @click="download" :disabled="exportDisabled">
          <v-icon>vertical_align_bottom</v-icon>&nbsp;Export to Excel
        </v-btn>
      </v-flex>
    </v-layout>

    <v-layout row class="table-spacer">
      <v-flex xs12 m10>
        <v-data-table
          :headers="headers"
          :items="list.data"
          :no-data-text="defaultTableText"
          hide-actions
        >
          <template v-slot:items="props">
            <td class="text-xs-center">
              <a @click="view(props.item.loan_id)"
                ><u>{{ props.item.application_no }}</u></a
              >
            </td>
            <td class="text-xs-center">{{ props.item.client_name }}</td>
            <td class="text-xs-center">
              {{ props.item.created_date | formatDate }}
            </td>
            <td class="text-xs-center">{{ props.item.created_by_name }}</td>
            <td class="text-xs-center">{{ props.item.loan_status }}</td>
          </template>
        </v-data-table>
        <div class="text-xs-center pt-2">
          <pagination
            v-if="pages > 1"
            :maxVisibleButtons="5"
            :total-pages="pages"
            :current-page="searchListModel.page"
            @pagechanged="onPageChange"
          />
        </div>
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
      status: true,
      applicationTypeItems: ["All", "E-Commerce", "Checkpoint"],
      applicationStatusItems: ["All", "Pending", "Denied", "Approved"],

      pages: 0,
      listLength: 0,
      newPage: 1,
      isPagination: false,
      dateFromMenu: false,
      dateToMenu: false,
      fullscreenLoading: false,
      defaultTableText: constants.noRecords,
      headers: [
        {
          text: "APP#",
          align: "center",
          sortable: false,
          value: "app",
          width: "25%"
        },
        {
          text: "Client Name",
          align: "center",
          value: "clientName",
          sortable: false,
          width: "25%"
        },
        {
          text: "Date Created",
          align: "center",
          value: "dateCreated",
          sortable: false,
          width: "15%"
        },
        {
          text: "Created By",
          align: "center",
          value: "createdBy",
          sortable: false,
          width: "25%"
        },
        {
          text: "Status",
          align: "center",
          value: "appStatus",
          sortable: false,
          width: "10%"
        }
      ],
      currentApplicationNo: "",
      currentCreatedBy: "",
      currentStatus: "",
      currentClientName: "",
      currentStartDate: "",
      currentEndDate: ""
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
    this.initialize();
    this.search();
  },

  methods: {
    initialize() {
      this.$store.dispatch("application/installment/clear");
    },
    onPageChange(page) {
      this.newPage = page;
      this.isPagination = true;
      this.search();
    },

    search() {
      if (this.status === false) {
        this.handleConnectivityChange(this.status);
      } else {
        this.fullscreenLoading = true;
        this.searchListModel.page = this.isPagination ? this.newPage : 1;
        this.$store.dispatch("application/installment/listLoan").then(() => {
          // Searches for job order records using the search filters
          this.fullscreenLoading = false;
          if (this.errorMessage === null) {
            this.isPagination = false;
            this.currentApplicationNo = this.searchListModel.application_no;
            this.currentClientName = this.searchListModel.client_name;
            this.currentCreatedBy = this.searchListModel.created_by_name;
            this.currentStatus = this.searchListModel.status;
            this.currentStartDate = this.searchListModel.date_from;
            this.currentEndDate = this.searchListModel.date_to;
            this.pages = this.list.pagination.pages;
            this.listLength = this.list.data.length;
          } else {
            this.$refs.info.open(constants.warning, this.errorMessage, {
              color: constants.error_color
            });
          }
        });
      }
    },
    download() {
      this.fullscreenLoading = true;
      this.searchListModel.application_no = this.currentApplicationNo;
      this.searchListModel.client_name = this.currentClientName;
      this.searchListModel.created_by_name = this.currentCreatedBy;
      this.searchListModel.status = this.currentStatus;
      this.searchListModel.date_from = this.currentStartDate;
      this.searchListModel.date_to = this.currentEndDate;
      this.$store.dispatch("application/installment/download").then(() => {
        // Retrieves all the job order records and puts it into an excel file
        this.fullscreenLoading = false;
        if (this.errorMessage !== null) {
          this.$refs.info.open(constants.warning, this.errorMessage, {
            color: constants.error_color
          });
        }
      });
    },
    view(loan_id) {
      console.log(loan_id);
      this.$router.push({
        name: "installmentView",
        params: {
          loan_id: loan_id.toString(),
          prevPage: "installmentList"
        }
      });
    },
    clearFilters() {
      this.$store.dispatch("application/installment/clearFilters"); // Resets the search filters to the default values
      this.search();
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
      list: "application/installment/list",
      searchListModel: "application/installment/searchListModel",
      errorMessage: "application/installment/errorMessage"
    }),
    dateFilterNotValid() {
      return this.searchListModel.date_to < this.searchListModel.date_from
        ? true
        : false;
    },
    exportDisabled() {
      return this.listLength > 0 && !this.dateFilterNotValid ? false : true;
    }
  }
};
</script>

<style></style>
