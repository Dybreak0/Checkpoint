using Microsoft.AspNetCore.Http;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Response;
using System.Collections.Generic;
using System.Net.Http;

namespace MobileJO.Domain.Contracts
{
    public interface IResponseService
    {
        ListViewModel Find(int responseID);
        ListViewModel Search(ResponseSearchViewModel searchModel, string companyID);
        object SearchByResponseID(int responseID, int templateID, int userID);
        ListViewModel SearchByTemplateID(ResponseSearchViewModel searchModel);
        ResponseAnswerViewModel CreateUpdateResponse(ResponseAnswerViewModel responseAnswer, string checkpointAnswersPath);
        HttpResponseMessage DownloadResponseSummary(ResponseSearchViewModel searchModel, string companyID);
        ListViewModel DownloadResponseSummaryPDF(ResponseSearchViewModel searchModel, string companyID);
        void UpdateResponse(ResponseEditViewModel responseViewModel);
        byte[] GetMedia(int answerID);
        List<Data.ViewModels.Questionnaire.ResponseAnswerDetailsViewModel> RetrieveResponseQuestionAnswer(int templateID, int userID);
        string UploadMedia(IFormFileCollection files, string checkpointAnswersPath);
    }
}
