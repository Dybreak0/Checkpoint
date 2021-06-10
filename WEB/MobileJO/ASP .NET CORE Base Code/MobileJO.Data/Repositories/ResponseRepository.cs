using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Questionnaire;
using MobileJO.Data.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class ResponseRepository : BaseRepository, IResponseRepository
    {
        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ResponseRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public ListViewModel Search(ResponseSearchViewModel searchModel, string companyID)
        {

            var responses = GetDbSet<Response>()
            .Include(template => template.Template)
            .Include(user => user.User)
            .Where(x => (string.IsNullOrEmpty(searchModel.Title) || x.Template.Title.Contains(searchModel.Title)) &&
                        (string.IsNullOrEmpty(searchModel.Category) || x.Template.Category.Contains(searchModel.Category)) &&
                        (x.Template.StartDate >= searchModel.StartDate && x.Template.EndDate < searchModel.EndDate.AddDays(1)) &&
                        (x.Status == true) &&
                        (string.IsNullOrEmpty(companyID) || x.CompanyID == Convert.ToInt32(companyID)))
                        .OrderByDescending(x => x.DateSubmitted);

            if (0 == searchModel.Page)
                searchModel.Page = 1;

            var totalCount = responses.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = responses.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(response => new
                {
                    id = response.ResponseID,
                    title = response.Template.Title,
                    submitted_by = response.User.FirstName + " " + response.User.LastName,
                    category = response.Template.Category,
                    date_submitted = Helper.GetFormattedDate(response.DateSubmitted),
                    status = response.Status,
                    isApproved = response.IsApproved,

                    branch = GetDbSet<Branch>()
                            .Where(a => a.BranchID == response.BranchID)
                            .Select(x => x.Name).FirstOrDefault(),

                    company_name = GetDbSet<Company>()
                            .Where(a => a.CompanyID == response.CompanyID)
                            .Select(x => x.CompanyName).FirstOrDefault(),
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        /// Find responsedetails by response id
        /// </summary>
        /// <param name="responseID"></param>
        /// <returns></returns>
        public ListViewModel Find(int responseID)
        {
            var responseDetails = GetDbSet<Response>()
                .Include(response => response.Template)
                .Where(response => response.ResponseID == responseID)
                .FirstOrDefault();

            var answerList = GetDbSet<Answer>()
                    .Where(answer => answer.ResponseID == responseID)
                    .ToList();

            var questionList = GetDbSet<Question>()
                .AsEnumerable()
                .Where(question => !question.IsDeleted && question.TemplateID == responseDetails.TemplateID)
                .Select(question => new
                {
                    questionID = question.QuestionID,
                    questionTypeID = question.QuestionTypeID,
                    questionType = GetDbSet<QuestionType>()
                        .Where(questionType => questionType.QuestionTypeID == question.QuestionTypeID)
                        .Select(o => o.Type)
                        .FirstOrDefault(),
                    question = question.Qquestion,
                    choices = GetDbSet<Choice>()
                            .Where(choice => choice.QuestionID == question.QuestionID && !choice.IsDeleted).ToList()
                            .Select(choice => new {
                                choiceID = choice.ChoiceID,
                                label = choice.Label,
                                value = choice.Value,
                                isSelected = GetDbSet<Answer>()
                                    .Where(answer => answer.ResponseID == responseID && answer.QuestionID == question.QuestionID)
                                    .Select(answer => answer.ChoiceID).FirstOrDefault() == choice.ChoiceID
                            }),
                    answer = answerList
                        .Where(answer => answer.QuestionID == question.QuestionID)
                        .Select(answer => new
                        {
                            answerID = answer.AnswerID,
                            value = Helper.getMediaAnswer(answer.Question.QuestionTypeID, answer.Value),
                        })
                        .FirstOrDefault()
                })
                .ToList();


            var questionnaireDetails = new
            {
                responseID,
                templateID = responseDetails.Template.TemplateID,
                userID = responseDetails != null ? responseDetails.UserID : 0,
                companyID = responseDetails != null ? responseDetails.CompanyID : 0,
                branchID = responseDetails != null ? responseDetails.BranchID : 0,
                title = responseDetails.Template.Title,
                description = responseDetails.Template.Description,
                category = responseDetails.Template.Category,
                duration = string.Format(Constants.Common.DateOnlyFromTo, responseDetails.Template.StartDate.ToString(Constants.Common.DateOnlyFormat),
                responseDetails.Template.EndDate.ToString(Constants.Common.DateOnlyFormat)),
                maxLimit = responseDetails.Template.MaxLimit,
                status = responseDetails != null ? responseDetails.Status : false,
                isApproved = responseDetails != null ? responseDetails.IsApproved : false,
                remarks = responseDetails.Remarks,
                questionList
            };

            return new ListViewModel { Pagination = null, Data = questionnaireDetails };
        }

        /// <summary>
        ///     Used to retrieve the response details by response ID
        /// </summary>
        /// <param name="responseID"></param>
        /// <param name="templateID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public object SearchByResponseID(int responseID, int templateID, int userID)
        {
            var responseDetails = GetDbSet<Response>()
                .Include(response => response.Template)
                .Where(response => response.ResponseID == responseID)
                .FirstOrDefault();

            var templateDetails = responseDetails == null ? GetDbSet<Template>()
                .Where(template => template.TemplateID == templateID && !template.IsDeleted)
                .FirstOrDefault() : responseDetails.Template;

            var choiceList = GetDbSet<Choice>()
                .Where(choice => !choice.IsDeleted);

            var answerList = GetDbSet<Answer>()
                    .Where(answer => answer.ResponseID == responseID)
                    .ToList();

            var questionList = GetDbSet<Question>()
                .AsEnumerable()
                .Where(question => !question.IsDeleted && question.TemplateID == templateID)
                .Select(question => new
                {
                    questionID = question.QuestionID,
                    questionTypeID = question.QuestionTypeID,
                    question = question.Qquestion,
                    // for radio buttons and checkboxes
                    choices = choiceList
                        .Where(choice => choice.QuestionID == question.QuestionID)
                        .Select(choice => new
                        {
                            choiceID = choice.ChoiceID,
                            answerID = answerList == null || answerList.Count == 0 ? 0 : answerList
                                .Where(answer => answer.QuestionID == question.QuestionID && answer.ChoiceID == choice.ChoiceID)
                                .Select(answer => answer.AnswerID)
                                .FirstOrDefault(),
                            label = choice.Label,
                            value = choice.Value,
                            isSelected = answerList == null || answerList.Count == 0 ? false : answerList
                                .Where(answer => answer.QuestionID == question.QuestionID && answer.ChoiceID == choice.ChoiceID)
                                .Count() == 1
                        })
                        .ToList(),
                    // for text, image, video, slider
                    answer = answerList == null || answerList.Count == 0 ? null : answerList
                        .Where(answer => answer.QuestionID == question.QuestionID)
                        .Select(answer => new
                        {
                            answerID = answer.AnswerID,
                            value = answer.Value
                        })
                        .FirstOrDefault()
                })
                .ToList();

            var questionnaireDetails = new
            {
                responseID,
                templateID = templateDetails == null ? 0 : templateDetails.TemplateID,
                userID = responseDetails == null ? 0 : responseDetails.UserID,
                companyID = responseDetails == null ? 0 : responseDetails.CompanyID,
                branchID = responseDetails == null ? 0 : responseDetails.BranchID,
                title = templateDetails == null ? "" : templateDetails.Title,
                description = templateDetails == null ? "" : templateDetails.Description,
                maxLimit = templateDetails == null ? 0 : templateDetails.MaxLimit,
                status = responseDetails == null ? false : responseDetails.Status,
                canEditAnswer = responseDetails == null ? true : userID == responseDetails.UserID && !responseDetails.Status,
                questionList
            };

            return questionnaireDetails;
        }

        /// <summary>
        ///     Used to retrieve the list of responses by template ID
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel SearchByTemplateID(ResponseSearchViewModel searchModel)
        {
            int userTypeID = Convert.ToInt32(searchModel.UserTypeID);
            int userID = Convert.ToInt32(searchModel.UserID);
            int templateID = Convert.ToInt32(searchModel.TemplateID);

            // if logged in userType is SuperAdmin, get all responses made by all employees regardless of CompanyID and BranchID
            // if logged in userType is CompanyAdmin, get all responses made by all employees under specified company
            // if logged in useerType is Employee, get all responses made only by the employee
            var responses = GetDbSet<Response>()
                .Include(response => response.Template)
                .Where(response => response.TemplateID == templateID
                && ((int)Constants.UserType.SuperAdmin == userTypeID
                    || ((int)Constants.UserType.CompanyAdmin == userTypeID && response.CompanyID == Convert.ToInt32(searchModel.CompanyID))
                    || ((int)Constants.UserType.Employee == userTypeID && response.UserID == userID)))
                .OrderByDescending(x => x.DateSubmitted);

            int maxLimit = responses == null || responses.Count() == 0 ? 0 : responses.FirstOrDefault().Template.MaxLimit;

            int numOfAnswered = responses == null || responses.Count() == 0 ? 0 : responses
                .Where(response => response.UserID == userID
                    && response.DateSubmitted.Date.CompareTo(DateTime.Now.Date) == 0)
                .ToList()
                .Count();

            var results = responses == null || responses.Count() == 0 ? null : responses
                .Select(response => new
                {
                    responseID = response.ResponseID,
                    templateID = response.TemplateID,
                    userID = response.UserID,
                    title = response.Template.Title,
                    description = response.Template.Description,
                    dateSubmitted = response.DateSubmitted.ToString(Constants.Common.MonthNameDateFormat, CultureInfo.InvariantCulture),
                    status = response.Status,
                })
                .ToList();

            return new ListViewModel { Pagination = null, Data = new { MaxLimit = maxLimit, NumOfAnswered = numOfAnswered, Result = results } };

        }

        /// <summary>
        ///     Inserts response and answer data if not existing.
        ///     Updates response and answer data if already exists.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="answerList"></param>
        /// <returns></returns>
        public ResponseAnswerViewModel CreateUpdateResponse(Response response, List<ViewModels.Response.ResponseAnswerDetailsViewModel> answerList)
        {
            int responseID = 0;
            ResponseAnswerViewModel returnResponse = new ResponseAnswerViewModel();
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    responseID = response.ResponseID;

                    if (responseID > 0)
                    {
                        // Update existing response data
                        UpdateResponse(response);
                        UnitOfWork.SaveChanges();
                    }
                    else
                    {
                        // add new response data
                        GetDbSet<Response>().Add(response);
                        UnitOfWork.SaveChanges();
                        responseID = response.ResponseID;
                    }

                    List<Answer> answersToAdd = new List<Answer>();
                    List<int> answersToDelete = new List<int>();
                    DateTime currentDateTime = response.LastSyncDate;

                    foreach (ViewModels.Response.ResponseAnswerDetailsViewModel answerDetails in answerList)
                    {
                        if (answerDetails.QuestionTypeID == (int)Constants.QuestionType.Checkbox)
                        {
                            // delete existing answer data
                            if (answerDetails.AnswerID > 0) answersToDelete.Add(answerDetails.QuestionID);

                            // construct new answer data
                            answersToAdd.Add(ConstructNewAnswer(answerDetails, responseID, currentDateTime));
                            continue;
                        }

                        // update existing answer data
                        if (answerDetails.AnswerID > 0)
                        {
                            Answer answer = UpdateAnswer(answerDetails, currentDateTime);
                            UnitOfWork.SaveChanges();
                            continue;
                        }

                        // construct new answer data
                        answersToAdd.Add(ConstructNewAnswer(answerDetails, responseID, currentDateTime));
                    }

                    if (answersToDelete.Count > 0)
                    {
                        DeleteAnswers(responseID, answersToDelete);
                        UnitOfWork.SaveChanges();
                    }

                    if (answersToAdd.Count > 0)
                    {
                        GetDbSet<Answer>().AddRange(answersToAdd);
                        UnitOfWork.SaveChanges();
                    }

                    dbContextTransaction.Commit();

                    List<Answer> answersFinalList = FindAnswersByResponseID(responseID);
                    returnResponse = ConstructReturnResponse(response, answersFinalList);
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return null;
                }
            }

            return returnResponse;
        }

        private ResponseAnswerViewModel ConstructReturnResponse(Response response, List<Answer> answersFinalList)
        {
            ResponseAnswerViewModel returnResponse = new ResponseAnswerViewModel
            {
                ResponseID = response.ResponseID,
                TemplateID = response.TemplateID,
                UserID = response.UserID,
                BranchID = response.BranchID,
                CompanyID = response.CompanyID,
                DateSubmitted = response.DateSubmitted,
                Status = response.Status,
                CreatedDate = response.CreatedDate,
                CreatedBy = response.CreatedBy,
                UpdatedDate = response.UpdatedDate,
                UpdatedBy = response.UpdatedBy,
                LastSyncDate = response.LastSyncDate,
                AnswerList = ConstructReturnAnswerList(answersFinalList)

            };

            return returnResponse;
        }

        private List<ViewModels.Response.ResponseAnswerDetailsViewModel> ConstructReturnAnswerList(List<Answer> answersFinalList)
        {
            List<ViewModels.Response.ResponseAnswerDetailsViewModel> answerList = new List<ViewModels.Response.ResponseAnswerDetailsViewModel>();

            foreach (Answer answer in answersFinalList)
            {
                answerList.Add(new ViewModels.Response.ResponseAnswerDetailsViewModel
                {
                    AnswerID = answer.AnswerID,
                    ResponseID = answer.ResponseID,
                    TemplateID = answer.TemplateID,
                    QuestionID = answer.QuestionID,
                    ChoiceID = answer.ChoiceID,
                    UserID = answer.UserID,
                    Value = answer.Value
                });
            }

            return answerList;
        }

        private List<Answer> FindAnswersByResponseID(int responseID)
        {
            return GetDbSet<Answer>()
                .Where(answer => answer.ResponseID == responseID)
                .ToList();
        }

        private void UpdateResponse(Response response)
        {
            Response responseForUpdate = FindResponse(response.ResponseID);
            responseForUpdate.UserID = response.UserID;
            responseForUpdate.UpdatedBy = response.UpdatedBy;
            responseForUpdate.UpdatedDate = response.UpdatedDate;
            responseForUpdate.Status = response.Status;
            responseForUpdate.DateSubmitted = response.DateSubmitted;
            responseForUpdate.LastSyncDate = response.LastSyncDate;
        }

        /// <summary>
        ///     Used to retrieve a response's details by response's id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Response FindResponse(int responseID)
        {
            return GetDbSet<Response>()
                .Where(response => response.ResponseID == responseID)
                .FirstOrDefault();
        }

        private void DeleteAnswers(int responseID, List<int> answersToDelete)
        {
            List<Answer> answersForDeletion = GetDbSet<Answer>()
                .Where(answer => answer.ResponseID == responseID && answersToDelete.Contains(answer.QuestionID))
                .ToList();

            if (answersForDeletion.Count() > 0)
            {
                GetDbSet<Answer>().RemoveRange(answersForDeletion);
            }
        }

        private Answer ConstructNewAnswer(ViewModels.Response.ResponseAnswerDetailsViewModel answerDetails, int responseID, DateTime currentDateTime)
        {
            return new Answer()
            {
                AnswerID = 0,
                ResponseID = responseID,
                TemplateID = answerDetails.TemplateID,
                QuestionID = answerDetails.QuestionID,
                ChoiceID = answerDetails.ChoiceID,
                UserID = answerDetails.UserID,
                Value = answerDetails.Value,
                DateSubmitted = currentDateTime,
                CreatedDate = currentDateTime,
                CreatedBy = answerDetails.UserID,
                UpdatedDate = currentDateTime,
                UpdatedBy = answerDetails.UserID,
                LastSyncDate = currentDateTime
            };
        }

        private Answer UpdateAnswer(ViewModels.Response.ResponseAnswerDetailsViewModel answerDetails, DateTime currentDateTime)
        {
            Answer answerForUpdate = FindAnswer(answerDetails.AnswerID);
            answerForUpdate.ChoiceID = answerDetails.ChoiceID;
            answerForUpdate.UserID = answerDetails.UserID;
            answerForUpdate.Value = answerDetails.Value;
            answerForUpdate.DateSubmitted = currentDateTime;
            answerForUpdate.UpdatedDate = currentDateTime;
            answerForUpdate.UpdatedBy = answerDetails.UserID;
            answerForUpdate.LastSyncDate = currentDateTime;
            return answerForUpdate;
        }

        private Answer FindAnswer(int answerID)
        {
            return GetDbSet<Answer>()
                .Where(answer => answer.AnswerID == answerID)
                .FirstOrDefault();
        }

        /// <summary>
        /// Download response summary
        /// </summary>
        /// <param name="searchModel"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<ResponseListViewModel> DownloadResponseSummary(ResponseSearchViewModel searchModel, string companyID)
        {

            var responses = GetDbSet<Response>()
            .Include(template => template.Template)
            .Include(user => user.User)
            .Where(x => (string.IsNullOrEmpty(searchModel.Title) || x.Template.Title.Contains(searchModel.Title)) &&
                        (string.IsNullOrEmpty(searchModel.Category) || x.Template.Category.Contains(searchModel.Category)) &&
                        (x.Template.StartDate >= searchModel.StartDate && x.Template.EndDate < searchModel.EndDate.AddDays(1)) &&
                        (string.IsNullOrEmpty(companyID) || x.CompanyID == Convert.ToInt32(companyID)) &&
                        (x.Status == true))
                        .OrderByDescending(x => x.DateSubmitted);

            if (0 == searchModel.Page)
                searchModel.Page = 1;

            var totalCount = responses.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = responses.Select(response => new ResponseListViewModel
                {
                    ID = response.ResponseID,
                    Title = response.Template.Title,
                    SubmittedBy = response.User.FirstName + " " + response.User.LastName,
                    Category = response.Template.Category,
                    DateSubmitted = response.DateSubmitted,
                    Status = response.IsApproved ? "Approved" : "Submitted",
                    Branch = GetDbSet<Branch>()
                            .Where(a => a.BranchID == response.BranchID)
                            .Select(x => x.Name).FirstOrDefault(),
                    Company = GetDbSet<Company>()
                            .Where(a => a.CompanyID == response.CompanyID)
                            .Select(x => x.CompanyName).FirstOrDefault(),
                })
                .ToList();


            return results;
        }

        public ListViewModel DownloadResponseSummaryPDF(ResponseSearchViewModel searchModel, string companyID)
        {

            var responses = GetDbSet<Response>()
            .Include(template => template.Template)
            .Include(user => user.User)
            .Where(x => (string.IsNullOrEmpty(searchModel.Title) || x.Template.Title.Contains(searchModel.Title)) &&
                        (string.IsNullOrEmpty(searchModel.Category) || x.Template.Category.Contains(searchModel.Category)) &&
                        (x.Template.StartDate >= searchModel.StartDate && x.Template.EndDate < searchModel.EndDate.AddDays(1)) &&
                        (x.Status == true) &&
                        (string.IsNullOrEmpty(companyID) || x.CompanyID == Convert.ToInt32(companyID)))
                        .OrderByDescending(x => x.DateSubmitted);

            if (0 == searchModel.Page)
                searchModel.Page = 1;

            var totalCount = responses.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = responses.Select(response => new
                {
                    id = response.ResponseID,
                    title = response.Template.Title,
                    submitted_by = response.User.FirstName + " " + response.User.LastName,
                    category = response.Template.Category,
                    date_submitted = Helper.GetFormattedDate(response.DateSubmitted),
                    status = response.Status,
                    isApproved = response.IsApproved,

                branch = GetDbSet<Branch>()
                            .Where(a => a.BranchID == response.BranchID)
                            .Select(x => x.Name).FirstOrDefault(),

                    company_name = GetDbSet<Company>()
                            .Where(a => a.CompanyID == response.CompanyID)
                            .Select(x => x.CompanyName).FirstOrDefault(),
                })
                .ToList();

            var pagination = new
            {
                pages = totalPages,
                size = totalCount
            };

            return new ListViewModel { Pagination = pagination, Data = results };
        }

        /// <summary>
        ///     Used to update a response record.
        /// </summary>
        /// <param name="response"></param>
        public void UpdateResponse(ResponseEditViewModel responseViewModel)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var responseDB = FindResponse(responseViewModel.ResponseID);
                responseDB.Remarks = responseViewModel.Remarks;
                responseDB.IsApproved = responseViewModel.IsApproved;
                responseDB.UpdatedBy = responseViewModel.UpdatedBy;
                responseDB.UpdatedDate = DateTime.Now;
                UnitOfWork.SaveChanges();

                transaction.Commit();
            }
        }

        /// <summary>
        /// Return base64 equivalent of vide/image
        /// </summary>
        /// <param name="answerID"></param>
        /// <returns></returns>
        public byte[] GetMedia(int answerID)
        {
            Answer answer = GetDbSet<Answer>()
                .Where(ans => ans.AnswerID == answerID)
                .FirstOrDefault();

            if (answer == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(answer.Value))
            {
                return null;
            }

            return File.ReadAllBytes(answer.Value);
        }

        public List<ViewModels.Questionnaire.ResponseAnswerDetailsViewModel> RetrieveResponseQuestionAnswer(int templateID, int userID)
        {
            List<ViewModels.Questionnaire.ResponseAnswerDetailsViewModel> responseList = new List<ViewModels.Questionnaire.ResponseAnswerDetailsViewModel>();

            var userInfo = GetDbSet<User>()
                .Where(user => user.ID == userID)
                .FirstOrDefault();

            var responses = GetDbSet<Response>()
               .Where(response => response.TemplateID == templateID
               && ((int)Constants.UserType.SuperAdmin == userInfo.UserTypeID
                   || ((int)Constants.UserType.CompanyAdmin == userInfo.UserTypeID && response.CompanyID == Convert.ToInt32(userInfo.CompanyID))
                   || ((int)Constants.UserType.Employee == userInfo.UserTypeID && response.UserID == userID)))
               .OrderByDescending(x => x.DateSubmitted);

            foreach (var res in responses)
            {
                ViewModels.Questionnaire.ResponseAnswerDetailsViewModel respo = new ViewModels.Questionnaire.ResponseAnswerDetailsViewModel();

                var ans = GetDbSet<Answer>()
                    .Where(a => a.ResponseID == res.ResponseID)
                    .ToList();

                List<AnswerDetailsViewModel> answersList = new List<AnswerDetailsViewModel>();

                foreach (var answ in ans)
                {
                    answersList.Add(new AnswerDetailsViewModel
                    {
                        AnswerID = answ.AnswerID,
                        ResponseID = answ.ResponseID,
                        TemplateID = answ.TemplateID,
                        QuestionID = answ.QuestionID,
                        ChoiceID = answ.ChoiceID,
                        UserID = answ.UserID,
                        Value = answ.Value,
                        DateSubmitted = answ.DateSubmitted,
                        CreatedDate = answ.CreatedDate,
                        CreatedBy = answ.CreatedBy,
                        UpdatedDate = answ.UpdatedDate,
                        UpdatedBy = answ.UpdatedBy,
                        LastSyncDate = answ.LastSyncDate
                    });
                }

                responseList.Add(new ViewModels.Questionnaire.ResponseAnswerDetailsViewModel
                {
                    ResponseID = res.ResponseID,
                    TemplateID = res.TemplateID,
                    UserID = res.UserID,
                    BranchID = res.BranchID,
                    CompanyID = res.CompanyID,
                    DateSubmitted = res.DateSubmitted,
                    Remarks = res.Remarks,
                    Status = res.Status,
                    CreatedDate = res.CreatedDate,
                    CreatedBy = res.CreatedBy,
                    UpdatedDate = res.UpdatedDate,
                    UpdatedBy = res.UpdatedBy,
                    LastSyncDate = res.LastSyncDate,
                    Answers = answersList
                });

            }
            return responseList;
        }
    }
}
