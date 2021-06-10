using MobileJO.Data.ViewModels.Common;
using System.Collections.Generic;

namespace MobileJO.Domain.Contracts
{
    public interface IDropdownService
    {
        List<DropdownViewModel> GetApplicationType();
        List<DropdownViewModel> GetStatus();
        List<DropdownViewModel> GetUsers();
        List<DropdownViewModel> GetAccounts();
        List<DropdownViewModel> GetRoles();
        List<DropdownViewModel> GetBranches(int companyID);
        List<DropdownViewModel> GetCompanies(int userTypeID, int companyID, int templateID);
        List<DropdownViewModel> GetCompanies();
        List<DropdownViewModel> GetUserTypes(int userTypeID);
        List<DropdownViewModel> GetCompanies(int roleID, int companyID);
        List<DropdownViewModel> GetBranches(int roleID, int companyID, int selectedCompanyID);
        List<DropdownViewModel> GetRegion();
        List<DropdownViewModel> GetCity(int RegionID);
    }
}
