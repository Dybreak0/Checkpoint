using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using System.Collections.Generic;

namespace MobileJO.Domain.Contracts
{
    public interface IAssignedCasesService
    {
        AssignedCasesViewModel Find(int id);
        ListViewModel Search(AssignedCasesSearchViewModel searchViewModel);
        List<string> GetApplicationTypes();
        List<string> GetCaseStatusList();
        List<AssignedCasesListViewModel> FindCases(string ids);
        AssignedCase FindByCaseNumber(string CaseNumber);
        string Create(AssignedCase assignedCase);
        string Update(AssignedCase assignedCase);
    }
}
