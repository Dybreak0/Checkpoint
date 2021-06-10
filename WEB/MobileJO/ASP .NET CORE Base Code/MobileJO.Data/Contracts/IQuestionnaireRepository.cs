using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Questionnaire;
using System;
using System.Collections.Generic;

namespace MobileJO.Data.Contracts
{
    public interface IQuestionnaireRepository
    {
        QuestionnaireViewModel FindTemplate(int id);
        List<Branch> GetAllBranches();
        void Update(Template template, List<int> templateBranch);
        int Create(Template template, List<int> templateBranch);
        ListViewModel Search(QuestionnaireSearchViewModel searchModel);
        void DeleteTemplate(int id, int updatedByID);
        int CreateChoice(Choice choice);
        Choice FindChoice(int id);
        List<QuestionType> GetAllQuestionTypes();
        Question FindQuestion(int id);
        int CreateQuestion(Question question);
        void UpdateQuestion(Question question);
        void DeleteQuestion(int id, int updatedByID);
        void UpdateChoice(Choice choice);
        void DeleteChoice(int id, int updatedByID);
        List<QuestionnaireViewModelMobile> GetAllTemplates(int userID);
        int CreateResponse(Response response);
        int CreateAnswer(Answer answer);
        int UpdateAnswer(Answer answer);
        int UpdateResponse(Response response);
        Boolean HasReachedMaxQuestions(int templateID);

    }
}
