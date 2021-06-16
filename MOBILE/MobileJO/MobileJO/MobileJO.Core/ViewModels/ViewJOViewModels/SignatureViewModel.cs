using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.Utilities;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;

namespace MobileJO.Core.ViewModels
{
    public class SignatureViewModel : BaseViewModel
    {
        public JobOrder value { get; set; } = new JobOrder();

        private readonly IMvxNavigationService _navigationService;
        private readonly IAppSettings _settings;
        private readonly IWebService _webService;
        private readonly IUserDialogs _userDialogs;
        private readonly ILocalizeService _localizeService;
        private Dictionary<string, string> _parameter;
        public string signatureUri;
        

        public SignatureViewModel(IMvxNavigationService navigationService,
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

        public ImageSource imageData;
        public ImageSource ImageData
        {
            set
            {
                if (imageData != value)
                {
                    imageData = value;
                }
            }
            get
            {
                return imageData;
            }
        }

        public override void Prepare(Dictionary<string, string> parameter)
        {
            _parameter = parameter;

            if (_parameter != null )
            {
                ViewSignature.Execute();
            }
        }

        public IMvxAsyncCommand ViewSignature => new MvxAsyncCommand(async () =>
        {

            if (IsBusy)
                return;

            IsBusy = true;
            var error = false;

            try
            {
                if (NetworkCheck.HasInternet())
                {
                    byte[] image = await _webService.DownloadFile(_parameter);

                    if (image != null)
                    {
                        var stream = new MemoryStream(image);
                        ImageData = (StreamImageSource)ImageSource.FromStream(() => stream);
                    }
                }
                else
                {
                    if (_parameter.ContainsKey(Constants.Params.SignatureName) && _parameter.ContainsKey(Constants.Params.SignaturePath))
                    {
                        string filename = _parameter[Constants.Params.SignatureName];
                        string folderPath = _parameter[Constants.Params.SignaturePath];

                        bool isFolderExist = await Data.FileAppData.IsFolderExistAsync(folderPath);

                        if (isFolderExist)
                        {
                            bool isFileExist = await Data.FileAppData.IsFileExistAsync(filename, folderPath);

                            if (isFileExist)
                            {
                                var imageAsBytes = await Data.FileAppData.LoadFile(filename, folderPath);

                                ImageData = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                            }
                        }
                        else
                        {
                            var localizedMessage = LocalizeService.Translate(Constants.Messages.NoInternet);
                            await _userDialogs.AlertAsync(localizedMessage, Constants.Modal.Warning, Constants.Common.OK);
                            await _navigationService.Close(this);
                        }
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
