using MobileJO.Data.Models;
using MobileJO.Data.ViewModels;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Response;
using System.Collections.Generic;

namespace MobileJO.Data.Contracts
{
    public interface IResponseRepository
    {
        ListViewModel Find(int responseID);
        ListViewModel Search(ResponseSearchViewModel searchModel, string companyID);
        object SearchByResponseID(int responseID, int templateID, int userID);
        ListViewModel SearchByTemplateID(ResponseSearchViewModel searchModel);
        ResponseAnswerViewModel CreateUpdateResponse(Response response, List<ResponseAnswerDetailsViewModel> answerList);
        List<ResponseListViewModel> DownloadResponseSummary(ResponseSearchViewModel searchModel, string companyID);
        ListViewModel DownloadResponseSummaryPDF(ResponseSearchViewModel searchModel, string companyID);
        void UpdateResponse(ResponseEditViewModel responseViewModel);
        byte[] GetMedia(int answerID);
        List<ViewModels.Questionnaire.ResponseAnswerDetailsViewModel> RetrieveResponseQuestionAnswer(int templateID, int userID);
    }
}
