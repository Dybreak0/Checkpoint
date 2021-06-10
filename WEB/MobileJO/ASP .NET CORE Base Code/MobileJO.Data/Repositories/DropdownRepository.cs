using MobileJO.Data.Contracts;
using MobileJO.Data.Models;
using MobileJO.Data.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;

namespace MobileJO.Data.Repositories
{
    public class DropdownRepository : BaseRepository, IDropdownRepository
    {
        public DropdownRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        { }

        public List<DropdownViewModel> GetApplicationType()
        {
            return GetDbSet<ApplicationType>()
                .Select(x => new DropdownViewModel
                {
                    Value = x.ID,
                    Text = x.ApplicationName
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetStatus()
        {
            return GetDbSet<JobOrderStatus>()
                .Select(x => new DropdownViewModel
                {
                    Value = x.ID,
                    Text = x.Status
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetUsers()
        {
            return GetDbSet<User>()
                .Select(x => new DropdownViewModel
                {
                    Value = x.ID,
                    Text = string.Format(Constants.Common.NameFormat, x.FirstName, x.LastName)
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetAccounts()
        {
            return GetDbSet<Account>()
                .Select(x => new DropdownViewModel
                {
                    Value = x.ID,
                    Text = x.Name
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetRoles()
        {
            return GetDbSet<Role>()
               .Select(x => new DropdownViewModel
               {
                   Value = x.ID,
                   Text = x.RoleName
               })
               .OrderBy(x => x.Text)
               .ToList();

        }

        public List<DropdownViewModel> GetUserTypes(int userTypeID)
        {

            return GetDbSet<UserType>()
                .Where(x => userTypeID == Constants.User.AdminID
                    || (userTypeID == Constants.User.CompanyAdminID && x.UserTypeID != Constants.User.AdminID))
                .Select(x => new DropdownViewModel
                {
                    Value = x.UserTypeID,
                    Text = x.Type
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetCompanies(int userTypeID, int companyID)
        {
            return GetDbSet<Company>()
                .Where(x => userTypeID == Constants.User.AdminID
                        || (userTypeID == Constants.User.CompanyAdminID && x.CompanyID == companyID))
                .Select(x => new DropdownViewModel
                {
                    Value = x.CompanyID,
                    Text = x.CompanyName
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetBranches(int userTypeID, int companyID, int selectedCompanyID)
        {
            return GetDbSet<Branch>()
                .Where(x => (userTypeID == Constants.User.AdminID && selectedCompanyID == 0)
                    || (userTypeID == Constants.User.AdminID && x.CompanyID == selectedCompanyID)
                    || (userTypeID == Constants.User.CompanyAdminID && x.CompanyID == companyID))
                .Select(x => new DropdownViewModel
                {
                    Value = x.BranchID,
                    Text = x.Name
                })
                .OrderBy(x => x.Text)
                .ToList();
            
        }

        public List<DropdownViewModel> GetBranches(int companyID)
        {
            return GetDbSet<Branch>()
                .Where(x => x.CompanyID == companyID)
                .Select(x => new DropdownViewModel
                {
                    Value = x.BranchID,
                    Text = x.Name
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetCompanies()
        {
            return GetDbSet<Company>()
                .Select(x => new DropdownViewModel
                {
                    Value = x.CompanyID,
                    Text = x.CompanyName
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetCompanies(int userTypeID, int companyID, int templateID)
        {
            var templateBranchList = GetDbSet<Template_Branch>()
                .Where(x => x.TemplateID == templateID)
                .ToList();
            var branchList = GetDbSet<Branch>().ToList();
            List<int> companyIDs = templateBranchList.Join(branchList, a => a.BranchID, b => b.BranchID, (a, b) => b.CompanyID).ToList();

            return GetDbSet<Company>()
                .Where(x => ((int)Constants.UserType.SuperAdmin == userTypeID && companyIDs.Contains(x.CompanyID))
                    || x.CompanyID == companyID)
                .Select(x => new DropdownViewModel
                {
                    Value = x.CompanyID,
                    Text = x.CompanyName
                })
                .OrderBy(x => x.Text)
                .ToList();
        }

        public List<DropdownViewModel> GetRegion()
        {
            return GetDbSet<Region>()
                .Select(x => new DropdownViewModel
                {
                    Value = x.RegionID,
                    Text = x.RegionName
                })
                .OrderBy(x => x.Text)
                .ToList();
        }
        public List<DropdownViewModel> GetCity(int RegionID)
        {
            return GetDbSet<RegionCity>()
                .Where(x => x.RegionID == RegionID)
                .Select(x => new DropdownViewModel
                {
                    Value = x.CityID,
                    Text = x.CityName,
                    Code = x.ZipCode,
                })
                .OrderBy(x => x.Text)
                .ToList();
        }
    }
}
