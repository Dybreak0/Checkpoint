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
                        <v-icon>person</v-icon> View Response
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
                                        <span> Title</span>
                                    </v-flex>
                                    <v-flex xs3 md3 class="input-label">
                                        {{model.data.data.title}}
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Description</span>
                                    </v-flex>
                                    <v-flex xs3 md3 class="input-label">
                                        {{model.data.data.description}}
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex md3 class="input-label">
                                        <span>Category:</span>
                                    </v-flex>
                                    <v-flex xs3 md3 class="input-label">
                                        {{model.data.data.category}}
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Duration:</span>
                                    </v-flex>
                                    <v-flex class="input-label">
                                        {{model.data.data.duration}}
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Status:</span>
                                    </v-flex>
                                    <v-flex>
                                        <v-checkbox class="input-label"
                                                    v-model="model.data.data.isApproved"
                                                    :label="approval"
                                                    color="black"></v-checkbox>
                                    </v-flex>
                                </v-layout>
                            </v-flex>
                        </v-layout>
                        <v-layout row>
                            <v-flex xs8 md8>
                                <v-layout>
                                    <v-flex xs3 md3 class="input-label">
                                        <span>Remarks:</span>
                                    </v-flex>
                                    <v-flex xs7 m7>
                                        <v-textarea v-model="model.data.data.remarks"
                                                    :rules="[validateField]"
                                                    single-line
                                                    solo
                                                    color="red">
                                        </v-textarea>
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
            <v-layout row>
                <v-layout column>
                    <v-flex xs12 md12 class="vcard-center"
                            v-for="(item, index) in model.data.data.questionList" :key="item.questionID">
                        <v-card class="question-card">
                            <v-card-title class="grey darken-0">
                                <v-layout row>
                                    <v-flex xs6 md6 class="index-label">
                                        <b class="questionID">#{{index + 1}}</b>
                                    </v-flex>
                                </v-layout>
                            </v-card-title>
                            <v-container class="question-body">
                                <v-layout row>
                                    <v-flex row xs12 md12>
                                        <v-layout row>
                                            <v-flex row xs6 md6>
                                                <v-textarea v-model="item.question"
                                                            solo
                                                            :readonly="true"
                                                            maxlength="100"
                                                            color="black"
                                                            rows="3">
                                                </v-textarea>
                                            </v-flex>
                                            <v-flex xs6 md6 class="question-label-flex">
                                                <span class="question-label">{{item.questionType}}</span>
                                            </v-flex>
                                        </v-layout>
                                    </v-flex>
                                </v-layout>

                                <v-flex class="choice-div" v-for="choice in item.choices" v-if="questionCheckbox == item.questionTypeID || questionRadio == item.questionTypeID || questionSlider == item.questionTypeID" :key="choice.choiceID">
                                    <v-layout row>
                                        <v-flex row xs1 md1 v-if="questionRadio == item.questionTypeID" class="type-icon">
                                            <v-icon color="black"
                                                    :readonly="true"
                                                    v-if="choice.isSelected">radio_button_checked</v-icon>
                                            <v-icon color="black"
                                                    :readonly="true"
                                                    v-else>radio_button_unchecked</v-icon>
                                        </v-flex>
                                        <v-flex row xs1 md1 v-if="questionCheckbox == item.questionTypeID" class="type-icon">
                                            <v-checkbox v-if="choice.isSelected"
                                                        :readonly="true"
                                                        input-value="true"
                                                        color="black"></v-checkbox>
                                            <v-checkbox v-else
                                                        :readonly="true"
                                                        color="black"></v-checkbox>

                                        </v-flex>
                                        <v-flex row xs6 md6 v-if="questionCheckbox == item.questionTypeID || questionRadio == item.questionTypeID">
                                            <v-text-field v-model="choice.label"
                                                          :readonly="true"
                                                          single-line
                                                          solo
                                                          maxlength="100"
                                                          color="black">
                                            </v-text-field>
                                        </v-flex>
                                        <v-flex xs2 md2 v-if="questionSlider == item.questionTypeID">
                                            <v-select v-model="item.answer.value"
                                                      :items="sliderItems"
                                                      :readonly="true"
                                                      solo
                                                      color="black">
                                            </v-select>
                                        </v-flex>
                                        <v-flex xs5 md5 class="range-input" v-if="questionSlider == item.questionTypeID">
                                            <v-text-field class="req"
                                                          v-model="choice.value"
                                                          :readonly="true"
                                                          single-line
                                                          solo
                                                          maxlength="100"
                                                          color="black">
                                            </v-text-field>
                                        </v-flex>
                                    </v-layout>
                                </v-flex>
                                <v-flex class="choice-div" v-if="questionText == item.questionTypeID" :key="item.answer.answerID">
                                    <v-textarea v-model="item.answer.value"
                                                :readonly="true"
                                                solo
                                                maxlength="100"
                                                color="black"
                                                rows="2">
                                    </v-textarea>
                                </v-flex>

                                <v-flex class="choice-div videoDiv" v-if="questionVideo == item.questionTypeID" :key="item.answer.answerID" :data-answerID="item.answer.answerID">
                                    <v-btn @click="getVideo(item.answer.answerID)"
                                           class="btn_secondary">
                                        Load Video
                                    </v-btn>
                                    <video id="videoElement" class="response-media" controls :answerID="item.answer.answerID" hidden>
                                        <source :src="item.answer.value" type="video/mp4">
                                    </video>
                                    <v-flex id="videoSpinner" class="spinner-video" hidden>
                                        <v-progress-circular indeterminate color="red"></v-progress-circular>
                                    </v-flex>

                                </v-flex>

                                <v-flex class="choice-div imageDiv" v-if="questionImage == item.questionTypeID" :key="item.answer.answerID" :data-answerID="item.answer.answerID">
                                    <v-btn @click="getImage(item.answer.answerID)"
                                           class="btn_secondary">
                                        Load Image
                                    </v-btn>
                                    <br />
                                    <img id="imageElement" class="response-media"
                                         :src="item.answer.value" hidden>
                                    <v-flex id="imageSpinner" class="spinner-image" hidden>
                                        <v-progress-circular indeterminate color="red"></v-progress-circular>
                                    </v-flex>
                                </v-flex>
                                
                            </v-container>
                        </v-card>
                    </v-flex>
                </v-layout>
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
    import constants from '../../../common/utils/constants';
    import validators from '@/common/utils/form/validators';
    import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

    export default {
        data() {
            return {
                valid: false,
                fullscreenLoading: false,
                sliderItems: ['1', '2', '3', '4', '5', '6', '7', '8', '9', '10'],
                approval: 'Approval',
                oldResponse: "",
                status: true,
                constants,
                questionText: 1,
                questionCheckbox : 2,
                questionRadio: 3,
                questionVideo: 5,
                questionImage: 6,
                questionSlider: 7,
            }
        },

        components: {
            confirm,
            info,
            loading,
            offline,
        },

        created() {
            this.fullscreenLoading = true;
            this.$store.dispatch(constants.findResponseDetail, this.$props.id).then(() => {
                setTimeout(() => {
                    var message = this.$store.getters[constants.responseDetail].message;
                    if (this.$store.getters[constants.findResponseDetail] === constants.noInternet) {
                        this.$refs.info.open(constants.warning, constants.noInternet, { color: constants.error_color }).then(() => {
                            this.$router.push({ path: constants.responseList });
                        });
                    }

                    else if (message === constants.noResults) {
                        this.$refs.info.open(constants.warning, constants.recordNotExist, { color: constants.error_color }).then(() => {
                            this.$router.push({ path: constants.responseList });
                        });
                    }

                    else {
                        this.oldResponse = JSON.stringify(this.$store.getters[constants.responseDetail].data.data);
                    }

                    this.fullscreenLoading = false;

                    //load video/image all at once
                    /*var questionList = this.$store.getters[constants.responseDetail].data.data.questionList;
					if (questionList != null)
					{
                        for (var question of questionList) {
							var answerID = question.answer.answerID;
							var questionTypeID = question.questionTypeID;
							
							if (this.questionImage === questionTypeID) {
								this.getImage(answerID);
								continue;
							}
							
							if (this.questionVideo === questionTypeID) {
								this.getVideo(answerID);
							}
						}
					}*/
                }, 1000);
            });
        },

        methods: {
           
            back() {
                if (this.status === false) {
                    this.handleConnectivityChange(this.status)
                }
                else {
                    if (JSON.stringify(this.$store.getters[constants.responseDetail].data.data) !== this.oldResponse) {
                        this.$refs.confirm.open(constants.confirm, constants.cancelConfirm, { color: constants.modal_color }).then((confirm) => {
                            if (confirm) {
                                this.$router.push({ path: constants.responseList });
                            }
                        });
                    }
                    else {
                        this.$router.push({ path: constants.responseList });
                    } 
                }
            },

            save() {
                if (JSON.stringify(this.$store.getters[constants.responseDetail].data.data) !== this.oldResponse) {
                    this.$refs.form.validate();
                    if (this.valid) {
                        if (this.status === false) {
                            this.handleConnectivityChange(this.status)
                        }
                        else {
                            this.$refs.confirm.open(constants.confirm, constants.saveConfirm, { color: constants.modal_color }).then((confirm) => {
                                if (confirm) {
                                    this.fullscreenLoading = true;
                                    setTimeout(() => {
                                        this.addToModel();
                                        this.$store.dispatch(constants.editResponseDetail).then(() => {
                                            var message = this.$store.getters[constants.responseLists].message;

                                            if (this.$store.getters[constants.responseLists] === constants.noInternet) {
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
                                            else if (message === constants.saved) {
                                                this.$refs.info.open(constants.message, message, { color: constants.modal_color }).then(() => {
                                                    this.$router.push({ path: constants.responseList });
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
                }
                else {
                    this.$refs.info.open(constants.message, constants.saved, { color: constants.modal_color }).then(() => {
                        this.$router.push({ path: constants.responseList });
                    });
                }
            },
           
            addToModel() {
                var responseModel = this.$store.getters[constants.responseDetail].data.data;

                this.$store.getters[constants.responseDetail].data.data.responseID = responseModel.responseID;
                this.$store.getters[constants.responseDetail].data.data.templateID = responseModel.templateID;
                this.$store.getters[constants.responseDetail].data.data.userID = responseModel.userID;
                this.$store.getters[constants.responseDetail].data.data.companyID = responseModel.companyID;
                this.$store.getters[constants.responseDetail].data.data.branchID = responseModel.branchID;
                this.$store.getters[constants.responseDetail].data.data.status = responseModel.status;
                this.$store.getters[constants.responseDetail].data.data.remarks = responseModel.remarks;
                this.$store.getters[constants.responseDetail].data.data.isApproved = responseModel.isApproved;
            },

            validateField(v) {
                if (!v) return true;
                else if (!(validators.textFormat).test(v)) {
                    return constants.invalidInput;
                }
                else {
                    return true;
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
                this.$store.dispatch(constants.clearResponse);
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
            },

            getVideo(answerID) {
                //display video spinner
                var elList = document.getElementsByClassName("videoDiv");
                var spinnerList = document.getElementsByClassName("spinner-video");

                var spinnerEl = null;

                for (var i = 0; i < elList.length; i++) {
                    if (elList[i].dataset['answerid'] == answerID) {
                        spinnerEl = spinnerList[i];
                        break;
                    }
                }
                if (spinnerEl != null) {
                    spinnerEl.style.display = "block";
                }

                var vidConnection = new HubConnectionBuilder().withUrl(constants.videoURL).build();
                vidConnection.serverTimeoutInMilliseconds = 600000;
                vidConnection.start().then(function () {
                    vidConnection.invoke(constants.getVideo, answerID).catch(function (err) {
						console.error(err.toString());
					});
				}).catch(function (err) {
					console.error(err.toString());
				});

                vidConnection.on(constants.receiveVideo, function (vidSrc, answerID) {
                    var elList = document.getElementsByClassName("videoDiv");
                    var spinnerList = document.getElementsByClassName("spinner-video");
                    
                    var parentEl = null;
                    var spinnerEl = null;
					
					for (var i = 0; i < elList.length; i++) {
						if (elList[i].dataset['answerid'] == answerID) {
                            parentEl = elList[i];
                            spinnerEl = spinnerList[i];
							break;
						}
					}
                    if (parentEl != null) {
						var el = parentEl.getElementsByTagName("video")[0].getElementsByTagName("source")[0];
                        var currentSrc = el.getAttribute("src");
                        el.setAttribute("src", currentSrc + vidSrc);

                        parentEl.getElementsByTagName("video")[0].style.display = "block";
                        spinnerEl.style.display = "none";

                        parentEl.innerHTML = parentEl.innerHTML;
                       
					}
				});
            },
			
            getImage(answerID) {
                var elList = document.getElementsByClassName("imageDiv");
                var spinnerList = document.getElementsByClassName("spinner-image");

                var spinnerEl = null;
                
                for (var i = 0; i < elList.length; i++) {
                    if (elList[i].dataset['answerid'] == answerID) {
                        spinnerEl = spinnerList[i];
                        break;
                    }
                }
                if (spinnerEl != null) {
                    spinnerEl.style.display = "block";
                }

                var imgConnection = new HubConnectionBuilder().withUrl(constants.imageURL).build();
                imgConnection.start().then(function () {
                    imgConnection.invoke(constants.getImage, answerID).catch(function (err) {
						console.error(err.toString());
					});
				}).catch(function (err) {
					console.error(err.toString());
				});

                imgConnection.on(constants.receiveImage, function (imgSrc, answerID) {
                    var elList = document.getElementsByClassName("imageDiv");
                    var spinnerList = document.getElementsByClassName("spinner-image");
                    var el = null;
                    var spinnerEl = null;
					
					for (var i = 0; i < elList.length; i++) {
						if (elList[i].dataset['answerid'] == answerID) {
                            el = elList[i];
                            spinnerEl = spinnerList[i];
							break;
						}
					}
					if (el != null) {
						el = el.getElementsByTagName("img")[0];
						var currentSrc = el.getAttribute("src");
                        el.setAttribute("src", currentSrc + imgSrc);
                        el.style.display = "block";
                        spinnerEl.style.display = "none";
                        el.innerHTML = el.innerHTML;
					}
				});
            },
      
        },

        props: {
            id: String
        },

        computed: {
            ...mapGetters({
                model: constants.responseDetail,
            }),
        }

    }

</script>

<style>
</style>

