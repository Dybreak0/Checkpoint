using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using MobileJO.Core.Base;
using MvvmCross.Navigation;
using MobileJO.Core.Contracts;
using MvvmCross.Commands;
using MobileJO.Core.Utilities;
using MobileJO.Core.Models;
using SignaturePad.Forms;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using MvvmCross.Forms.Presenters.Attributes;
using System.Linq;

namespace MobileJO.Core.ViewModels
{
    public class ClientSignatureViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly IUserDialogs _userDialogs;
        private readonly IAppSettings _settings;
        private readonly ILocalizeService _localizeService;

        private string _stringConvertedSignatureArray { get; set; } 

        public ClientSignatureViewModel(IMvxNavigationService navigationService, 
                                        IAppSettings settings,
                                        IUserDialogs userDialogs, 
                                        ILocalizeService localizeService) : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;

        }

        public IMvxCommand CloseCommand => new MvxCommand(async () =>
        {
            await _navigationService.Close(this, _stringConvertedSignatureArray);
        });

        public IMvxCommand GoToLastPageCommand => new MvxCommand<byte[]>(async (signatureBytes) =>
        {            
            if (signatureBytes != null)
            {
                var signatureArray = signatureBytes.Select(byteValue => byteValue.ToString()).ToArray();

                _stringConvertedSignatureArray = string.Join(Constants.SpecialCharacters.Comma, signatureArray);
            }          

            await _navigationService.Close(this, _stringConvertedSignatureArray);
        });

    }
}
