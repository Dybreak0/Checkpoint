using System;
using System.Collections.Generic;
using Acr.UserDialogs;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Utilities;
using Xamarin.Forms;
using System.IO;
using System.Linq;
using MobileJO.Core.Models;

namespace MobileJO.Core.ViewModels
{
    public class ViewSignatureViewModel : BaseViewModel
    {
        private readonly IMvxNavigationService _navigationService;
        private readonly ILocalizeService _localizeService;
        private readonly IAppSettings _settings;
        private readonly IUserDialogs _userDialogs;
        private readonly IWebService _webService;
        private Dictionary<string, string> _parameter;

        private bool _isEnabledSignAgain;
        public bool IsEnabledSignAgain
        {
            get => _isEnabledSignAgain;
            set
            {
                SetProperty(ref _isEnabledSignAgain, value);
            }
        }

        public string NavButtonText { get; set; }

        public ViewSignatureViewModel(IMvxNavigationService navigationService, 
                                      IAppSettings settings,
                                      IUserDialogs userDialogs, 
                                      ILocalizeService localizeService,
                                      IWebService webService) : base(userDialogs, localizeService)
        {
            _navigationService = navigationService;
            _settings = settings;
            _userDialogs = userDialogs;
            _localizeService = localizeService;
            _webService = webService;
        }

        public IMvxAsyncCommand BackCommand => new MvxAsyncCommand(async () =>
        {
            var localizedText = _localizeService.Translate(_settings.PersistentText);

            await _userDialogs.AlertAsync(localizedText);
            await _navigationService.Close(this);
        });

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            if(NetworkCheck.HasInternet())
            {
                ViewSignature.Execute();
            }
            else
            {
                GetImage();
            }            
        }
        
        public ImageSource SignatureImageSource { get; set; }

        public async void GetImage()
        {
            if (_parameter.ContainsKey(Constants.Params.SignatureName) && _parameter.ContainsKey(Constants.Params.SignaturePath))
            {
                string filename = _parameter[Constants.Params.SignatureName];
                string folderPath = _parameter[Constants.Params.SignaturePath];

                bool isFolderExist = await Data.FileAppData.IsFolderExistAsync(folderPath);

                if(isFolderExist)
                {
                    IsEnabledSignAgain = false;
                    NavButtonText = string.Empty;

                    bool isFileExist = await Data.FileAppData.IsFileExistAsync(filename, folderPath);

                    if (isFileExist)
                    {
                        var imageAsBytes = await Data.FileAppData.LoadFile(filename, folderPath);

                        if(imageAsBytes != null)
                        {
                            SignatureImageSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                        }                        
                    }                    
                }
                else
                {
                    await _userDialogs.AlertAsync(Constants.Messages.NoInternet, Constants.Modal.Warning, Constants.Common.OK);
                    await _navigationService.Close(this);
                }
            }
            else if(_parameter.ContainsKey(Constants.Params.SignatureBytes))
            {
                var signatureStringArray = _parameter[Constants.Params.SignatureBytes].Split(Constants.SpecialCharacters.CharComma);

                byte[] signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                SignatureImageSource = ImageSource.FromStream(() => new MemoryStream(signatureBytes));

                IsEnabledSignAgain = true;
                NavButtonText = "Sign Again";
            }
        }

        public IMvxAsyncCommand SignAgain => new MvxAsyncCommand(async () =>
        {
            if ((_parameter.ContainsKey(Constants.Params.SignatureName) && 
                _parameter.ContainsKey(Constants.Params.SignaturePath)) || 
                (_parameter.ContainsKey(Constants.Params.FileName)      &&
                _parameter.ContainsKey(Constants.Params.AttachmentType)))
                return;

            await _navigationService.Close(this, Constants.Params.SignAgain); 
        });

        public IMvxAsyncCommand CloseCommand => new MvxAsyncCommand(async () =>
        {
            await _navigationService.Close(this, string.Empty);
        });

        public IMvxAsyncCommand ViewSignature => new MvxAsyncCommand(async () =>
        {

            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (_parameter.ContainsKey(Constants.Params.SignatureBytes))
                {
                    var signatureStringArray = _parameter[Constants.Params.SignatureBytes].Split(Constants.SpecialCharacters.CharComma);

                    byte[] signatureBytes = signatureStringArray.Select(byte.Parse).ToArray();

                    SignatureImageSource = ImageSource.FromStream(() => new MemoryStream(signatureBytes));

                    IsEnabledSignAgain = true;
                    NavButtonText = "Sign Again";
                }
                else 
                {
                    byte[] image = await _webService.DownloadFile(_parameter);

                    if (image != null)
                    {
                        var stream = new MemoryStream(image);
                        SignatureImageSource = (StreamImageSource)ImageSource.FromStream(() => stream);
                    }
                }                
            }
            catch (Exception)
            {
                error = true;
            }
            finally
            {
                IsBusy = false;
            }

            if (error)
            {
                var localizedMessage = LocalizeService.Translate(Constants.Messages.ErrorRetrieving);
                await UserDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                await _navigationService.Close(this);
            }
        });
    }
}
