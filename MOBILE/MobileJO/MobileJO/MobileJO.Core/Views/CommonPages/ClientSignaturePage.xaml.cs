using MobileJO.Core.Base;
using MobileJO.Core.ViewModels;
using MvvmCross.Forms.Presenters.Attributes;
using SignaturePad.Forms;
using System;
using System.IO;

namespace MobileJO.Core.Views
{
    [MvxModalPresentation]
    public partial class ClientSignaturePage : BaseContentPage
    {
        public ClientSignaturePage()
        {
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
        }

        private async void SaveSignatureButton_Clicked(object sender, System.EventArgs e)
        {
            Stream bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);

            byte[] myBynary = null;

            if (bitmap != null)
            {
                
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.CopyTo(ms);
                    myBynary = ms.ToArray();
                }
            }            
            var vm = (ClientSignatureViewModel)DataContext;

            vm.GoToLastPageCommand.Execute(myBynary);

        }

        protected override bool OnBackButtonPressed()
        {
            var vm = (ClientSignatureViewModel)DataContext;

            vm.CloseCommand.Execute();

            return true;
        }
    }
}
