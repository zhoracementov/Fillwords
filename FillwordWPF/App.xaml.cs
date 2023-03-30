using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace FillwordWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly object syncRoot = new object();
        private static volatile App instance;
        public static App Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new App();
                        }
                    }
                }

                return instance;
            }
        }

        public static string SettingsFileName => Path.Combine(CurrentDirectory, ConfigurationManager.AppSettings["recordsFilePath"] ?? "records");
        public static bool IsDesignMode { get; set; } = true;

        private App() : base()
        {
            //...
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IsDesignMode = false;

            var w = new MainWindow();
            w.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        public static string CurrentDirectory => IsDesignMode
                ? Path.GetDirectoryName(GetSourceCodePath())
                : Environment.CurrentDirectory;

        public static string GetSourceCodePath([CallerFilePath] string path = null) => path;
    }
}
