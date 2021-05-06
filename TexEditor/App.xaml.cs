using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace TexEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            // Application is running
            // Process command line args
            var dictionary = e.Args.Select(a => a.Split('='))
                     .ToDictionary(a => a[0], a => a.Length == 2 ? a[1] : null);

            // Create main application window, starting minimized if specified
            
            if (dictionary.Count == 0)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                dictionary["InputFilePath"];
                dictionary["OutputPath"];
                dictionary["FileType"];
            }
            
        }
    }
}
