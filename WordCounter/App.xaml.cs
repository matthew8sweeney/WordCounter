using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WordCounter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            // create the MainWindow
            MainWindow wnd = new MainWindow();

            // check if the app is being used to open a file
            if (e.Args.Length > 0)
            {
                //Trace.WriteLine("First cmdLine Arg: " + e.Args[0]);
                // try to open the requested file
                wnd.OpenFile(e.Args[0]);
            }

            wnd.Show();
        }
    }
}
