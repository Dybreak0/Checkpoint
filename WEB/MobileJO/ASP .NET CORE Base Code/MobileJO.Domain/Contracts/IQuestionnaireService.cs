using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Questionnaire;
using System.Collections.Generic;

namespace MobileJO.Domain.Contracts
{
    public interface IQuestionnaireService
    {
        QuestionnaireViewModel Find(int id, int userID);
        List<Branch> GetAllBranches();
        List<QuestionType> GetAllQuestionTypes();
        int CreateChoice(ChoiceDetailsViewModel choiceDetails);
        void UpdateTemplate(QuestionnaireDetailsViewModel questionnaireDetails);
        ListViewModel Search(QuestionnaireSearchViewModel searchModel);
        void DeleteTemplate(int id, int updatedByID);
        int Create(QuestionnaireDetailsViewModel questionnaireDetails);
        int CreateQuestion(QuestionDetailsViewModel questionDetails);
        void UpdateQuestion(QuestionDetailsViewModel questionDetails);
        void DeleteQuestion(int id, int updatedByID);
        void UpdateChoice(ChoiceDetailsViewModel choiceDetails);
        void DeleteChoice(int id, int updatedByID);
        List<QuestionnaireViewModelMobile> GetAllTemplates(int userID);
        ResponseAnswerDetailsViewModel SyncResponseAndAnswer(ResponseAnswerDetailsViewModel responseAnswerDetails);
    }
}
