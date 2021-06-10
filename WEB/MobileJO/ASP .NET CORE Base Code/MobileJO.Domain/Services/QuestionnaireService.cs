using AutoMapper;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Questionnaire;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.Domain.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly IQuestionnaireRepository _questionnaireRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IQuestionnaireRepository, IUserRepository and IMapper
        /// </summary>
        /// <param name="questionnaireRepository"></param>
        /// <param name="userRepository"></param>
        /// <param name="mapper"></param>
        public QuestionnaireService(IQuestionnaireRepository questionnaireRepository, IUserRepository userRepository, IMapper mapper)
        {
            _questionnaireRepository = questionnaireRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Calls Questionnaire Repository method FindTemplate()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public QuestionnaireViewModel Find(int id, int userID)
        {
            QuestionnaireViewModel questionnaireViewModel = null;


            if (Helper.isNumberNullOrZero(id))
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);
            }
            var userDetails = _userRepository.Find(userID);

            if (null == userDetails || !userDetails.IsActive)
            {
                throw new InvalidDataException(Constants.Common.deletedUser);
            }

            else if (1 != userDetails.RoleID)
            {
                throw new InvalidDataException(Constants.Common.NotAdmin);
            }

            var questionnaire = _questionnaireRepository.FindTemplate(id);

            if (null == questionnaire || questionnaire.IsDeleted)
            {
                throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
            }

            questionnaireViewModel = _mapper.Map<QuestionnaireViewModel>(questionnaire);

            return questionnaireViewModel;
        }

        /// <summary>
        ///     Call Questionniare Repository method DeleteTemplate()
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedByID"></param>
        public void DeleteTemplate(int id, int updatedByID)
        {
            var questionnaire = this.Find(id, updatedByID);

            if (null == questionnaire)
            {
                return;
            }

            var userDetails = _userRepository.Find(updatedByID);

            if (null == userDetails || !userDetails.IsActive)
            {
                throw new InvalidDataException(Constants.Common.deletedUser);
            }
            else if (1 != userDetails.RoleID)
            {
                throw new InvalidDataException(Constants.Common.NotAdmin);
            }

            var currentDateTime = DateTime.Parse(DateTime.Now.ToString(Constants.Common.DateOnlyFormat), CultureInfo.InvariantCulture);

            if ((0 < id) && (questionnaire.EndDate >= currentDateTime && currentDateTime >= questionnaire.StartDate))
            {
                throw new ArgumentOutOfRangeException(Constants.Questionnaire.CannotDeleteActiveTemplate);
            }

            _questionnaireRepository.DeleteTemplate(id, updatedByID);
        }

        /// <summary>
        ///     Calls Questionnaire Repository method GetAllBranches()
        /// </summary>
        /// <returns></returns>
        public List<Branch> GetAllBranches()
        {
            return _questionnaireRepository.GetAllBranches();
        }

        /// <summary>
        ///     Call Questionniare Repository method Search()
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel Search(QuestionnaireSearchViewModel searchModel)
        {
            var result = _questionnaireRepository.Search(searchModel);

            return result;
        }

        /// <summary>
        ///     Calls Questionnaire Repository method Create()
        /// </summary>
        /// <param name="questionnaireDetails"></param>
        /// <returns></returns>
        public int Create(QuestionnaireDetailsViewModel questionnaireDetails)
        {
            int questionnaireId = 0;

            try
            {

                isValid(questionnaireDetails, true);

                var currentDateTime = DateTime.Now;

                Template template = new Template
                {
                    TemplateID = questionnaireDetails.TemplateID,
                    Title = questionnaireDetails.Title,
                    Description = questionnaireDetails.Description,
                    Category = questionnaireDetails.Category,
                    StartDate = questionnaireDetails.StartDate,
                    EndDate = questionnaireDetails.EndDate,
                    MaxLimit = questionnaireDetails.MaxLimit,
                    CreatedDate = currentDateTime,
                    CreatedBy = questionnaireDetails.CreatedBy,
                    UpdatedDate = currentDateTime,
                    UpdatedBy = questionnaireDetails.UpdatedBy
                };

                questionnaireId = _questionnaireRepository.Create(template, questionnaireDetails.BranchIds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return questionnaireId;
        }

        public void isValid(QuestionnaireDetailsViewModel questionnaireDetails, bool isAdd)
        {
            if (String.IsNullOrEmpty(questionnaireDetails.Title) || String.IsNullOrEmpty(questionnaireDetails.Description)
                || String.IsNullOrEmpty(questionnaireDetails.Category) || (Helper.isNumberNullOrZero(questionnaireDetails.MaxLimit))
                || null == questionnaireDetails.StartDate || null == questionnaireDetails.EndDate
                || (!isAdd && Helper.isNumberNullOrZero(questionnaireDetails.TemplateID))
                )
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);

            }

            if (questionnaireDetails.EndDate < questionnaireDetails.StartDate)
            {
                throw new ArgumentOutOfRangeException(Constants.Common.StartDateGreaterThanEndDate);
            }

            var userID = isAdd ? questionnaireDetails.CreatedBy : questionnaireDetails.UpdatedBy;

            var userDetails = _userRepository.Find(userID);

            if (null == userDetails || !userDetails.IsActive)
            {
                throw new InvalidDataException(Constants.Common.deletedUser);
            }
            else if (1 != userDetails.RoleID)
            {
                throw new InvalidDataException(Constants.Common.NotAdmin);
            }

            if (!isAdd)
            {
                var questionnaire = _questionnaireRepository.FindTemplate(questionnaireDetails.TemplateID);

                if (null == questionnaire || questionnaire.IsDeleted)
                {
                    throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
                }

            }
        }

        /// <summary>
        ///     Calls Questionnaire Repository method Update()
        /// </summary>
        /// <param name="questionnaireDetails"></param>
        public void UpdateTemplate(QuestionnaireDetailsViewModel questionnaireDetails)
        {

            isValid(questionnaireDetails, false);

            var currentDateTime = DateTime.Now;

            Template template = new Template
            {
                TemplateID = questionnaireDetails.TemplateID,
                Title = questionnaireDetails.Title,
                Description = questionnaireDetails.Description,
                Category = questionnaireDetails.Category,
                StartDate = questionnaireDetails.StartDate,
                EndDate = questionnaireDetails.EndDate,
                MaxLimit = questionnaireDetails.MaxLimit,
                UpdatedDate = currentDateTime,
                UpdatedBy = questionnaireDetails.UpdatedBy
            };

            _questionnaireRepository.Update(template, questionnaireDetails.BranchIds);

        }

        public List<QuestionType> GetAllQuestionTypes()
        {
            return _questionnaireRepository.GetAllQuestionTypes();
        }

        /// <summary>
        ///     Calls Questionnaire Repository method CreateChoice
        /// </summary>
        /// <param name="choiceDetails"></param>
        /// <returns></returns>
        public int CreateChoice(ChoiceDetailsViewModel choiceDetails)
        {
            int id = 0;

            if (isChoiceValid(choiceDetails, true))
            {
                var currentDateTime = DateTime.Now;

                Choice choice = new Choice
                {
                    ChoiceID = choiceDetails.ChoiceID,
                    QuestionID = choiceDetails.QuestionID,
                    Label = choiceDetails.Label,
                    Value = choiceDetails.Value,
                    CreatedDate = currentDateTime,
                    CreatedBy = choiceDetails.CreatedBy,
                    UpdatedDate = currentDateTime,
                    UpdatedBy = choiceDetails.UpdatedBy
                };

                id = _questionnaireRepository.CreateChoice(choice);
            }
            else
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);
            }

            return id;
        }

        public bool isChoiceValid(ChoiceDetailsViewModel choiceDetails, bool isAdd)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(choiceDetails.Label)
                || (isAdd && Helper.isNumberNullOrZero(choiceDetails.QuestionID))
                || (!isAdd && Helper.isNumberNullOrZero(choiceDetails.ChoiceID)))
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);

            }

            var userID = isAdd ? choiceDetails.CreatedBy : choiceDetails.UpdatedBy;

            var userDetails = _userRepository.Find(userID);

            if (null == userDetails || !userDetails.IsActive)
            {
                throw new InvalidDataException(Constants.Common.deletedUser);
            }
            else if (1 != userDetails.RoleID)
            {
                throw new InvalidDataException(Constants.Common.NotAdmin);
            }

            if (!isAdd)
            {
                var questionnaire = _questionnaireRepository.FindChoice(choiceDetails.ChoiceID);

                if (null == questionnaire || questionnaire.IsDeleted)
                {
                    throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
                }

            }

            return isValid;
        }

        public bool isQuestionValid(QuestionDetailsViewModel questionDetails, bool isAdd)
        {
            bool isValid = true;

            if (String.IsNullOrEmpty(questionDetails.Qquestion)
                || (isAdd && Helper.isNumberNullOrZero(questionDetails.TemplateID))
                || (isAdd && Helper.isNumberNullOrZero(questionDetails.QuestionTypeID))
                || (!isAdd && Helper.isNumberNullOrZero(questionDetails.QuestionID)))
            {
                isValid = false;
            }

            var userID = isAdd ? questionDetails.CreatedBy : questionDetails.UpdatedBy;

            var userDetails = _userRepository.Find(userID);

            if (null == userDetails || !userDetails.IsActive)
            {
                throw new InvalidDataException(Constants.Common.deletedUser);
            }
            else
            {
                if (1 != userDetails.RoleID)
                {
                    throw new InvalidDataException(Constants.Common.NotAdmin);
                }
            }

            if (!isAdd)
            {
                var questionnaire = _questionnaireRepository.FindQuestion(questionDetails.QuestionID);

                if (null == questionnaire || questionnaire.IsDeleted)
                {
                    throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
                }

            }

            if (isAdd)
            {
                if (_questionnaireRepository.HasReachedMaxQuestions(questionDetails.TemplateID))
                {
                    throw new InvalidDataException(Constants.Common.MaximumQuestionsReached);
                }
            }


            return isValid;
        }

        /// <summary>
        ///     Calls Questionnare Repository method CreateQuestion
        /// </summary>
        /// <param name="questionDetails"></param>
        /// <returns></returns>
        public int CreateQuestion(QuestionDetailsViewModel questionDetails)
        {
            int id = 0;

            if (isQuestionValid(questionDetails, true))
            {
                var currentDateTime = DateTime.Now;

                Question question = new Question
                {
                    QuestionID = questionDetails.QuestionID,
                    QuestionTypeID = questionDetails.QuestionTypeID,
                    TemplateID = questionDetails.TemplateID,
                    Qquestion = questionDetails.Qquestion,
                    CreatedDate = currentDateTime,
                    CreatedBy = questionDetails.CreatedBy,
                    UpdatedDate = currentDateTime,
                    UpdatedBy = questionDetails.UpdatedBy
                };

                id = _questionnaireRepository.CreateQuestion(question);
            }
            else
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);
            }

            return id;
        }

        /// <summary>
        ///     Calls Questionnaire Repository method UpdateQuestion
        /// </summary>
        /// <param name="questionDetails"></param>
        public void UpdateQuestion(QuestionDetailsViewModel questionDetails)
        {
            if (isQuestionValid(questionDetails, false))
            {
                var currentDateTime = DateTime.Now;

                Question question = new Question
                {
                    QuestionID = questionDetails.QuestionID,
                    Qquestion = questionDetails.Qquestion,
                    UpdatedDate = currentDateTime,
                    UpdatedBy = questionDetails.UpdatedBy
                };

                _questionnaireRepository.UpdateQuestion(question);
            }
            else
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);
            }
        }

        /// <summary>
        ///     Calls Questionnaire repository DeleteQuestion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedByID"></param>
        public void DeleteQuestion(int id, int updatedByID)
        {
            var question = _questionnaireRepository.FindQuestion(id);

            if (null == question)
            {
                throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
            }
            else
            {
                var userDetails = _userRepository.Find(updatedByID);

                if (null == userDetails || !userDetails.IsActive)
                {
                    throw new InvalidDataException(Constants.Common.deletedUser);
                }
                else
                {
                    if (1 == userDetails.RoleID)
                    {
                        if (0 < id)
                        {
                            var currentDateTime = DateTime.Now;

                            _questionnaireRepository.DeleteQuestion(id, updatedByID);

                        }
                    }
                    else
                    {
                        throw new InvalidDataException(Constants.Common.NotAdmin);
                    }
                }
            }

        }
       
        /// <summary>
        ///     Calls Questionnaire repository method DeleteChoice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedByID"></param>
        public void DeleteChoice(int id, int updatedByID)
        {
            var question = _questionnaireRepository.FindQuestion(id);

            if (null == question)
            {
                throw new NullReferenceException(Constants.Common.RecordDoesNotExist);
            }
            else
            {
                var userDetails = _userRepository.Find(updatedByID);

                if (null == userDetails || !userDetails.IsActive)
                {
                    throw new InvalidDataException(Constants.Common.deletedUser);
                }
                else
                {
                    if (1 == userDetails.RoleID)
                    {
                        if (0 < id)
                        {
                            var currentDateTime = DateTime.Now;

                            _questionnaireRepository.DeleteQuestion(id, updatedByID);

                        }
                    }
                    else
                    {
                        throw new InvalidDataException(Constants.Common.NotAdmin);
                    }
                }
            }

        }
        
        public void UpdateChoice(ChoiceDetailsViewModel choiceDetails)
        {
            if (isChoiceValid(choiceDetails, false))
            {
                var currentDateTime = DateTime.Now;

                Choice choice = new Choice
                {
                    ChoiceID = choiceDetails.ChoiceID,
                    Label = choiceDetails.Label,
                    Value = choiceDetails.Value,
                    UpdatedDate = currentDateTime,
                    UpdatedBy = choiceDetails.UpdatedBy
                };

                _questionnaireRepository.UpdateChoice(choice);
            }
            else
            {
                throw new MissingFieldException(Constants.Common.MissingRequiredField);
            }
        }

        public List<QuestionnaireViewModelMobile> GetAllTemplates(int userID)
        {
            return _questionnaireRepository.GetAllTemplates(userID);
        }

        public ResponseAnswerDetailsViewModel SyncResponseAndAnswer(ResponseAnswerDetailsViewModel responseAnswerDetails)
        {
            try
            {
                Response response = new Response();

                response.ResponseID = responseAnswerDetails.ResponseID;
                response.TemplateID = responseAnswerDetails.TemplateID;
                response.UserID = responseAnswerDetails.UserID;
                response.BranchID = responseAnswerDetails.BranchID;
                response.CompanyID = responseAnswerDetails.CompanyID;
                response.DateSubmitted = responseAnswerDetails.DateSubmitted;
                response.Remarks = responseAnswerDetails.Remarks;
                response.Status = responseAnswerDetails.Status;
                response.CreatedDate = responseAnswerDetails.CreatedDate;
                response.CreatedBy = responseAnswerDetails.CreatedBy;
                response.UpdatedDate = responseAnswerDetails.UpdatedDate;
                response.UpdatedBy = responseAnswerDetails.UpdatedBy;
                response.LastSyncDate = responseAnswerDetails.LastSyncDate;

                int id = 0;

                if (0 == responseAnswerDetails.ResponseID)
                {
                    id = _questionnaireRepository.CreateResponse(response);
                }
                else
                {
                    id = _questionnaireRepository.UpdateResponse(response);
                }

                responseAnswerDetails.ResponseID = id;
                if (0 < id)
                {
                    foreach (var ans in responseAnswerDetails.Answers)
                    {
                        Answer answer = new Answer();

                        DateTime currentDateTime = DateTime.Now;

                        answer.AnswerID = ans.AnswerID;
                        answer.ResponseID = id;
                        answer.TemplateID = ans.TemplateID;
                        answer.QuestionID = ans.QuestionID;
                        answer.ChoiceID = ans.ChoiceID;
                        answer.UserID = responseAnswerDetails.UserID;
                        answer.Value = ans.Value;
                        answer.DateSubmitted = currentDateTime;
                        answer.CreatedDate = currentDateTime;
                        answer.CreatedBy = responseAnswerDetails.CreatedBy;
                        answer.UpdatedBy = responseAnswerDetails.UpdatedBy;
                        answer.UpdatedDate = currentDateTime;
                        answer.LastSyncDate = currentDateTime;

                        if (0 == ans.AnswerID)
                        {
                            //create answer
                            ans.AnswerID = _questionnaireRepository.CreateAnswer(answer);
                        }
                        else
                        {
                            //update answer
                            ans.AnswerID = _questionnaireRepository.UpdateAnswer(answer);
                        }

                        answer = new Answer();

                    }
                }
                return responseAnswerDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
