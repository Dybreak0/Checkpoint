using MobileJO.Data.Contracts;
using MobileJO.Data.ViewModels.Common;
using MobileJO.Domain.Contracts;
using System.Collections.Generic;

namespace MobileJO.Domain.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly IDropdownRepository _dropdownRepository;

        public DropdownService(IDropdownRepository dropdownRepository)
        {
            _dropdownRepository = dropdownRepository;
        }

        public List<DropdownViewModel> GetApplicationType()
        {
            return _dropdownRepository.GetApplicationType();
        }

        public List<DropdownViewModel> GetStatus()
        {
            return _dropdownRepository.GetStatus();
        }

        public List<DropdownViewModel> GetUsers()
        {
            return _dropdownRepository.GetUsers();
        }

        public List<DropdownViewModel> GetAccounts()
        {
            return _dropdownRepository.GetAccounts();
        }

        public List<DropdownViewModel> GetRoles()
        {
            return _dropdownRepository.GetRoles();
        }

        public List<DropdownViewModel> GetBranches(int companyID)
        {
            return _dropdownRepository.GetBranches(companyID);
        }

        public List<DropdownViewModel> GetCompanies(int userTypeID, int companyID, int templateID)
        {
            return _dropdownRepository.GetCompanies(userTypeID, companyID, templateID);
        }

        public List<DropdownViewModel> GetCompanies()
        {
            return _dropdownRepository.GetCompanies();
        }

        public List<DropdownViewModel> GetUserTypes(int userTypeID)
        {
            return _dropdownRepository.GetUserTypes(userTypeID);
        }

        public List<DropdownViewModel> GetCompanies(int userTypeID, int companyID)
        {
            return _dropdownRepository.GetCompanies(userTypeID, companyID);
        }

        public List<DropdownViewModel> GetBranches(int userTypeID, int companyID, int selectedCompanyID)
        {
            return _dropdownRepository.GetBranches(userTypeID, companyID, selectedCompanyID);
        }
        public List<DropdownViewModel> GetRegion()
        {
            return _dropdownRepository.GetRegion();
        }
        public List<DropdownViewModel> GetCity(int RegionID)
        {
            return _dropdownRepository.GetCity(RegionID);
        }
    }
}
