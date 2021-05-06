using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TexEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
      

        private void ConvertBack_Click(object sender, RoutedEventArgs e)
        {

            var filePath = GetPath(".texjson", "Converted Tex Files (*.texjson)|*.texjson");
            try
            {
                LibTex.Serialize(filePath, filePath.Replace(".texjson", ".tex"));
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message, "Error");
            }
            
        }
        private void BtnFileSelect_Click(object sender, RoutedEventArgs e)
        {

            var filePath = GetPath(".tex", "Firefly Tex Files (*.tex)|*.tex");

            if (!File.Exists(filePath))
            {
                MessageBox.Show(this, "File NOT Exists!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            LibTex.Deserialize(filePath, filePath.Replace(".tex",".texjson"));
       
        }



        private String GetPath(String defaulttext, String filter)
        {
            // Create OpenFileDialog 
            var dlg = new Microsoft.Win32.OpenFileDialog
            {

                // Set filter for file extension and default file extension 
                DefaultExt = defaulttext,
                Filter = filter
            };

            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog(this);

            // Get the selected file name and display in a TextBox 
            if (result == null || result != true)
            {
                return null;
            }

            return dlg.FileName;
        }
    }
}
