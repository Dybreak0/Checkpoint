<template>
    <v-form v-model="valid" ref="form">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-container class="child-body">
            <v-layout row>
                <v-card flat>
                    <h2><v-icon>description</v-icon> Edit Questionnaire</h2>
                    <v-spacer></v-spacer>
                </v-card>
            </v-layout>
            <v-layout row>
                <v-divider></v-divider>
            </v-layout>
            <v-spacer class="formsSpacer"></v-spacer>
            <v-layout row>
                <v-flex xs8 m8 class="vcard-center">
                    <v-card class="search-filter-vcard" flat tile>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Title <b>*</b></span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-text-field
                                            v-model="
                                                model.data && model.data.title
                                            "
                                            single-line
                                            solo
                                            :rules="
                                                model.data &&
                                                    model.data.title &&
                                                    nameRules
                                            "
                                            maxlength="100"
                                            @keyup.enter="search"
                                            color="red"
                                        >
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Description <b>*</b></span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-textarea
                                            v-model="
                                                model.data &&
                                                    model.data.description
                                            "
                                            single-line
                                            :rules="
                                                model.data &&
                                                    model.data.description &&
                                                    nameRules
                                            "
                                            maxlength="250"
                                            solo
                                            @keyup.enter="search"
                                            color="red"
                                        >
                                        </v-textarea>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Category <b>*</b></span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-text-field
                                            v-model="
                                                model.data &&
                                                    model.data.category
                                            "
                                            single-line
                                            :rules="
                                                model.data &&
                                                    model.data.category &&
                                                    nameRules
                                            "
                                            maxlength="100"
                                            @keyup.enter="search"
                                            solo
                                        >
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Respondents <b>*</b></span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-select
                                            v-model="
                                                model.data &&
                                                    model.data.respondents
                                            "
                                            label=""
                                            item-text="Name"
                                            chips
                                            solo
                                            :rules="
                                                model.data &&
                                                    model.data.respondents && [
                                                        required
                                                    ]
                                            "
                                            class="box"
                                            multiple
                                            :items="respondents"
                                            hide-selected
                                            return-object
                                        >
                                            <template v-slot:selection="data">
                                                <v-chip
                                                    :selected="data.selected"
                                                    close
                                                    @input="remove(data.item)"
                                                >
                                                    <strong>{{
                                                        data.item.Name
                                                    }}</strong
                                                    >&nbsp;
                                                </v-chip>
                                            </template>
                                        </v-select>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Start Date: <b>*</b> </span>
                                    </v-flex>
                                    <v-flex xs2 md2>
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
                                                    v-model="
                                                        model.data &&
                                                            model.data
                                                                .start_date
                                                    "
                                                    v-on="on"
                                                    append-icon="event"
                                                    readonly
                                                    color="red"
                                                >
                                                </v-text-field>
                                            </template>
                                            <v-date-picker
                                                v-model="
                                                    model.data &&
                                                        model.data.start_date
                                                "
                                                class="customTable"
                                                color="red"
                                                :min="start_date"
                                                :max="
                                                    model.data &&
                                                        model.data.end_date
                                                "
                                                @input="dateFromMenu = false"
                                            >
                                            </v-date-picker>
                                        </v-menu>
                                    </v-flex>
                                    <v-flex xs1 md1></v-flex>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>End Date: <b>*</b> </span>
                                    </v-flex>
                                    <v-flex xs2 md2>
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
                                                    v-model="
                                                        model.data &&
                                                            model.data.end_date
                                                    "
                                                    v-on="on"
                                                    append-icon="event"
                                                    readonly
                                                    color="red"
                                                >
                                                </v-text-field>
                                            </template>
                                            <v-date-picker
                                                v-model="model.data && model.data.end_date"
                                                class="customTable"
                                                color="red"
                                                :min="
                                                    model.data &&
                                                        model.data.start_date
                                                "
                                                @input="dateToMenu = false"
                                            >
                                            </v-date-picker>
                                        </v-menu>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Max Limit Per Day: <b>*</b></span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-text-field
                                            type="number"
                                            v-model="
                                                model.data &&
                                                    model.data.max_limit
                                            "
                                            single-line
                                            :rules="
                                                model.data &&
                                                    model.data.max_limit && [
                                                        validateNumbers
                                                    ]
                                            "
                                            @keyup.enter="search"
                                            color="red"
                                            solo
                                        >
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex class="button-field" xs12 offset-xs9>
                                <v-layout>
                                    <v-flex offset-xs2>
                                        <v-btn
                                            @click="cancel"
                                            class="btn_secondary"
                                        >
                                            <v-icon>keyboard_return</v-icon>
                                            Cancel
                                        </v-btn>
                                    </v-flex>
                                    <v-flex md10>
                                        <v-btn
                                            @click="save"
                                            class="btn_primary"
                                        >
                                            <v-icon>save</v-icon>
                                            Save
                                        </v-btn>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                    </v-card>
                </v-flex>
            </v-layout>
            <v-layout row>
                <questionsTemplate
                    v-if="loaded"
                    :key="formKey"
                    :list="model.data"
                    :isDisabled="false"
                    @retrieveTemplate="retrieveTemplate"
                    :templateId="Number(this.$props.id)"
                ></questionsTemplate>
                <questionsTemplate v-else></questionsTemplate>
            </v-layout>
        </v-container>
    </v-form>
