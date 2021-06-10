using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.AssignedCase;
using MobileJO.Data.ViewModels.Common;
using System.Collections.Generic;

namespace MobileJO.Data.Contracts
{
    public interface IAssignedCasesRepository
    {
        ListViewModel Search(AssignedCasesSearchViewModel searchViewModel);
        AssignedCasesViewModel Find(int id);
        List<string> GetApplicationTypes();
        List<string> GetCaseStatusList();
        List<AssignedCasesListViewModel> FindCases(List<int> ids);
        AssignedCase FindByCaseNumber(string CaseNumber);
        string Create(AssignedCase assignedCase);
        string Update(AssignedCase assignedCase);
        AssignedCase FindByID(int id);
    }
}
