using Microsoft.EntityFrameworkCore;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Data.ViewModels.Questionnaire;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class QuestionnaireRepository : BaseRepository, IQuestionnaireRepository
    {
        /// <summary>
        ///     Constructor for IUnitOfWork
        /// </summary>
        /// <param name="unitOfWork"></param>
        public QuestionnaireRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        /// <summary>
        ///     Used to retrieve a template using an id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Template Find(Template template)
        {
            Template templateDB = null;

            if (template.TemplateID > 0)
            {
                templateDB = GetDbSet<Template>().Find(template.TemplateID);
                templateDB.Title = template.Title;
                templateDB.Description = template.Description;
                templateDB.Category = template.Category;
                templateDB.StartDate = template.StartDate;
                templateDB.EndDate = template.EndDate;
                templateDB.MaxLimit = template.MaxLimit;
                templateDB.UpdatedDate = template.UpdatedDate;
                templateDB.UpdatedBy = template.UpdatedBy;
            }

            return templateDB;
        }
        /// <summary>
        ///     Used to update a template 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateBranch"></param>
        public void Update(Template template, List<int> templateBranch)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    var templateDB = Find(template);

                    //remove all existing branches in a template
                    GetDbSet<Template_Branch>()
                        .RemoveRange(GetDbSet<Template_Branch>()
                        .Where(x => x.TemplateID == template.TemplateID)
                        .ToList());

                    List<Template_Branch> templateBranches = new List<Template_Branch>();
                    foreach (int branchId in templateBranch)
                    {
                        Template_Branch temp = new Template_Branch
                        {
                            BranchID = branchId,
                            TemplateID = template.TemplateID
                        };
                        templateBranches.Add(temp);
                    }
                    GetDbSet<Template_Branch>().AddRange(templateBranches);

                    UnitOfWork.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        /// <summary>
        ///     Used to delete a template using id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedByID"></param>
        public void DeleteTemplate(int id, int updatedByID)
        {
            Template templateDB = Context.Template.Find(id);

            templateDB.UpdatedBy = updatedByID;
            templateDB.UpdatedDate = DateTime.Now;
            templateDB.IsDeleted = true;

            UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Used to retrieve a template's details using id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuestionnaireViewModel FindTemplate(int id)
        {

            return GetDbSet<Template>()
                .Where(x => x.TemplateID == id)
                .Select(z => new QuestionnaireViewModel
                {
                    TemplateID = z.TemplateID,
                    Title = z.Title,
                    Description = z.Description,
                    Category = z.Category,
                    StartDate = DateTime.Parse(Helper.GetFormattedDate(z.StartDate), CultureInfo.InvariantCulture),
                    EndDate = DateTime.Parse(Helper.GetFormattedDate(z.EndDate), CultureInfo.InvariantCulture),
                    MaxLimit = z.MaxLimit,
                    IsDeleted = z.IsDeleted,
                    CreatedDate = z.CreatedDate,
                    CreatedBy = z.CreatedBy,
                    UpdatedDate = z.UpdatedDate,
                    Respondents = GetDbSet<Template_Branch>()
                                    .Include(branch => branch.BranchID)
                                    .Select(x => new
                                    {
                                        Name = x.BranchId.Name,
                                        BranchID = x.BranchID,
                                        TemplateID = x.TemplateId.TemplateID
                                    })
                                    .Where(a => a.TemplateID == z.TemplateID)
                                    .Select(y => new Branch
                                    {
                                        Name = y.Name,
                                        BranchID = y.BranchID,
                                    })
                                    .ToList(),
                    UpdatedBy = z.UpdatedBy,
                    Questions = GetDbSet<Question>()
                                .Include(q => q.QuestionTypeId)
                                .Where(x => !x.IsDeleted && x.TemplateID == z.TemplateID)
                                .Select(x => new QuestionsViewModel
                                {
                                    Question = x.Qquestion,
                                    Choices = GetDbSet<Choice>()
                                            .Where(m => !m.IsDeleted && m.QuestionID == x.QuestionID)
                                            .Select(c => new ChoiceViewModel
                                            {
                                                Label = c.Label,
                                                ChoiceID = c.ChoiceID,
                                                QuestionID = c.QuestionID,
                                                Value = c.Value
                                            }
                                            ).ToList(),
                                    QuestionID = x.QuestionID,
                                    QuestionTypeID = x.QuestionTypeID,
                                    TemplateID = x.TemplateID,
                                    QuestionType = x.QuestionTypeId.Type
                                })
                                 .ToList()
                }).FirstOrDefault();
        }

        /// <summary>
        ///     Used to retrieve the list of templates depending on the search filter from the client side
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ListViewModel Search(QuestionnaireSearchViewModel searchModel)
        {
            List<int> templateIDs = null;
            string branchID = null;
            string companyID = null;
            string title = null;
            string category = null;
            bool isMobile = false;

            if (searchModel != null)
            {
                branchID = searchModel.BranchID;
                companyID = searchModel.CompanyID;
                title = searchModel.Title;
                category = searchModel.Category;
                isMobile = searchModel.IsMobile;
            }

            if (!string.IsNullOrEmpty(branchID))
            {
                templateIDs = GetDbSet<Template_Branch>()
                    .Where(template_branch => template_branch.BranchID == Convert.ToInt32(branchID))
                    .Select(o => o.TemplateID)
                    .ToList();
            }
            else if (!string.IsNullOrEmpty(companyID))
            {
                var branchIDs = GetDbSet<Branch>()
                    .Where(branch => branch.CompanyID == Convert.ToInt32(companyID))
                    .Select(o => o.BranchID)
                    .ToList();
                templateIDs = GetDbSet<Template_Branch>()
                    .Where(template_branch => branchIDs.Contains(template_branch.BranchID))
                    .Select(o => o.TemplateID)
                    .ToList();
            }

            DateTime now = DateTime.Now.Date;
            var templates = GetDbSet<Template>()
                 .Where(x => (x.IsDeleted == false) &&
                             (string.IsNullOrEmpty(title) || x.Title.Contains(Convert.ToString(title))) &&
                             (string.IsNullOrEmpty(category) || x.Category.Contains(Convert.ToString(category))) &&
                             (templateIDs == null || templateIDs.Contains(x.TemplateID)) &&
                             (isMobile == false || (x.StartDate.CompareTo(now) <= 0 && x.EndDate.CompareTo(now) >= 0)))
                              .OrderByDescending(x => x.UpdatedDate);

            if (0 == searchModel.Page)
                searchModel.Page = 1;

            var totalCount = templates.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / searchModel.PageSize);

            var results = templates.Skip(searchModel.PageSize * (searchModel.Page - 1))
                .Take(searchModel.PageSize)
                .AsEnumerable()
                .Select(template => new
                {
                    id = template.TemplateID,
                    title = template.Title,
                    description = template.Description,
                    respondents = string.Join(", ", GetDbSet<Template_Branch>()
                                    .Include(branch => branch.BranchID)
                                    .Select(x => new
                                    {
                                        brancheName = x.BranchId.Name,
                                        templateId = x.TemplateId.TemplateID
                                    })
                                    .Where(a => a.templateId == template.TemplateID)
                                    .ToList()
                                    .Select(o => o.brancheName)),
                    category = template.Category,
                    companyBranchNames = string.Join(", ", GetDbSet<Template_Branch>()
                                    .Select(x => new
                                    {
                                        companyName = x.BranchId.CompanyId.CompanyName,
                                        branchName = x.BranchId.Name,
                                        templateId = x.TemplateId.TemplateID
                                    })
                                    .Where(a => a.templateId == template.TemplateID)
                                    .ToList()
                                    .Select(o => o.companyName + "-" + o.branchName)),
                    createdDate = template.CreatedDate.ToString(Constants.Common.MonthNameDateFormat, CultureInfo.InvariantCulture)
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
        ///     Used to create a new template
        /// </summary>
        /// <param name="template"></param>
        /// <param name="templateBranch"></param>
        /// <returns></returns>
        public int Create(Template template, List<int> templateBranch)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int insert_id = 0;

                    GetDbSet<Template>().Add(template);
                    UnitOfWork.SaveChanges();

                    insert_id = template.TemplateID;

                    if (0 < insert_id)
                    {
                        List<Template_Branch> templateBranches = new List<Template_Branch>();
                        foreach (int branchId in templateBranch)
                        {
                            Template_Branch temp = new Template_Branch
                            {
                                BranchID = branchId,
                                TemplateID = insert_id
                            };
                            templateBranches.Add(temp);
                        }
                        GetDbSet<Template_Branch>().AddRange(templateBranches);
                        UnitOfWork.SaveChanges();

                    }
                    dbContextTransaction.Commit();
                    return insert_id;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Used to retrieve all the branches 
        /// </summary>
        /// <returns></returns>
        public List<Branch> GetAllBranches()
        {
            return GetDbSet<Branch>()
                .Where(x => x.Status)
                .ToList();
        }

        public List<QuestionType> GetAllQuestionTypes()
        {
            return GetDbSet<QuestionType>()
                .ToList();
        }
        /// <summary>
        ///     Used to create a new choice
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        public int CreateChoice(Choice choice)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int insert_id = 0;

                    GetDbSet<Choice>().Add(choice);
                    UnitOfWork.SaveChanges();

                    insert_id = choice.ChoiceID;


                    dbContextTransaction.Commit();
                    return insert_id;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }
        /// <summary>
        ///     Used to retrieve a choice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Choice FindChoice(int id)
        {
            return GetDbSet<Choice>()
                .Where(x => !x.IsDeleted && x.ChoiceID == id)
                .FirstOrDefault();
        }

        public Question FindQuestion(int id)
        {
            return GetDbSet<Question>()
                .Where(x => !x.IsDeleted && x.QuestionID == id)
                .FirstOrDefault();
        }

        /// <summary>
        ///     Used to create a new question
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        public int CreateQuestion(Question question)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int insert_id = 0;

                    GetDbSet<Question>().Add(question);
                    UnitOfWork.SaveChanges();

                    insert_id = question.QuestionID;

                    if (7 == question.QuestionTypeID)
                    {
                        List<Choice> sliders = new List<Choice>();
                        Choice choice = new Choice
                        {
                            QuestionID = insert_id,
                            Label = "1",
                            Value = "",
                            CreatedDate = question.CreatedDate,
                            CreatedBy = question.CreatedBy,
                            UpdatedDate = question.UpdatedDate,
                            UpdatedBy = question.UpdatedBy
                        };

                        sliders.Add(choice);
                        Choice choice1 = new Choice
                        {
                            QuestionID = insert_id,
                            Label = "10",
                            Value = "",
                            CreatedDate = question.CreatedDate,
                            CreatedBy = question.CreatedBy,
                            UpdatedDate = question.UpdatedDate,
                            UpdatedBy = question.UpdatedBy
                        };
                        sliders.Add(choice1);
                        GetDbSet<Choice>().AddRange(sliders);
                        UnitOfWork.SaveChanges();
                    }


                    dbContextTransaction.Commit();
                    return insert_id;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Used to update a question
        /// </summary>
        /// <param name="question"></param>
        public void UpdateQuestion(Question question)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Question questionDB = Context.Question.Find(question.QuestionID);
                    questionDB.Qquestion = question.Qquestion;

                    UnitOfWork.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        /// <summary>
        ///     Used to delete a question
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedByID"></param>
        public void DeleteQuestion(int id, int updatedByID)
        {
            Question questionDB = Context.Question.Find(id);

            questionDB.UpdatedBy = updatedByID;
            questionDB.UpdatedDate = DateTime.Now;
            questionDB.IsDeleted = true;

            UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Used to delete a choice
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedByID"></param>
        public void DeleteChoice(int id, int updatedByID)
        {
            Choice choiceDB = Context.Choice.Find(id);

            choiceDB.UpdatedBy = updatedByID;
            choiceDB.UpdatedDate = DateTime.Now;
            choiceDB.IsDeleted = true;

            UnitOfWork.SaveChanges();
        }

        /// <summary>
        ///     Used to update a choice
        /// </summary>
        /// <param name="choice"></param>
        public void UpdateChoice(Choice choice)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Choice choiceDB = Context.Choice.Find(choice.ChoiceID);
                    choiceDB.Label = choice.Label;
                    choiceDB.Value = choice.Value;

                    UnitOfWork.SaveChanges();

                    dbContextTransaction.Commit();
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                }
            }
        }

        public List<QuestionnaireViewModelMobile> GetAllTemplates(int userID)
        {
            List<int> templateIDs = null;

            var userInfo = GetDbSet<User>()
                .Where(user => user.ID == userID)
                .FirstOrDefault();

            if (((int)Constants.UserType.CompanyAdmin).Equals(userInfo.UserTypeID))
            {
                var brancIDs = GetDbSet<Branch>()
                    .Where(branch => branch.CompanyID == userInfo.CompanyID)
                    .Select(branch => branch.BranchID)
                    .ToList();

                templateIDs = GetDbSet<Template_Branch>()
                    .Where(template_branch => brancIDs.Contains(template_branch.BranchID))
                    .Select(template_branch => template_branch.TemplateID)
                    .ToList();
            }

            else if (((int)Constants.UserType.Employee).Equals(userInfo.UserTypeID))
            {
                templateIDs = GetDbSet<Template_Branch>()
                    .Where(template_branch => template_branch.BranchID == userInfo.BranchID)
                    .Select(o => o.TemplateID)
                    .ToList();
            }

            DateTime now = DateTime.Now.Date;
            List<Template> templateList = GetDbSet<Template>()
                .Where(template => !template.IsDeleted &&
                (null == templateIDs || 0 == templateIDs.Count || templateIDs.Contains(template.TemplateID)) &&
                (template.StartDate.CompareTo(now) <= 0 && template.EndDate.CompareTo(now) >= 0))
                .OrderByDescending(template => template.UpdatedDate)
                .ToList();

            List<Template_Branch> templateBranchList = GetDbSet<Template_Branch>().ToList();
            List<Branch> branchList = GetDbSet<Branch>().ToList();
            List<Company> companyList = GetDbSet<Company>().ToList();

            var companyBranchNameList = (templateBranchList.Join(branchList, a => a.BranchID, b => b.BranchID, (a, b) => new { a.TemplateID, a.BranchID, b.CompanyID, BranchName = b.Name }))
                    .Join(companyList, a => a.CompanyID, b => b.CompanyID, (a, b) => new { a.TemplateID, a.BranchID, a.CompanyID, a.BranchName, b.CompanyName });

            return templateList
                .Select(template => new QuestionnaireViewModelMobile
                {
                    TemplateID = template.TemplateID,
                    Title = template.Title,
                    Description = template.Description,
                    Category = template.Category,
                    StartDate = DateTime.Parse(Helper.GetFormattedDate(template.StartDate), CultureInfo.InvariantCulture),
                    EndDate = DateTime.Parse(Helper.GetFormattedDate(template.EndDate), CultureInfo.InvariantCulture),
                    MaxLimit = template.MaxLimit,
                    IsDeleted = template.IsDeleted,
                    CreatedDate = template.CreatedDate,
                    CreatedBy = template.CreatedBy,
                    UpdatedDate = template.UpdatedDate,
                    Template_Branches = GetDbSet<Template_Branch>()
                                           .Where(a => a.TemplateID == template.TemplateID)
                                           .ToList(),
                    Company_Branches = companyBranchNameList
                                       .Where(a => a.TemplateID == template.TemplateID)
                                       .Select(company => new Company_Branch
                                       {
                                           CompanyID = company.CompanyID,
                                           CompanyName = company.CompanyName,
                                           BranchID = company.BranchID,
                                           BranchName = company.BranchName
                                       })
                                       .ToList(),
                    UpdatedBy = template.UpdatedBy,
                    Questions = GetDbSet<Question>()
                                       .Include(q => q.QuestionTypeId)
                                       .Where(x => !x.IsDeleted && x.TemplateID == template.TemplateID)
                                       .Select(x => new QuestionsViewModel
                                       {
                                           Question = x.Qquestion,
                                           Choices = GetDbSet<Choice>()
                                                   .Where(m => !m.IsDeleted && m.QuestionID == x.QuestionID)
                                                   .Select(c => new ChoiceViewModel
                                                   {
                                                       Label = c.Label,
                                                       ChoiceID = c.ChoiceID,
                                                       QuestionID = c.QuestionID,
                                                       Value = c.Value
                                                   }
                                                   ).ToList(),
                                           QuestionID = x.QuestionID,
                                           QuestionTypeID = x.QuestionTypeID,
                                           TemplateID = x.TemplateID,
                                           QuestionType = x.QuestionTypeId.Type
                                       })
                                        .ToList()
                })
                .ToList();
        }

        public int CreateAnswer(Answer answer)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    GetDbSet<Answer>().Add(answer);
                    UnitOfWork.SaveChanges();
                    dbContextTransaction.Commit();

                    return answer.AnswerID;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int CreateResponse(Response response)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {

                    GetDbSet<Response>().Add(response);

                    UnitOfWork.SaveChanges();
                    dbContextTransaction.Commit();

                    return response.ResponseID;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int UpdateAnswer(Answer answer)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    Answer answerDB = Context.Answers.Find(answer.AnswerID);

                    answerDB.UpdatedBy = answer.UpdatedBy;
                    answerDB.UpdatedDate = answer.UpdatedDate;
                    answerDB.Value = answer.Value;
                    answerDB.ChoiceID = answer.ChoiceID;
                    answerDB.DateSubmitted = answer.DateSubmitted;
                    answerDB.LastSyncDate = answer.LastSyncDate;

                    UnitOfWork.SaveChanges();
                    dbContextTransaction.Commit();

                    return answer.AnswerID;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public int UpdateResponse(Response response)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    int id = 0;

                    Response responseDB = Context.Response.Find(response.ResponseID);

                    responseDB.UpdatedDate = response.UpdatedDate;
                    responseDB.UpdatedBy = response.UpdatedBy;
                    responseDB.LastSyncDate = response.LastSyncDate;
                    responseDB.Status = response.Status;
                    response.DateSubmitted = responseDB.DateSubmitted;

                    UnitOfWork.SaveChanges();
                    dbContextTransaction.Commit();

                    return response.ResponseID;
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    return 0;
                }
            }
        }

        public bool HasReachedMaxQuestions(int templateID)
        {
            return Constants.Questionnaire.MaxQuestionsAllowed <= GetDbSet<Question>()
                .Where(x => x.TemplateID == templateID && !x.IsDeleted)
                .Count();
        }
    }
}
