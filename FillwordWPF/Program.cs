using System;

namespace FillwordWPF
{
    public static class Program
    {
        private static App app;

        public static App App
        {
            get
            {
                if (app == null)
                {
                    app = new App();
                }

                return app;
            }
        }

        [STAThread]
        public static void Main()
        {
            App.Run();
        }
    }
}
