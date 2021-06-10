using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using System.Collections.Generic;

namespace MobileJO.Data.Contracts
{
    public interface IDropdownRepository
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
        List<DropdownViewModel> GetCompanies(int userTypeID, int companyID);
        List<DropdownViewModel> GetBranches(int userTypeID, int companyID, int selectedCompanyID);
        List<DropdownViewModel> GetRegion();
        List<DropdownViewModel> GetCity(int RegionID);
    }
}
