using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MobileJO.Data;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Response;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MobileJO.Domain.Services
{
    public class ResponseService : IResponseService
    {
        private readonly IResponseRepository _responseRepository;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ResponseService(IResponseRepository responseRepository, IMapper mapper, IHostingEnvironment hostingEnvironment)
        {
            _responseRepository = responseRepository;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        ///     Calls User repository method Find().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ListViewModel Find(int id)
        {
            return _responseRepository.Find(id);
        }

        /// <summary>
        ///     Calls Response repository method SearchByResponseID().
        /// </summary>
        /// <param name="responseID"></param>
        /// <returns></returns>
        public object SearchByResponseID(int responseID, int templateID, int userID)
        {
            return _responseRepository.SearchByResponseID(responseID, templateID, userID);
        }

        /// <summary>
        ///     Calls Response repository method Search().
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel Search(ResponseSearchViewModel searchModel, string companyID)
        {
            return _responseRepository.Search(searchModel, companyID);
        }

        /// <summary>
        ///     Calls Response repository method SearchByTemplateID().
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel SearchByTemplateID(ResponseSearchViewModel searchModel)
        {
            return _responseRepository.SearchByTemplateID(searchModel);
        }

        public ResponseAnswerViewModel CreateUpdateResponse(ResponseAnswerViewModel responseAnswer, string checkpointAnswersPath)
        {
            Response response = new Response()
            {
                ResponseID = responseAnswer.ResponseID,
                TemplateID = responseAnswer.TemplateID,
                UserID = responseAnswer.UserID,
                BranchID = responseAnswer.BranchID,
                CompanyID = responseAnswer.CompanyID,
                DateSubmitted = responseAnswer.DateSubmitted,
                Status = responseAnswer.Status,
                CreatedDate = responseAnswer.CreatedDate,
                CreatedBy = responseAnswer.CreatedBy,
                UpdatedDate = responseAnswer.UpdatedDate,
                UpdatedBy = responseAnswer.UpdatedBy,
                LastSyncDate = responseAnswer.LastSyncDate
            };

            return _responseRepository.CreateUpdateResponse(response, responseAnswer.AnswerList);
        }

        /// <summary>
        ///     Uploads the media to the server.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="checkpointAnswersPath"></param>
        /// <returns></returns>
        public string UploadMedia(IFormFileCollection files, string checkpointAnswersPath)
        {
            var rootPath = _hostingEnvironment.ContentRootPath;

            var filePath = string.Format(Constants.CheckpointAnswers.FullFilePath, rootPath, checkpointAnswersPath);
            foreach (IFormFile file in files)
            {
                string fileName = file.FileName;
                fileName = string.IsNullOrEmpty(fileName) ?
                    fileName
                    : fileName.Contains(Constants.CheckpointAnswers.ImagePrefix) ?
                        fileName.Substring(fileName.IndexOf(Constants.CheckpointAnswers.ImagePrefix))
                        : fileName.Substring(fileName.IndexOf(Constants.CheckpointAnswers.VideoPrefix));
                filePath = filePath + fileName;

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
            }

            return filePath;
        }

        /// <summary>
        ///     Used to generate an excel format of retrieved response summary records
        /// </summary>
        /// <param name="searchModel">Holds the response summary search filters</param>
        /// <returns name="responseSummary">Holds the excel formatted response summary records</returns>
        public HttpResponseMessage DownloadResponseSummary(ResponseSearchViewModel searchModel, string companyID)
        {
            var responseList = _responseRepository.DownloadResponseSummary(searchModel, companyID);
            var responseSummary = new HttpResponseMessage();

            if (responseList == null || responseList.Count == 0)
            {
                return responseSummary = Helper.ComposeResponse(HttpStatusCode.NoContent, Constants.Common.NoRecordsFound);
            }


            var exportTable = new StringBuilder();
            var rows = new StringBuilder();

            foreach (var response in responseList)
            {
                rows.Append(String.Format(Constants.Response.ResponseExcelTableRows,
                                          response.Title,
                                          response.Company + " - " + response.Branch,
                                          response.Category,
                                          response.DateSubmitted.ToShortDateString(),
                                          response.SubmittedBy,
                                          response.Status));
            }

            exportTable.Append(String.Format(Constants.Response.ExcelTableResponse, Constants.Response.ResponseExcelTableHeaders, rows));

            responseSummary = Helper.ExportToExcel(exportTable.ToString(),
                String.Format(Constants.Reports.InitialExcelFilename, Constants.Response.ResponseReport));


            return responseSummary;
        }

        /// <summary>
        ///     Used to generate an excel format of retrieved response summary records
        /// </summary>
        /// <param name="searchModel">Holds the response summary search filters</param>
        /// <returns name="responseSummary">Holds the excel formatted response summary records</returns>
        public ListViewModel DownloadResponseSummaryPDF(ResponseSearchViewModel searchModel, string companyID)
        {
            return _responseRepository.DownloadResponseSummaryPDF(searchModel, companyID);
        }

        /// <summary>
        ///     Calls User repository method UpdateResponse().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public void UpdateResponse(ResponseEditViewModel responseViewModel)
        {
            _responseRepository.UpdateResponse(responseViewModel);
        }

        /// <summary>
        ///     Calls User repository method GetMedia().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public byte[] GetMedia(int answerID)
        {
            return _responseRepository.GetMedia(answerID);
        }

        public List<Data.ViewModels.Questionnaire.ResponseAnswerDetailsViewModel> RetrieveResponseQuestionAnswer(int templateID, int userID)
        {
            return _responseRepository.RetrieveResponseQuestionAnswer(templateID, userID);
        }
    }
}
