namespace ImageBox
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using Xamarin.Essentials;
    using Microsoft.AppCenter;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using System.IO;

    public partial class App : Application
    {
        public static object ParentWindow { get; set; }
        public App()
        {
            InitializeComponent();
            string _directoryName = Path.Combine(FileSystem.CacheDirectory, "Cache");
            string[] _files = Directory.GetFiles(_directoryName);
            foreach(string _file in _files)
            {
                File.Delete(_file);
            }

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=79662850-16c1-4561-8b3d-fa2410c34bff;" +
                  "uwp={Your UWP App secret here};" +
                  "ios={Your iOS App secret here}",
                  typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private static MyDatabase _myDatabase;
        public static MyDatabase Database
        {
            get
            {
                if (_myDatabase == null)
                {
                    
                    var databasePath = Path.Combine(FileSystem.AppDataDirectory, "ImageBox.db");

                    _myDatabase = new MyDatabase(databasePath);
                }
                return _myDatabase;
            }
        }
    }
}
