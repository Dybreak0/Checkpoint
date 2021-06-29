
using MobileJO.Core.Models;
using System.Collections.ObjectModel;

namespace MobileJO.Core.ViewModels.FieldCOViewModels
{
    public class FirstPageCOViewModel
    {

        public int SelectedBranch { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Spouse { get; set; }
        public string DeliveryAddress { get; set; }

        public ObservableCollection<UnitDesiredModel> UnitDesiredDDL { get; set; }

        public decimal Total { get; set; }
        public string OfficialReceipt { get; set; }

        public string ClientResCertNo { get; set; }
    }
}
