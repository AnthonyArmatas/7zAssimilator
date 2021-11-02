using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WK.Libraries.BetterFolderBrowserNS;

namespace _7zAssimilator.Views
{
    /// <summary>
    /// Interaction logic for ConverterView.xaml
    /// </summary>
    public partial class ConverterView : Window
    {
        public ConverterView()
        {
            InitializeComponent();
            this.Left = (SystemParameters.WorkArea.Width - Width) / 2 + SystemParameters.WorkArea.Left;
            this.Top = (SystemParameters.WorkArea.Height - Height) / 2 + SystemParameters.WorkArea.Top;
        }

        private void ChooseLocBtn_Click(object sender, RoutedEventArgs e)
        {
            LocateDirectory(this.ChooseLocTxt);
        }

        private void ChooseTempLocBtn_Click(object sender, RoutedEventArgs e)
        {
            LocateDirectory(this.ChooseTempLocTxt);
        }

        private void LocateDirectory(TextBlock textBlock)
        {
            var betterFolderBrowser = new BetterFolderBrowser();

            betterFolderBrowser.Title = "Select folders...";
            betterFolderBrowser.RootFolder = "C:\\";

            // Allow multi-selection of folders.
            betterFolderBrowser.Multiselect = false;

            if (betterFolderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBlock.Text = "Location: " + betterFolderBrowser.SelectedPath;
            }
        }
    }
}
