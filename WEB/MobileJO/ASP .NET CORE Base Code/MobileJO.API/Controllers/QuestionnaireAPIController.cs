using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobileJO.Data;
using MobileJO.Data.ViewModels.Questionnaire;
using MobileJO.Domain.Contracts;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using Constants = MobileJO.Data.Constants;

namespace MobileJO.API.Controllers
{
    [Authorize(AuthenticationSchemes = Constants.Common.Bearer, Roles = Constants.Roles.Administrator)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionnaireAPIController : ControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IQuestionnaireService and IMapper
        /// </summary>
        /// <param name="questionnaireService"></param>
        /// <param name="mapper"></param>
        public QuestionnaireAPIController(IQuestionnaireService questionnaireService, IMapper mapper)
        {
            _questionnaireService = questionnaireService;
            _mapper = mapper;
        }

        [HttpGet]
        [ActionName(Constants.Questionnaire.GetQuestionTypes)]
        public HttpResponseMessage GetQuestionTypes()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var result = _questionnaireService.GetAllQuestionTypes();

                responseData = new { message = Constants.Common.NoResults };

                if (null != result)
                {
                    responseData = new { message = Constants.Common.Success, data = result };
                }
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles web-api call to add a new choice
        /// </summary>
        /// <param name="choiceDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("add_choice")]
        public HttpResponseMessage AddChoice(ChoiceDetailsViewModel choiceDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int createdID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            try
            {
                choiceDetails.CreatedBy = createdID;
                choiceDetails.UpdatedBy = createdID;

                if (0 < _questionnaireService.CreateChoice(choiceDetails))
                {
                    responseData = new { message = Constants.Common.SuccessSave };
                }
            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }



        /// <summary>
        ///     Handles web-api calls to update a template
        /// </summary>
        /// <param name="questionnaireDetails"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("edit")]
        public HttpResponseMessage EditTemplateAsync(QuestionnaireDetailsViewModel questionnaireDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int updatedByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            try
            {
                questionnaireDetails.UpdatedBy = updatedByID;

                _questionnaireService.UpdateTemplate(questionnaireDetails);
                responseData = new { message = Constants.Common.SuccessSave };

            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                responseData = new { message = ex.ParamName };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        /// Handles web-api calls to get the list of templates/questionnaires
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("list")]
        public HttpResponseMessage GetTemplateList([FromQuery] QuestionnaireSearchViewModel searchModel)
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                searchModel.UserTypeID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.UserTypeID).Value);

                if (string.IsNullOrEmpty(searchModel.CompanyID) && (int)Constants.UserType.SuperAdmin != searchModel.UserTypeID)
                {
                    searchModel.CompanyID = claimsIdentity.FindFirst(Constants.ClaimTypes.CompanyID).Value;
                }
                if (string.IsNullOrEmpty(searchModel.BranchID) && (int)Constants.UserType.Employee == searchModel.UserTypeID)
                {
                    searchModel.BranchID = claimsIdentity.FindFirst(Constants.ClaimTypes.BranchID).Value;
                }

                var result = _questionnaireService.Search(searchModel);

                responseData = new { message = Constants.Common.NoResults };

                if (result != null)
                {
                    responseData = new { message = Constants.Common.Success, data = result };
                }

                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///    Handle web-api call to delete template/questionnaire
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName(Constants.Questionnaire.Delete)]
        public HttpResponseMessage Delete(int id)
        {
            var responseData = new Object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int updateByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

                _questionnaireService.DeleteTemplate(id, updateByID);
                responseData = new { message = Constants.Common.SuccessDelete };

            }

            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                responseData = new { message = ex.ParamName };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);

        }

        /// <summary>
        ///     Handles web-api calls to create a new template
        /// </summary>
        /// <param name="questionnaireDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("add")]
        public HttpResponseMessage CreateTemplateAsync(QuestionnaireDetailsViewModel questionnaireDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int createdID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            try
            {
                questionnaireDetails.CreatedBy = createdID;
                questionnaireDetails.UpdatedBy = createdID;

                if (0 < _questionnaireService.Create(questionnaireDetails))
                {
                    responseData = new { message = Constants.Common.SuccessSave };
                }
            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                responseData = new { message = ex.ParamName };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = Constants.Common.ErrorSave };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles web-api calls to get the list of branches
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(Constants.Questionnaire.GetBranches)]
        public HttpResponseMessage GetAllBranches()
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var result = _questionnaireService.GetAllBranches();

                responseData = new { message = Constants.Common.NoResults };

                if (null != result)
                {
                    responseData = new { message = Constants.Common.Success, data = result };
                }
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles web-api calls to get the details of a template
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("find")]
        public HttpResponseMessage GetTemplateDetails(int id)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;
                var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
                int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

                var result = _questionnaireService.Find(id, userID);

                responseData = new { message = Constants.Common.Success, data = result };

            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }
            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles web-api calls to add a question
        /// </summary>
        /// <param name="questionDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("add_question")]
        public HttpResponseMessage AddQuestion(QuestionDetailsViewModel questionDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int createdID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            try
            {
                questionDetails.CreatedBy = createdID;
                questionDetails.UpdatedBy = createdID;

                if (0 < _questionnaireService.CreateQuestion(questionDetails))
                {
                    responseData = new { message = Constants.Common.SuccessSave };
                }
            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles web-api call to update a question
        /// </summary>
        /// <param name="questionDetails"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("edit_question")]
        public HttpResponseMessage EditQuestion(QuestionDetailsViewModel questionDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int updatedByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            try
            {
                questionDetails.UpdatedBy = updatedByID;

                _questionnaireService.UpdateQuestion(questionDetails);
                responseData = new { message = Constants.Common.SuccessSave };

            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                responseData = new { message = ex.ParamName };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles web-api calls to delete a question
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ActionName("delete_question")]
        public HttpResponseMessage DeleteQuestion(int id)
        {
            var responseData = new Object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int updateByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

                _questionnaireService.DeleteQuestion(id, updateByID);
                responseData = new { message = Constants.Common.SuccessDelete };

            }

            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);

        }

        [HttpDelete]
        [ActionName("delete_choice")]
        public HttpResponseMessage DeleteChoice(int id)
        {
            var responseData = new Object();
            HttpStatusCode responseCode = HttpStatusCode.OK;

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int updateByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

                _questionnaireService.DeleteChoice(id, updateByID);
                responseData = new { message = Constants.Common.SuccessDelete };

            }

            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);

        }
        
        /// <summary>
        ///     Handles web-api call to update a choice
        /// </summary>
        /// <param name="choiceDetails"></param>
        /// <returns></returns>
        [HttpPut]
        [ActionName("edit_choice")]
        public HttpResponseMessage EditChoice(ChoiceDetailsViewModel choiceDetails)
        {
            HttpStatusCode responseCode = HttpStatusCode.OK;
            var responseData = new object();

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var name = claimsIdentity.FindFirst(Constants.ClaimTypes.UserName).Value;
            int updatedByID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

            try
            {
                choiceDetails.UpdatedBy = updatedByID;

                _questionnaireService.UpdateChoice(choiceDetails);
                responseData = new { message = Constants.Common.SuccessSave };

            }
            catch (MissingFieldException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (InvalidDataException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (ArgumentOutOfRangeException ex)
            {
                responseData = new { message = ex.ParamName };
            }
            catch (NullReferenceException ex)
            {
                responseData = new { message = ex.Message };
            }
            catch (Exception ex)
            {
                responseCode = HttpStatusCode.BadRequest;
                responseData = new { message = ex.Message };
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        [HttpGet]
        [ActionName(Constants.Questionnaire.GetAllTemplatesMobile)]
        public HttpResponseMessage GetTemplateList()
        {
            var responseCode = new HttpStatusCode();
            var responseData = new object();

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int userID = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);

                responseData = _questionnaireService.GetAllTemplates(userID);
                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }

        /// <summary>
        ///     Handles QuestionnaireAPI/SyncResponseAndAnswerDetails web-api call to sync response and answer details.
        /// </summary>
        /// <param name="responseAnswerDetailsViewModel">Holds response and answer details request data</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(Constants.Questionnaire.SyncResponseAndAnswer)]
        public HttpResponseMessage SyncResponseAndAnswerDetails(ResponseAnswerDetailsViewModel responseAnswerDetailsViewModel)
        {
            var responseData = new object();
            HttpStatusCode responseCode = HttpStatusCode.BadRequest;

            try
            {
                var claimsIdentity = User.Identity as ClaimsIdentity;

                int id = Convert.ToInt32(claimsIdentity.FindFirst(Constants.ClaimTypes.ID).Value);
                DateTime currentDateTime = DateTime.Now;

                responseAnswerDetailsViewModel.CreatedBy = id;
                responseAnswerDetailsViewModel.UpdatedBy = id;
                responseAnswerDetailsViewModel.UserID = id;
                responseAnswerDetailsViewModel.DateSubmitted = currentDateTime;
                responseAnswerDetailsViewModel.CreatedDate = currentDateTime;
                responseAnswerDetailsViewModel.UpdatedDate = currentDateTime;
                responseAnswerDetailsViewModel.LastSyncDate = currentDateTime;

                responseData = _questionnaireService.SyncResponseAndAnswer(responseAnswerDetailsViewModel);
                responseCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Helper.GetErrors(ex, out responseCode, out responseData);
            }

            return Helper.ComposeResponse(responseCode, responseData);
        }
    }
}