<template>
    <v-app>
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <v-navigation-drawer
            :mini-variant="miniVariant"
            :clipped="clipped"
            v-model="drawer"
            fixed
            app
            dark
            class="navigation-drawer"
        >
            <v-list>
                <v-card flat class="header-icon" router :to="header.to">
                    <v-card-text
                        class="d-flex justify-center align-center px-1 py-0"
                    >
                        <v-img
                            class="ml-2"
                            :src="
                                require('../../assets/images/logo-addessa-lg.png')
                            "
                            max-height="57"
                            max-width="60"
                            contain
                        ></v-img>
                        <v-toolbar-title class="ml-2">
                            Checkpoint Portal
                        </v-toolbar-title>
                    </v-card-text>
                </v-card>
                <v-divider></v-divider>
                <v-list-group prepend-icon="description" v-if="role == 1">
                    <v-list-tile slot="activator">
                        <v-list-tile-title
                            >Installment Application</v-list-tile-title
                        >
                    </v-list-tile>
                    <v-list-tile

                        router
                        :to="item.to"
                        :key="i"
                        v-for="(item, i) in installmentItemsAdmin"
                        exact
                    >
                        <v-list-tile-action>
                            <v-icon v-html="item.icon"></v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content>
                            <v-list-tile-title
                                v-text="item.title"
                            ></v-list-tile-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-list-group>
                <v-list-group prepend-icon="description" v-if="role == 2">
                    <v-list-tile slot="activator">
                        <v-list-tile-title
                            >Installment Application</v-list-tile-title
                        >
                    </v-list-tile>
                    <v-list-tile

                        router
                        :to="item.to"
                        :key="i"
                        v-for="(item, i) in installmentItemsUser"
                        exact
                    >
                        <v-list-tile-action>
                            <v-icon v-html="item.icon"></v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content>
                            <v-list-tile-title
                                v-text="item.title"
                            ></v-list-tile-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-list-group>
                <v-list-group prepend-icon="settings" v-if="role == 1">
                    <v-list-tile slot="activator">
                        <v-list-tile-title>Maintenance</v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile
                        router
                        :to="item.to"
                        :key="i"
                        v-for="(item, i) in items"
                        exact
                    >
                        <v-list-tile-action>
                            <v-icon v-html="item.icon"></v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content>
                            <v-list-tile-title
                                v-text="item.title"
                            ></v-list-tile-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-list-group>
                <!-- <v-list-group prepend-icon="description" v-if="role == 1">
                    <v-list-tile slot="activator">
                        <v-list-tile-title>Questionnaire</v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile
                        router
                        :to="item.to"
                        :key="i"
                        v-for="(item, i) in questionnaireItems"
                        exact
                    >
                        <v-list-tile-action>
                            <v-icon v-html="item.icon"></v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content>
                            <v-list-tile-title
                                v-text="item.title"
                            ></v-list-tile-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-list-group>

                <v-list-group prepend-icon="description" v-if="role == 1">
                    <v-list-tile slot="activator">
                        <v-list-tile-title>Questionnaire</v-list-tile-title>
                    </v-list-tile>
                    <v-list-tile
                        router
                        :to="item.to"
                        :key="i"
                        v-for="(item, i) in reportItems"
                        exact
                    >
                        <v-list-tile-action>
                            <v-icon v-html="item.icon"></v-icon>
                        </v-list-tile-action>
                        <v-list-tile-content>
                            <v-list-tile-title
                                v-text="item.title"
                            ></v-list-tile-title>
                        </v-list-tile-content>
                    </v-list-tile>
                </v-list-group> -->

                <v-list-tile @click="logout" replace="">
                    <v-list-tile-action>
                        <v-icon>exit_to_app</v-icon>
                    </v-list-tile-action>
                    <v-list-tile-title>
                        Logout
                    </v-list-tile-title>
                </v-list-tile>
            </v-list>
        </v-navigation-drawer>
        <v-toolbar
            app
            :clipped-left="clipped"
            style="background-color:#FF0000 !important"
        >
            <v-toolbar-side-icon
                style="color:#fff;"
                @click.stop="drawer = !drawer"
            ></v-toolbar-side-icon>
            <v-spacer></v-spacer>
            <div>
                <v-toolbar
                    dark
                    flat
                    style="background-color: transparent !important;"
                >
                    <h4 class="user-info">
                        <v-icon>face</v-icon>
                        <strong v-if="role==1"><span>Administrator</span></strong>
                        <strong v-if="role==2"><span>User</span></strong>
                        <span>|</span>
                        <strong><span>{{ fullName }}</span></strong>
                    </h4>
                </v-toolbar>
            </div>
        </v-toolbar>
        <v-content>
            <v-container fluid>
                <v-slide-y-transition mode="out-in">
                    <router-view />
                </v-slide-y-transition>
            </v-container>
        </v-content>
    </v-app>
</template>

<script>
import { mapGetters } from "vuex";
import otherUtils from "../utils/other-utils";
import { loadLanguageAsync } from "../../i18n/lang";
import confirm from "../../common/layout/confirm-modal";
import info from "../../common/layout/info-modal";
import constants from "../../common/utils/constants";

export default {
    name: "app-header",
    data() {
        return {
            clipped: false,
            drawer: true,
            fixed: false,
            authenticated: false,
            miniVariant: false,
            right: true,
            rightDrawer: false,
            title: "",
            header: { title: "Header", to: "/user-list" },
            items: [
                {
                    title: "User Maintenance",
                    to: "/user-list"
                },
                {
                    title: "Company Maintenance",
                    to: "/"
                },
                {
                    title: "Business Unit Maintenance",
                    to: "/"
                },
                {
                    title: "Branch Maintenance",
                    to: "/"
                },
                { title: 'Account Maintenance', to: '/account-list' }
            ],
            installmentItemsAdmin: [
                {
                    title: "Loan Requests",
                    to: "/application/list",
                },
                {
                    title: "Create Application",
                    to: "/application/create"
                },
                {
                    title: "Pending Application",
                    to: "/application/approval"
                }
            ],
            installmentItemsUser: [
                {
                    title: "Loan Requests",
                    to: "/application/list"
                },
                {
                    title: "Create Application",
                    to: "/application/create"
                },
            ],
            // questionnaireItems: [
            //     {
            //         title: "Response List",
            //         to: "/error/401"
            //     },
            //     {
            //         title: "Templates",
            //         to: "/questionnaire/template"
            //     }
            // ],
            // reportItems: [
            //         { title: 'TS JO Report', to: '/report/job-order' },
            //         { title: 'Assigned Cases Report', to: '/report/assigned-case' },
            //         { title: 'JO Client Rating Report', to: '/report/job-order-client-rating' }
            //     ],
            mounted() {
                if (isLoggedIn) {
                    this.$router.replace({ name: "/login" });
                }
            }
        };
    },
    components: {
        confirm,
        info
    },
    onIdle() {
        this.$refs.info
            .open(constants.message, constants.sessionTimeout, {
                color: constants.modal_color
            })
            .then(() => {
                this.clearStore();
            });
    },
    computed: {
        ...mapGetters({
            loading: "person/loading",
            isLoggedIn: "login/isLoggedIn",
            userName: "login/userName",
            fullName: "login/fullName",
            role: "login/role"
        })
    },
    methods: {
        logout() {
            this.$refs.confirm
                .open(constants.confirm, constants.cancelLogout, {
                    color: constants.modal_color
                })
                .then(confirm => {
                    if (confirm) {
                        this.clearStore();
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
            this.$router.push("/login");
        }
    }
};
</script>

<style></style>
