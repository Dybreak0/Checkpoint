<template>
    <v-layout row>
         <confirm ref="confirm"></confirm>
        <info ref="info"></info>
        <offline @detected-condition="handleConnectivityChange"></offline>
        <v-form v-model="valid" ref="form" class="choice-form">
        <v-layout row>
            <v-flex row xs1 md1 v-if="3 == questionType" class="type-icon">
                <v-icon>radio_button_unchecked</v-icon>
            </v-flex>
            <v-flex row xs1 md1 v-if="2 == questionType" class="type-icon">
                <v-icon>check_box_outline_blank</v-icon>
            </v-flex>
            <v-flex row xs6 md6 v-if="2 == questionType || 3 == questionType" >
                <v-text-field class="req"
                    v-model="choiceModel.label"
                    single-line
                    solo
                    :rules="nameRules"
                    :disabled="!active"
                    maxlength="100"
                    color="black">
                </v-text-field>
                
            </v-flex>
            <v-flex row xs6 md6 v-if="(2 == questionType || 3 == questionType) && !active">
                <v-layout row class="choice-actions-row">
                    <v-btn @click="deleteChoice(choiceModel.choice_id)" :class=" IsView ? 'mx-2 actions-question btn_primary disable-events' : 'mx-2 actions-question btn_primary'" small fab>
                    <v-icon class="actions-icon">delete</v-icon>
                    </v-btn>
                    <v-btn @click="setActive(choiceIndex)" :class="IsView ? 'mx-2 actions-question disable-events' : 'mx-2 actions-question'" small fab color="#1332A8">
                        <v-icon class="edit-icon">fas fa-edit</v-icon>
                    </v-btn>
                </v-layout>
                
            </v-flex>
            <v-flex row xs6 md6 v-if="(2 == questionType || 3 == questionType) && active">
                <v-layout row class="choice-actions-row">
                    <v-btn @click="cancelAddChoice()" class="mx-2 actions-question btn_primary" small fab>
                    <v-icon class="actions-icon">fas fa-ban</v-icon>
                    </v-btn>
                    <v-btn @click="addChoice()" class="mx-2 actions-question" small fab color="#1332A8">
                        <v-icon class="check-icon1">fas fa-check</v-icon>
                    </v-btn>
                </v-layout>
                
            </v-flex>
            <v-flex xs2 md2  v-if="7 == questionType">
                <v-select v-model="choiceModel.label"
                        :items="sliderItems"
                        solo
                        :disabled="!active"
                        color="black">
                        </v-select>
                    </v-flex>
                    <v-flex xs5 md5 class="range-input" v-if="7 == questionType">
                        <v-text-field class="req"
                            v-model="choiceModel.value"
                            single-line
                            label="Label (Optional)"
                            solo
                            :disabled="!active"
                            maxlength="100"
                            color="black">
                        </v-text-field>
                    </v-flex>
                    <v-flex row xs6 md6 v-if="7 == questionType && !active">
                    <v-layout row class="choice-actions-row">
                        <v-btn @click="setActive(choiceIndex)" class="mx-2 actions-question" small fab color="#1332A8">
                            <v-icon class="edit-icon">fas fa-edit</v-icon>
                        </v-btn>
                    </v-layout>
                     </v-flex>
                    <v-flex row xs6 md6 v-if="7 == questionType && active">
                    <v-layout row class="choice-actions-row">
                        <v-btn @click="cancelAddChoice()" class="mx-2 actions-question btn_primary" small fab>
                        <v-icon class="actions-icon">fas fa-ban</v-icon>
                        </v-btn>
                        <v-btn @click="addChoice()" class="mx-2 actions-question" small fab color="#1332A8">
                            <v-icon class="check-icon1">fas fa-check</v-icon>
                        </v-btn>
                    </v-layout>
            </v-flex>
            </v-layout>
        </v-form>
    </v-layout>
</template>

