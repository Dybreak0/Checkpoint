<template>
    <v-layout column>
    <confirm ref="confirm"></confirm>
    <info ref="info"></info>
    <offline @detected-condition="handleConnectivityChange"></offline>
        <v-form v-model="valid" ref="form">
            <v-layout row v-if="!disabled && loaded" class="question-flex">
                <v-flex row xs2 md2 class="question">
                    <v-text-field
                        class="req"
                        v-model="question"
                        single-line
                        solo
                        :rules="questionRules"
                        placeholder="Enter Question"
                        maxlength="100"
                        color="black">
                    </v-text-field>
                </v-flex>
                <v-flex row xs2 md2 class="question">
                <v-select
                        v-model="select"
                        :items="questionType"
                        :rules ="[validateType]"
                        solo
                        label="Select Question Type"
                        color="black">
                    </v-select>
                </v-flex>
                <v-flex md4 xs4>
                    <v-btn @click="saveQuestion"
                        class="btn_primary">
                        <v-icon>add</v-icon>
                        Add
                    </v-btn>
                </v-flex>
            </v-layout>
        </v-form>
        <v-layout column>
            <v-flex 
            xs12 md12 class="vcard-center"
            v-for="(item, index) in questions" :key="item.question_id"
            >
                <v-card
                    class="question-card"
                >
                <v-card-title class="grey darken-0">
                    <v-layout row>
                        <v-flex xs6 md6 class="index-label">
                            <b class="question-id">#{{index + 1}}</b>
                        </v-flex>
                        <v-flex xs6 md6 class="actions-flex">
                            <v-layout row class="actions-row" v-if="activeQuestion != item.question_id">
                                <v-btn @click="deleteQuestion(item.question_id)" :class="(disabled || (activeQuestion != item.question_id && 0 != activeQuestion)) ? 'mx-2 actions-question btn_primary disable-events' : 'mx-2 actions-question btn_primary'" fab>
                                    <v-icon class="actions-icon">delete</v-icon>
                                </v-btn>
                                <v-btn @click="editQuestion(item.question_id)" :class="(disabled || (activeQuestion != item.question_id && 0 != activeQuestion)) ? 'mx-2 actions-question disable-events' : 'mx-2 actions-question'" fab  small color="#1332A8">
                                    <v-icon class="edit-icon">fas fa-edit</v-icon>
                                </v-btn>
                            </v-layout>
                             <v-layout row class="actions-row" v-else>
                                <v-btn @click="cancelUpdate(item.question_id, index)" :class="disabled ? 'mx-2 actions-question btn_primary disable-events' : 'mx-2 actions-question btn_primary'" fab>
                                    <v-icon class="actions-icon">fas fa-ban</v-icon>
                                </v-btn>
                                <v-btn @click="saveUpdatedQuestion(item.question_id, index)" :class="disabled ? 'mx-2 actions-question disable-events' : 'mx-2 actions-question'" fab  small color="#1332A8">
                                    <v-icon class="check-icon">fas fa-check</v-icon>
                                </v-btn>
                            </v-layout>
                        </v-flex>
                    </v-layout>
                </v-card-title>
                    <v-container class="question-body">
                        <v-layout row>
                            <v-flex row xs12 md12>
                                <v-layout row>
                                    <v-flex row xs6 md6>
                                        <v-text-field class="req"
                                            v-model="item.question"
                                            single-line
                                            solo
                                            :disabled="activeQuestion != item.question_id"
                                            maxlength="100"
                                            :rules="nameRules"
                                            color="black">
                                        </v-text-field>
                                    </v-flex>
                                    <v-flex xs6 md6 class="question-label-flex">
                                        <span class="question-label">{{item.question_type}}</span>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout> 
                        <v-flex class="add-flex" v-if="2 == item.question_type_id || 3 == item.question_type_id">
                                <v-btn @click="addChoice(item.question_id, index)"  :class="(disabled ||  null != activeChoice) ? 'mx-2 actions-question disable-events' : 'mx-2 actions-question'" fab small color="#1332A8">
                                    <v-icon class="add-icon">add</v-icon>
                                </v-btn>
                        </v-flex>
                        <v-flex class="choice-div" v-for="(data, i) in item.Choices" v-if="2 == item.question_type_id || 3 == item.question_type_id || 7 == item.question_type_id" :key="data.choice_id">
                                <choicesTemplate v-if="loaded" :key="formKey" :typeid=item.question_type_id :choice=data :questionID=item.question_id @saveChoice="saveChoice" @editChoice="editChoice" @resetOld="resetOld" @retrieveTemplate="retrieve" :disabled=isDisabledF(data.choice_id) :isActive='isActive(i, item.question_id)' @noActiveChoice=noActiveChoice @resetChoice=resetChoice :index=index :choiceIdx=i></choicesTemplate>
                                <choicesTemplate v-else></choicesTemplate>
                        </v-flex>
                    </v-container>        
                </v-card>      

            </v-flex>
        </v-layout>
    </v-layout>    
