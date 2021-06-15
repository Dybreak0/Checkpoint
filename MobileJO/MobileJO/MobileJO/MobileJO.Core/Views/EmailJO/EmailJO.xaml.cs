using Acr.UserDialogs;
using MobileJO.Core.Base;
using MobileJO.Core.Contracts;
using MobileJO.Core.Models;
using MobileJO.Core.ViewModels.EmailJO;
using MvvmCross.Navigation;
using Plugin.SecureStorage;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileJO.Core.Views.EmailJO
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmailJO : BaseContentPage
    {
		public EmailJO ()
		{
            InitializeComponent();

            //initialize only if needed Activity Indicator
            var tempContent = Content;

            //Assign content to null to fix bug for iOS
            Content = null;
            Content = CreateLoadingIndicatorRelativeLayout(tempContent);
           
        }

        private void UseDefaultAddress_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (useDefaultAddress.IsToggled)
            {
                emailRecipient.IsVisible = false;
            }
            else
            {
                emailRecipient.IsVisible = true;
            }
            
        }

        //private void To_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        //                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        //    var text = e.NewTextValue;
        //    char lastCharacter = char.Parse(text.Substring(text.Length - 1, 1));

        //    if (text.Substring(text.Length-1,1) == ";")
        //    {
        //        ToRecipient.Text = "GOCHA";
        //    }
        //}

  

    }
}