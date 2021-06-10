using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using MobileJO.Core.Contracts;
using MobileJO.Droid;
using Plugin.CurrentActivity;
using Plugin.Media;
using System;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(MainApplication.CloseApplication))]
[assembly: Xamarin.Forms.Dependency(typeof(MainApplication.DirectoryHelper))]
namespace MobileJO.Droid
{
    //You can specify additional application information in this attribute
    #if DEBUG
    [Application(Debuggable = true)]
    #else
    [Application(Debuggable = false)]
    #endif
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override async void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            //A great place to initialize Xamarin.Insights and Dependency Services!
            CrossCurrentActivity.Current.Init(this);

        }

        public override void OnTerminate()
        {
            base.OnTerminate();

            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {

        }

        public void OnActivityDestroyed(Activity activity)
        {            
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {

        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {

        }

        public void OnActivityStarted(Activity activity)
        {

        }

        public void OnActivityStopped(Activity activity)
        {

        }
        
        public class CloseApplication : ICloseApplication
        {
            public void ExitApplication()
            {
                Process.KillProcess(Process.MyPid());
            }
        }

        public class DirectoryHelper : IDirectory
        {
            public string documentBasePath = Android.OS.Environment.ExternalStorageDirectory.Path;

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
                return;
            }

            
        }
    }
}