</template>

<script>
import { mapGetters } from "vuex";
import offline from "v-offline";
import confirm from "../../../common/layout/confirm-modal";
import info from "../../../common/layout/info-modal";
import loading from "../../../common/layout/progress";
import validators from "@/common/utils/form/validators";
import constants from "../../../common/utils/constants";
import moment from "moment";
import questionsTemplate from "./questions";
export default {
    data: () => ({
        respondents: [],
        valid: false,
        status: true,
        loaded: false,
        fullscreenLoading: false,
        formKey: 0,
        nameRules: [
            v => !!v || constants.fillRequireFieldsError,
            v => validators.textFormat.test(v) || constants.invalidInput
        ],
        dateFromMenu: false,
        dateToMenu: false,
        start_date: moment(new Date()).format(constants.dateFormat2),
        oldTemplateData: ""
    }),
    components: {
        confirm,
        info,
        loading,
        offline,
        questionsTemplate
    },
    created() {
        this.loaded = false;
        this.fullscreenLoading = true;
        this.retrieveTemplate();
    },
    props: {
        id: String
    },
    computed: {
        ...mapGetters({
            model: constants.templateModel,
            branches: constants.branchesList,
            questionTypes: constants.questionTypes
        })
    },
    methods: {
        remove(item) {
            this.model.data.respondents.splice(
                this.model.data.respondents.indexOf(item),
                1
            );
            this.model.data.respondents = [...this.model.data.respondents];
        },

        retrieveTemplate() {
            this.$store
                .dispatch(constants.templateViewDetails, this.$props.id)
                .then(() => {
                    setTimeout(() => {
                        this.loaded = true;
                        var message = this.$store.getters[
                            constants.templateModel
                        ].message;
                        if (!message) {
                            this.$refs.info
                                .open(constants.warning, constants.noInternet, {
                                    color: constants.error_color
                                })
                                .then(() => {
                                    this.$router.push({
                                        path: constants.templateList
                                    });
                                });
                        } else if (message === constants.recordNotExist) {
                            this.$refs.info
                                .open(constants.warning, message, {
                                    color: constants.error_color
                                })
                                .then(() => {
                                    this.$router.push({
                                        path: constants.templateList
                                    });
                                });
                        } else if (message === constants.deletedUser) {
                            this.$refs.info
                                .open(constants.warning, message, {
                                    color: constants.modal_error
                                })
                                .then(() => {
                                    this.clearStore();
                                });
                        } else if (message === constants.notAdmin) {
                            this.$refs.info
                                .open(constants.warning, message, {
                                    color: constants.modal_error
                                })
                                .then(() => {
                                    this.clearStore();
                                });
                        } else {
                            this.getInitialValue();
                            this.getBranches();
                        }

                        this.fullscreenLoading = false;
                    }, 1000);
                    this.formKey++;
                });
        },

        save() {
            if (
                JSON.stringify(
                    this.$store.getters[constants.templateModel].data
                ) !== this.oldTemplateData
            ) {
                this.$refs.form.validate();
                if (this.valid) {
                    if (this.status === false) {
                        this.handleConnectivityChange(this.status);
                    } else {
                        this.$refs.confirm
                            .open(constants.confirm, constants.saveConfirm, {
                                color: constants.modal_color
                            })
                            .then(confirm => {
                                if (confirm) {
                                    this.fullscreenLoading = true;
                                    setTimeout(() => {
                                        this.addToModel();
                                        this.$store
                                            .dispatch(constants.editTemplate)
                                            .then(() => {
                                                var message = this.$store
                                                    .getters[
                                                    constants.listTemplate
                                                ].message;

                                                if (
                                                    this.$store.getters[
                                                        constants.listTemplate
                                                    ] === constants.noInternet
                                                ) {
                                                    this.$refs.info
                                                        .open(
                                                            constants.message,
                                                            constants.noInternet,
                                                            {
                                                                color:
                                                                    constants.error_color
                                                            }
                                                        )
                                                        .then(() => {});
                                                } else if (
                                                    message ===
                                                    constants.deletedUser
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
                                                    message ===
                                                    constants.notAdmin
                                                ) {
                                                    this.$refs.info
                                                        .open(
                                                            constants.warning,
                                                            message,
                                                            {
                                                                color:
                                                                    constants.error_color
                                                            }
                                                        )
                                                        .then(() => {
                                                            this.clearStore();
                                                        });
                                                } else if (
                                                    message === constants.saved
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
                                                            this.$router.push({
                                                                path:
                                                                    constants.templateList
                                                            });
                                                        });
                                                } else {
                                                    this.$refs.info
                                                        .open(
                                                            constants.message,
                                                            message,
                                                            {
                                                                color:
                                                                    constants.error_color
                                                            }
                                                        )
                                                        .then(() => {});
                                                }
                                            });
                                        this.fullscreenLoading = false;
                                    }, 1000);
                                }
                            });
                    }
                }
            } else {
                this.$refs.info
                    .open(constants.message, constants.saved, {
                        color: constants.modal_color
                    })
                    .then(() => {
                        this.$router.push({ path: constants.templateList });
                    });
            }
        },
        cancel() {
            if (this.status === false) {
                this.handleConnectivityChange(this.status);
            } else {
                if (
                    JSON.stringify(
                        this.$store.getters[constants.templateModel].data
                    ) !== this.oldTemplateData
                ) {
                    this.$refs.confirm
                        .open(constants.confirm, constants.cancelConfirm, {
                            color: constants.modal_color
                        })
                        .then(confirm => {
                            if (confirm) {
                                this.$router.push({
                                    path: constants.templateList
                                });
                            }
                        });
                } else {
                    this.$router.push({ path: constants.templateList });
                }
            }
        },
        getInitialValue() {
            this.oldTemplateData = JSON.stringify(
                this.$store.getters[constants.templateModel].data
            );
        },
        required(value) {
            if (value instanceof Array && value.length == 0) {
                return constants.fillRequireFieldsError;
            }
            return !!value || constants.fillRequireFieldsError;
        },
        validateNumbers(v) {
            if (!v) {
                return constants.fillRequireFieldsError;
            } else if (0 > v) {
                return constants.greaterThanZero;
            } else {
                return true;
            }
        },
        getBranches() {
            this.fullscreenLoading = true;
            this.$store.dispatch(constants.branchesList).then(() => {
                this.fullscreenLoading = false;
                setTimeout(() => {
                    var message = this.branches.message;
                    if (constants.success != message) {
                        this.$refs.info
                            .open(constants.warning, constants.noInternet, {
                                color: constants.error_color
                            })
                            .then(() => {});
                    } else {
                        var branches = this.branches.data;
                        branches &&
                            branches.map((data, index) => {
                                var branch = {
                                    BranchID: data.BranchID,
                                    Name: data.Name
                                };
                                this.respondents.push(branch);
                            });
                    }
                });
            });
        },
        addToModel() {
            var templateModel = this.$store.getters[constants.templateModel]
                .data;

            var respondents = [];

            templateModel.respondents &&
                templateModel.respondents.map((data, index) => {
                    respondents.push(data.BranchID);
                });

            this.$store.getters[constants.templateModel].data.TemplateID =
                templateModel.id;
            this.$store.getters[constants.templateModel].data.Title =
                templateModel.title;
            this.$store.getters[constants.templateModel].data.Description =
                templateModel.description;
            this.$store.getters[constants.templateModel].data.Category =
                templateModel.category;
            this.$store.getters[constants.templateModel].data.StartDate =
                templateModel.start_date;
            this.$store.getters[constants.templateModel].data.EndDate =
                templateModel.end_date;
            this.$store.getters[constants.templateModel].data.MaxLimit =
                templateModel.max_limit;
            this.$store.getters[
                constants.templateModel
            ].data.BranchIds = respondents;
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
                this.$refs.info
                    .open(constants.message, constants.noInternet, {
                        color: constants.error_color
                    })
                    .then(() => {});
            } else {
                this.status = true;
            }
        }
    }
};
</script>

<style></style>