</template>

<script>
    import { mapGetters } from 'vuex';
    import offline from 'v-offline';
    import validators from '@/common/utils/form/validators';
    import constants from '../../../common/utils/constants';
    import choicesTemplate from './form-template';
    import confirm from '../../../common/layout/confirm-modal';
    import  info from '../../../common/layout/info-modal';
    export default {
        data : () => ({
            questions: [],
            valid: false,
            select: constants.defaultSelect,
            formKey: 0,
            loaded: false,
            disabled: true,
            question: "",
            questionType: [constants.defaultSelect],
            nameRules: [
                v => !!v || constants.fillRequireFieldsError,
                v =>  ((validators.textFormat).test(v)) || constants.invalidInput,
            ],
            questionRules: [
                 v => !!v || constants.fillRequireFieldsError,
            ],
            status: true,
            activeQuestion: 0,
            oldQuestion: "",
            currentQuestion: [],
            activeQuestionIndex: 0,
            activeChoice: null,
            quest_id: null
        }),
        components: {
            choicesTemplate,
            confirm,
            info,
            offline
        },
        props: {
            list: Object,
            isDisabled: Boolean,
            templateId: Number,
            retrieveTemplate: Function
        },
        computed: {
            ...mapGetters({
                questionTypes: constants.questionTypes,
                questionModel: constants.questionModel
            })
        },
        methods: {
            resetOld(choiceIdx, questionId){
                this.activeChoice = null;
                this.questions = JSON.parse(this.oldQuestions);
                this.formKey++;
            },
            noActiveChoice(){
                this.activeChoice = null;
                this.formKey++;
            },
            editChoice(index, questionID){
                this.activeChoice = index;
                this.quest_id = questionID;
                this.formKey++; 
            },
            resetChoice(index){
                var choice = this.questions[index].Choices;
                if(null != choice || undefined == choice){
                    this.questions[index].Choices.pop();
                }
                
                this.activeChoice = null;
            },
            saveChoice(){
                this.activeChoice = null;
                this.retrieve();
            },
            isActive(id, question_id){
                return id == this.activeChoice && null != this.activeChoice && (this.quest_id == question_id || null == this.quest_id);
            },
            isDisabledF(id){
                return id != this.activeChoice;
            },
            addChoice(id, index){
                var choice = {
                    'label' : '',
                    'value' : '',
                    'choice_id' : 0
                }
                this.questions[index].Choices.push(choice);
                this.activeChoice = this.questions[index].Choices.length - 1;
            },
            cancelUpdate(id, index){

                var oldQuestions = JSON.parse(this.oldQuestions)

                var tempOldData = oldQuestions.filter(data => data.question_id == id);
                var tempNewData = this.questions.filter(data => data.question_id == id);
                
                if(this.status === false){
                    this.handleConnectivityChange(this.status);
                }
                else{
                    if(JSON.stringify(tempNewData) == JSON.stringify(tempOldData)){
                        this.activeQuestion = 0;
                    }
                    else{
                        this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.questions.splice(index, 1, tempOldData[0]);
                                this.activeQuestion = 0;
                            }
                        });
                    }
                }
            },
            saveUpdatedQuestion(id, index){
                var tempNewData = this.questions.filter(data => data.question_id == id);
                this.currentQuestion = tempNewData;
                var question = tempNewData[0].question;
                if("" != question){
                    if (this.status === false) {
                        this.handleConnectivityChange(this.status)
                    } 
                    else{
                        this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                                if (confirm) {
                                    this.fullscreenLoading = true;
                                   setTimeout(() => {
                                        this.addUpdateModel();
                                        this.$store.dispatch(constants.editQuestion).then(() => {

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
                                                     this.retrieve();
                                                });
                                            } 
                                            else {
                                               this.$refs.info.open(constants.message, message, { color: constants.error_color }).then(() => {
                                            });
                                           }
                                         
                                        });
                                        this.fullscreenLoading = false;
                                    }, 1000);
                                }
                            })
                    }
                }
            },
            deleteQuestion(id){
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    this.$refs.confirm.open(constants.confirm, constants.deleteConfirm, { color: constants.modal_color }).then((confirm) => {
                        if (confirm) {
                            this.fullscreenLoading = true;
                            var oldData = this.$store.getters[constants.listTemplate].data;
                            this.$store.dispatch(constants.deleteQuestion, id).then(() => {
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
            editQuestion(id){
                this.activeQuestion = id;
            },
            getQuestionTypes(){
                this.$store.dispatch(constants.questionTypes).then(() => {
                    setTimeout(() => {
                        var message = this.questionTypes.message;
                        if(constants.success != message){
                          this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                            });  
                        }
                        else{
                            var questionTypes = this.questionTypes.data;
                            questionTypes && questionTypes.map((data, index) => {
                                this.questionType.push(data);
                            })
                        }
                    })
                })
            },
            retrieve() {
                this.$emit('retrieveTemplate');
            },
            saveQuestion(){
                this.$refs.form.validate();
                if(this.valid){
                    if (this.status === false) {
                        this.handleConnectivityChange(this.status)
                    } 
                    else{
                        this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                                if (confirm) {
                                    this.fullscreenLoading = true;
                                    setTimeout(() => {
                                        this.addToModel();
                                        this.$store.dispatch(constants.addQuestion).then(() => {

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
                                                     this.retrieve();
                                                });
                                            } 
                                            else {
                                               this.$refs.info.open(constants.message, message, { color: constants.error_color }).then(() => {
                                            });
                                           }
                                         
                                        });
                                        this.fullscreenLoading = false;
                                    }, 1000);
                                }
                            })
                    }
                }
            },

            dispatchAll(){
                this.$store.dispatch(constants.templateViewDetails, this.$props.templateId).then(() => {
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
            })
            },
            validateType(v) {
                if (v == constants.defaultSelect) {
                    return constants.fillRequireFieldsError;
                }
                else {
                    return true;
                }
            },

            addUpdateModel(){
                var currentQuestion = this.currentQuestion[0];
                this.$store.getters[constants.questionModel][0] = {
                    "questionid": currentQuestion.question_id,
                    "qquestion": currentQuestion.question
                }
            },

            addToModel(){
                
                this.$store.getters[constants.questionModel][0] = {
                    "templateID": this.$props.templateId,
                    "qquestion": this.question,
                    "questionTypeID": this.select,
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
            }

        },
        created() {
            this.getQuestionTypes();
            this.disabled = this.$props.isDisabled;
            this.loaded = false;     
            if(undefined != this.$props.list){
                this.loaded = true;    
                this.$props.list.Questions.map((data, index) => {
                    this.questions.push(data);
                })
                this.oldQuestions = JSON.stringify(this.questions);
            }
         },

    }
</script>
