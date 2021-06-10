using System.Threading;
using UIKit;
using MobileJO.Core.Contracts;
using MobileJO.Core.Utilities;
using MobileJO.iOS;
using System.IO;
using System;
using Foundation;
using System.Drawing;
using Xamarin.Forms;
using System.Net;
using QuickLook;
using CoreFoundation;
using MobileJO.Core.CustomRenderer;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency(typeof(MobileJO.iOS.Application.CloseApplication))]
[assembly: Xamarin.Forms.Dependency(typeof(MobileJO.iOS.Application.DirectoryHelper))]
namespace MobileJO.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, Constants.Common.AppDelegate);
        }

        public class CloseApplication : ICloseApplication
        {
            public void ExitApplication()
            {
                Thread.CurrentThread.Abort();
            }
        }

        public class DirectoryHelper : IDirectory
        {
            public string documentBasePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);


            public string CreateDirectory(string directoryName)
            {
                var directoryPath = Path.Combine(documentBasePath, directoryName);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                return directoryPath;
            }

            public void OpenFile(string fileUri)
            {
                string test = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                var app = UIApplication.SharedApplication;

                var newFile = fileUri.Replace("%20"," ");
                var previewController = UIDocumentInteractionController.FromUrl(NSUrl.FromFilename(newFile));

                previewController.Delegate = new MyInteractionDelegate(UIApplication.SharedApplication.KeyWindow.RootViewController);
                previewController.PresentPreview(true);

                //Launch the saved file for viewing in default viewer.
                UIViewController currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                while (currentController.PresentedViewController != null)
                    currentController = currentController.PresentedViewController;
                UIView currentView = currentController.View;

            }

            public class UIDocumentInteractionControllerDelegateClass : UIDocumentInteractionControllerDelegate
            {
                UIViewController ownerVC;

                public UIDocumentInteractionControllerDelegateClass(UIViewController vc)
                {
                    ownerVC = vc;
                }

                public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
                {
                    return ownerVC;
                }

                public override UIView ViewForPreview(UIDocumentInteractionController controller)
                {
                    return ownerVC.View;
                }
            }

            //NEW CODE BELOW

            public class MyInteractionDelegate : UIDocumentInteractionControllerDelegate
            {
                UIViewController parent;

                public MyInteractionDelegate(UIViewController controller)
                {
                    parent = controller;
                }

                public MyInteractionDelegate(DirectoryHelper directoryHelper)
                {
                }

                public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
                {
                    return parent;
                }
            }

            // NEW CODE HERE

            public class PreviewControllerDS : QLPreviewControllerDataSource
            {
                private QLPreviewItem _item;

                public PreviewControllerDS(QLPreviewItem item)
                {
                    _item = item;
                }

                public override nint PreviewItemCount(QLPreviewController controller)
                {
                    return (nint)1;
                }

                public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
                {
                    return _item;
                }
            }

        }     
    }
    
}