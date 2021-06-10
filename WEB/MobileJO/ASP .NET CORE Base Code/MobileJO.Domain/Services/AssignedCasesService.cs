using AutoMapper;
using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Domain.Services
{
    public class AssignedCasesService : IAssignedCasesService
    {
        private readonly IAssignedCasesRepository _assignedCaseRepository;
        private readonly IMapper _mapper;

        /// <summary>
        ///     Constructor for IAssignedCasesRepository and IMapper
        /// </summary>
        /// <param name="assignedCasesRepository"></param>
        /// <param name="mapper"></param>
        public AssignedCasesService(IAssignedCasesRepository assignedCasesRepository, IMapper mapper)
        {
            _assignedCaseRepository = assignedCasesRepository;
            _mapper = mapper;
        }

        /// <summary>
        ///     Calls Assigned Case repository method Find().
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AssignedCasesViewModel Find(int id)
        {
            return _assignedCaseRepository.Find(id);
        }

        /// <summary>
        ///     Calls Assigned Case repository method Search().
        /// </summary>
        /// <param name="searchViewModel"></param>
        /// <returns></returns>
        public ListViewModel Search(AssignedCasesSearchViewModel searchViewModel)
        {
             return _assignedCaseRepository.Search(searchViewModel);
        }

        /// <summary>
        ///     Calls Assigned Case repository method GetApplicationTypes().
        /// </summary>
        /// <returns></returns>
        public List<string> GetApplicationTypes()
        {
            return _assignedCaseRepository.GetApplicationTypes();
        }

        /// <summary>
        ///     Calls Assigned Case repository method GetCaseStatusList().
        /// </summary>
        /// <returns></returns>
        public List<string> GetCaseStatusList()
        {
            return _assignedCaseRepository.GetCaseStatusList();
        }

        public List<AssignedCasesListViewModel> FindCases(string ids)
        {
            var caseIds = ids.Split(',').Select(int.Parse).ToList(); ;

            return _assignedCaseRepository.FindCases(caseIds);
        }

        /// <summary>
        ///     Calls Assigned Case repository method Find().
        /// </summary>
        /// <param name="CaseNumber"></param>
        /// <returns></returns>
        public AssignedCase FindByCaseNumber(string CaseNumber)
        {
            return _assignedCaseRepository.FindByCaseNumber(CaseNumber);
        }

        /// <summary>
        ///     Calls Assigned Case repository method Find().
        /// </summary>
        /// <param name="assignedCase"></param>
        /// <returns></returns>
        public string Create(AssignedCase assignedCase)
        {
            return _assignedCaseRepository.Create(assignedCase);
        }

        /// <summary>
        ///     Calls Assigned Case repository method Find().
        /// </summary>
        /// <param name="assignedCase"></param>
        /// <returns></returns>
        public string Update(AssignedCase assignedCase)
        {
           return _assignedCaseRepository.Update(assignedCase);
        }
    }
}