<script>
    import { mapGetters } from 'vuex';
    import validators from '@/common/utils/form/validators';
    import constants from '../../../common/utils/constants';
    import confirm from '../../../common/layout/confirm-modal';
    import info from '../../../common/layout/info-modal';
    import offline from 'v-offline';
    export default {
        data : () => ({
               choiceModel: {},
               questionType: 0,
               sliderItems: ['1','2','3','4','5','6','7','8','9','10'],
               isRangeReady: false,
               range: {},
               active: false,
               nameRules: [
                v => !!v || constants.fillRequireFieldsError,
                v =>  ((validators.textFormat).test(v)) || constants.invalidInput,
                ],
                currentChoice: "",
                status: true,
                questionId: 0,
                oldChoice: "",
                idx: null,
                valid: false,
                choiceIndex: 0,
                IsView: false
        }),
         components: {
            confirm,
            info,
            offline
        },
        props: {
            choice: Object,
            disabled: Boolean,
            typeid: Number,
            questionID: Number,
            isActive: Boolean,
            saveChoice: Function,
            resetChoice: Function,
            index: Number,
            retrieveTemplate: Function,
            editChoice: Function,
            choiceIdx: Number,
            noActiveChoice: Function,
            resetOld: Function,
            isView: Boolean
        },
        computed: {
            ...mapGetters({
                rangeQuestion: constants.questionRange
            })
        },
        methods: {
            deleteChoice(id){
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$refs.confirm.open(constants.confirm, constants.deleteConfirm, { color: constants.modal_color }).then((confirm) => {
                        if (confirm) {
                            this.fullscreenLoading = true;
                            var oldData = this.$store.getters[constants.listTemplate].data;
                            this.$store.dispatch(constants.deleteChoice, id).then(() => {
                                this.fullscreenLoading = false;
                                 if(!this.$store.getters[constants.listTemplate].message){
                                    this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                                    });
                                }
                                else{
                                    var message = this.$store.getters[constants.listTemplate].message;
                                    this.$store.getters[constants.listTemplate].data = oldData;
                                    if (message === constants.deletedUser) {
                                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                            this.clearStore();
                                        });
                                    }
                                    else if (message === constants.notAdmin) {
                                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                            this.clearStore();
                                        });
                                    }
                                    else if (message === constants.recordNotExist) {
                                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                        
                                        });
                                    }
                                    else if (message === constants.SuccesfullyDeleted){
                                         this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                            this.retrieve();
                                        });
                                    }
                                    else{
                                        this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                        
                                        });
                                    }

                                }

                            });
                        }
                    })
                }
                
            },
            cancelAddChoice(){
                var newChoice = this.choiceModel.label.trim();
                var oldChoiceJSON = JSON.parse(this.oldChoice);
                var oldChoice = oldChoiceJSON.label.trim();

                var choiceId = this.choiceModel.choice_id;

                if(this.status === false){
                    this.handleConnectivityChange(this.status);
                }
                else{
                    if(0 == choiceId){
                        if(newChoice == oldChoice){
                            this.choiceReset(this.idx);
                        }
                        else{
                            this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                                if (confirm) {
                                    this.choiceReset(this.idx);
                                }
                            });   
                        }
                    }
                    else{
                        var choiceData = JSON.stringify(this.choiceModel);
                        if(choiceData == this.oldChoice){
                            this.$emit('noActiveChoice');
                        }
                        else{
                             this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.$emit('resetOld', this.idx, this.questionID);
                            }
                        });
                        }
                    }
                }
                

            },
            addChoice(){
                this.$refs.form.validate();
                var choice = this.choiceModel.label.trim();
                if(this.valid){
                    if(this.status === false){
                       this.handleConnectivityChange(this.status);
                    }
                    else{
                        this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                                if (confirm) {
                                    this.fullscreenLoading = true;
                                     setTimeout(() => {
                                        var choiceID = this.choiceModel.choice_id;
                                        if(0 == choiceID){
                                            this.addChoiceModel();
                                        this.$store.dispatch(constants.addChoice).then(() => {

                                           var message = this.$store.getters[constants.listTemplate].message;

                                           if(this.$store.getters[constants.listTemplate] === constants.noInternet){
                                                this.$refs.info.open(constants.message, constants.noInternet, { color: constants.error_color }).then(() => {
                                                });
                                           }
                                           else if (message === constants.deletedUser) {
                                                this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                                    this.clearStore();
                                                });
                                            }
                                    
                                           else if (message === constants.notAdmin) {
                                               this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                                   this.clearStore();
                                               });
                                           }
                                           else if(message === constants.saved){
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.choiceSave();
                                                });
                                            } 
                                            else {
                                               this.$refs.info.open(constants.message, message, { color: constants.error_color }).then(() => {
                                            });
                                           }
                                         
                                        });
                                        }
                                        else{
                                            this.addUpdateModel();
                                            this.$store.dispatch(constants.editChoice).then(() => {

                                           var message = this.$store.getters[constants.listTemplate].message;

                                           if(this.$store.getters[constants.listTemplate] === constants.noInternet){
                                                this.$refs.info.open(constants.message, constants.noInternet, { color: constants.error_color }).then(() => {
                                                });
                                           }
                                           else if (message === constants.deletedUser) {
                                                this.$refs.info.open(constants.warning, message, { color: constants.modal_error }).then(() => {
                                                    this.clearStore();
                                                });
                                            }
                                    
                                           else if (message === constants.notAdmin) {
                                               this.$refs.info.open(constants.warning, message, { color: constants.error_color }).then(() => {
                                                   this.clearStore();
                                               });
                                           }
                                           else if(message === constants.saved){
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.choiceSave();
                                                });
                                            } 
                                            else {
                                               this.$refs.info.open(constants.message, message, { color: constants.error_color }).then(() => {
                                            });
                                           }
                                         
                                        });
                                            
                                        }
                                        this.fullscreenLoading = false;
                                    }, 1000);
                                }
                            })
                    }
                }
            },
            handleConnectivityChange(status) {
                if (status === false) {
                    this.status = false;
                    this.$refs.info.open(constants.message, constants.noInternet, { color: constants.error_color }).then(() => {});
                }
                else {
                    this.status = true;
                }
            },

            addChoiceModel(){
                var currentChoice = this.choiceModel;
                this.$store.getters[constants.choiceModel][0] = {
                    "label" : currentChoice.label,
                    "value" : currentChoice.value,
                    "questionid" : this.questionId
                }
            },

            addUpdateModel(){
                var currentChoice = this.choiceModel;
                 this.$store.getters[constants.choiceModel][0] = {
                    "label" : currentChoice.label,
                    "value" : currentChoice.value,
                    "choiceid" : currentChoice.choice_id
                }
            },

            choiceReset(idx){
                this.$emit('resetChoice', idx);
            },

           choiceSave(){
                this.$emit('saveChoice');
            },

            retrieve() {
                this.$emit('retrieveTemplate');
            },
            setActive(idx){
                this.$emit('editChoice', idx, this.questionId);
            }

        },
        created() {
            this.isRangeReady = false;
            this.active = this.$props.isActive;
            this.idx = this.$props.index;
            this.choiceIndex = this.$props.choiceIdx;
            this.questionId = this.$props.questionID;
            if(undefined != this.$props.choice){
                this.choiceModel = this.$props.choice;
                this.questionType = this.$props.typeid;
                this.oldChoice = JSON.stringify(this.choiceModel);
            }
            if(undefined != this.$props.choice.range){
                this.range = this.$props.choice.range;
            }
        },

    }
</script>