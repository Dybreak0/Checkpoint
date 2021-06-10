<template>
    <v-form v-model="valid" ref="form">
        <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <loading v-if="fullscreenLoading"></loading>
        <offline @detected-condition="handleConnectivityChange"></offline>
            <v-container class="child-body">
                <v-layout row>
                    <v-card flat>
                        <h2>
                            <v-icon>description</v-icon> View Questionnaire
                        </h2>
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
                                            v-model="model.data && model.data.title"                                       
                                            single-line
                                            solo
                                            disabled
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
                                            v-model="model.data && model.data.description"        
                                            single-line
                                            solo
                                            disabled
                                            color="red">
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
                                            v-model="model.data && model.data.category"                                            
                                            single-line
                                            disabled
                                            solo>
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
                                        <v-combobox
                                        label=""
                                        chips
                                        solo
                                        class="box"
                                        disabled
                                        color="red"
                                        multiple
                                        v-model="model.data && model.data.respondents"
                                        >
                                        <template v-slot:selection="data">
                                            <v-chip :selected="data.selected" close>
                                                <strong>{{ data.item.Name }}</strong>&nbsp;
                                            </v-chip>
                                        </template>
                                        </v-combobox>
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
                                            transition="scale-transition"
                                            :close-on-content-click="false"                                            
                                            lazy                                            
                                            offset-y
                                            full-width>
                                            <template v-slot:activator="{ on }">
                                                <v-text-field 
                                                    v-model="model.data && model.data.start_date"
                                                    v-on="on"
                                                    disabled
                                                    append-icon="event"
                                                    readonly
                                                    color="red">
                                                </v-text-field>
                                            </template>
                                            <v-date-picker 
                                                v-model="model.data && model.data.start_date"
                                                class="customTable"
                                                color="red"  
                                                @input="dateFromMenu = false">
                                            </v-date-picker>
                                        </v-menu>
                                    </v-flex>
                                    <v-flex xs1 md1></v-flex>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>End Date: <b>*</b> </span>
                                    </v-flex>
                                    <v-flex xs2 md2>
                                        <v-menu
                                            transition="scale-transition"
                                            :close-on-content-click="false"
                                            lazy                                            
                                            offset-y
                                            full-width>
                                            <template v-slot:activator="{ on }">
                                                <v-text-field
                                                    v-model="model.data && model.data.end_date"
                                                    v-on="on"
                                                    disabled
                                                    append-icon="event"
                                                    readonly
                                                    color="red">
                                                </v-text-field>
                                            </template>
                                            <v-date-picker 
                                                class="customTable"
                                                color="red"                     
                                                @input="dateToMenu = false" >
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
                                            v-model="model.data && model.data.max_limit"                           
                                            single-line
                                            disabled
                                            solo>
                                        </v-text-field>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>  
                        <v-layout row>
                    <v-flex class="button-field"
                            xs12
                            offset-xs9>
                        <v-layout>
                            <v-flex offset-xs2>
                                <v-btn @click="back"
                                        class="btn_secondary">
                                    <v-icon>keyboard_return</v-icon>
                                    Cancel
                                </v-btn>
                            </v-flex>
                            <v-flex md10>
                                <v-btn
                                        @click="edit"
                                        class="btn_primary">
                                    <v-icon>edit</v-icon>
                                    Edit
                                </v-btn>
                            </v-flex>
                        </v-layout>
                    </v-flex>
                </v-layout>`
                    </v-card>
                </v-flex>
            </v-layout>
            <v-layout row>
                <questionsTemplate v-if="loaded" :key="formKey" :isView="true" :list=model.data :isDisabled="true"  :templateId="Number(this.$props.id)"></questionsTemplate>
                <questionsTemplate v-else></questionsTemplate>
            </v-layout row>
            </v-container>
        </v-form>
</template>


<script>
    import { mapGetters } from 'vuex';
    import offline from 'v-offline';
    import confirm from '../../../common/layout/confirm-modal';
    import info from '../../../common/layout/info-modal';
    import loading from '../../../common/layout/progress';
    import validators from '@/common/utils/form/validators';
    import constants from '../../../common/utils/constants';
    import moment from 'moment';
    import questionsTemplate from './questions';

    export default {
        data: () => ({
            valid: false,
            status: true,
            fullscreenLoading: false,
            loaded: false,
            formKey: 0,
        }),
        components: {
            confirm,
            info,
            loading,
            offline,
            questionsTemplate
        },
        created(){
            this.fullscreenLoading = true;
            this.loaded = false;
            this.$store.dispatch(constants.templateViewDetails, this.$props.id).then(() => {
                setTimeout(() => {
                    this.loaded = true;
                    var message = this.$store.getters[constants.templateModel].message;
                    if(!message){
                        this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                              this.$router.push({ path: constants.templateList })
                        });
                    }
                    else if(message === constants.recordNotExist){
                        this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                              this.$router.push({ path: constants.templateList })
                        });
                    }
                    else if (message === constants.deletedUser) {
                           this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                            this.clearStore();
                        });
                    }
                    else if (message === constants.notAdmin) {
                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                            this.clearStore();
                        });
                    }
                    
                    this.fullscreenLoading = false;
                }, 1000);
                 this.formKey++;
            })
        },
        props: {
            id: String
        },
        computed: {
            ...mapGetters({
                model: constants.templateModel
            })
        },
        methods: {
            
             back() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.templateList });
                }
            },

            edit(){
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$router.push({ path: constants.templateEdit + this.$props.id });
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

        }

    }
</script>

<style>
   
</style>


