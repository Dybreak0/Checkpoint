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
                            <v-icon>person</v-icon> Add Questionnaire
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
                                            v-model="template.title"                                          
                                            single-line
                                            :rules="nameRules"
                                            solo
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
                                            v-model="template.description"                        
                                            rows="2"           
                                            single-line
                                            :rules="nameRules"
                                            solo
                                             maxlength="250"
                                            @keyup.enter="search"
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
                                            v-model="template.category"                                            
                                            single-line
                                            :rules="nameRules"
                                            solo
                                            maxlength="100"
                                            @keyup.enter="search"
                                            color="red">
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
                                        v-model="template.respondents"
                                        multiple
                                        class="box"
                                        solo
                                        chips
                                        :rules="[required]"
                                        :items="respondents"
                                        item-text="BranchName"
                                        hide-selected
                                        return-object
                                        clearable
                                        >
                                        <template v-slot:selection="data">
                                            <v-chip :selected="data.selected"
                                                    close
                                                    @input="remove(data.item)">
                                                <strong>{{ data.item.BranchName }}</strong>&nbsp;
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
                                            full-width>
                                            <template v-slot:activator="{ on }">
                                                <v-text-field 
                                                    v-model="template.start_date"
                                                    v-on="on"
                                                    append-icon="event"
                                                    readonly
                                                    :min="template.end_date"
                                                    color="red">
                                                </v-text-field>
                                            </template>
                                            <v-date-picker 
                                                v-model="template.start_date"
                                                class="customTable"
                                                color="red"
                                                :min="template.start_date"    
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
                                            v-model="dateToMenu"
                                            transition="scale-transition"
                                            :close-on-content-click="false"
                                            lazy                                            
                                            offset-y
                                            full-width>
                                            <template v-slot:activator="{ on }">
                                                <v-text-field
                                                    v-model="template.end_date"
                                                    v-on="on"
                                                    append-icon="event"
                                                    readonly
                                                    color="red">
                                                </v-text-field>
                                            </template>
                                            <v-date-picker 
                                                v-model="template.end_date"
                                                class="customTable"
                                                color="red"         
                                                :min="template.start_date"             
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
                                            v-model="template.max_limit"   
                                            :rules="[validateNumbers]"                          
                                            single-line
                                            solo
                                            @keyup.enter="search"
                                            color="red">
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
                                <v-btn @click="save"
                                        class="btn_primary">
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
    export default {
        data: () => ({
            valid: false,
            respondents: [],
            template: {
                title: '',
                description: '',
                respondents: [],
                start_date: moment(new Date()).format(constants.dateFormat2),
                end_date: moment(new Date()).format(constants.dateFormat2),
                max_limit: ''
            },
            dateFromMenu: false,
            dateToMenu: false,
           nameRules: [
                v => !!v || constants.fillRequireFieldsError,
               v => ((validators.textFormat).test(v)) || constants.invalidInput,
            ],
            status: true,
            fullscreenLoading: false
        }),
        components: {
            confirm,
            info,
            loading,
            offline
        },
        created(){
            this.getBranches();
        },
        computed: {
            ...mapGetters({
                branches: constants.branchesList,
                model: constants.templateModel
            }),
            dateFilterNotValid() {
                return this.template.start_date > this.template.end_date;
            },
        },
        methods: {
            getBranches(){
                this.fullscreenLoading = true;
                this.$store.dispatch(constants.branchesList).then(() => {
                    this.fullscreenLoading = false;
                    setTimeout(() => {
                        var message = this.branches.message;
                        if(constants.success != message){
                          this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                            });  
                        }
                        else{
                            var branches = this.branches.data;
                            branches && branches.map((data, index) => {
                                var branch = { ID : data.BranchID, BranchName : data.Name }
                                this.respondents.push(branch);
                            })
                        }
                    })
                })
            },
            save() {
                this.$refs.form.validate()
                if (this.valid) {
                    if (this.status === false) {
                        this.handleConnectivityChange(this.status)
                    }
                    else {
                        this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.fullscreenLoading = true;
                                this.addToModel();
                                this.$store.dispatch(constants.addTemplate).then(() => {
                                    setTimeout(() => {
                                        var message = this.$store.getters[constants.listTemplate].message;
                                        if(!message){
                                              this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                            });  
                                        }
                                        else if(message === constants.failedSave){
                                            this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                            });
                                        }
                                        else if(message === constants.notAdmin){
                                            this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                                 this.clearStore();
                                            });
                                        }
                                        else if(message === constants.deletedUser){
                                            this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                                 this.clearStore();
                                            });
                                        }
                                        else if(message === constants.saved){
                                            this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                this.$router.push({ path: constants.templateList });
                                                
                                            });
                                        }
                                        else{
                                            this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                            });
                                        }
                                        this.fullscreenLoading = false;
                                    }, 1000)
                                })
                            }
                        })
                    }
                }
            },
            remove(item) {
                this.template.respondents.splice(this.template.respondents.indexOf(item), 1)
                this.template.respondents = [...this.template.respondents]
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
                }
                else if(0 > v){
                    return constants.greaterThanZero;
                }
                else {
                    return true;
                }
            },
             back() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    if (JSON.stringify(this.template) !== validators.emptyTemplate) {
                        this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.$router.push({ path: constants.templateList });
                            }
                        });
                    }
                    else {
                        this.$router.push({ path: constants.templateList });
                    }
                }
            },
            addToModel() {
                var respondents = [];
                this.template.respondents.map((data, index) => {
                    respondents.push(data.ID);
                });
                this.$store.getters[constants.templateModel][0] = {
                    "Title": this.template.title,
                    "Description": this.template.description,
                    "BranchIds": respondents,
                    "StartDate": this.template.start_date,
                    "EndDate": this.template.end_date,
                    "MaxLimit": this.template.max_limit,
                    "Category" : this.template.category
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


