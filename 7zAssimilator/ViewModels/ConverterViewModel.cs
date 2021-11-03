using _7zAssimilator.CommonFormFiles;
using _7zAssimilator.Models;
using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _7zAssimilator.ViewModels
{
    class ConverterViewModel : Screen
    {
        const string DownloadLocationPrefix = "Location: ";

        

        private string _chooseLocTxt;

        public string ChooseLocTxt
        {
            get { return _chooseLocTxt; }
            set
            {
                _chooseLocTxt = value;
                NotifyOfPropertyChange(() => _chooseLocTxt);
            }
        }

        private string _chooseTempLocTxt;

        public string ChooseTempLocTxt
        {
            get { return _chooseTempLocTxt; }
            set
            {
                _chooseTempLocTxt = value;
                NotifyOfPropertyChange(() => _chooseTempLocTxt);
            }
        }

        public ConverterViewModel()
        {
            ChooseLocTxt = DownloadLocationPrefix;
            ChooseTempLocTxt = DownloadLocationPrefix;
        }

        public bool CanConvertBtn(string chooseLocTxt, string chooseTempLocTxt, string postConvertTxt)
        {
            string choiceLocation = ChooseLocTxt.Replace(DownloadLocationPrefix, "");
            string tempChoiceLocation = ChooseTempLocTxt.Replace(DownloadLocationPrefix, "");
            return !string.IsNullOrWhiteSpace(choiceLocation) && !string.IsNullOrWhiteSpace(tempChoiceLocation);
        }

        public void ConvertBtn(string chooseLocTxt, string chooseTempLocTxt, string postConvertTxt)
        {
            MassFileZipConvertModel converResults = MassFileZipConvert.Convert(chooseLocTxt.Replace(DownloadLocationPrefix, ""), chooseTempLocTxt.Replace(DownloadLocationPrefix, ""));
            string text = $"Total Converted: {converResults.CompressedCount}\nTotal Errors: {converResults.ErrorCount}\nCheck logs for more info";
            MessageBox.Show(text, "Conversion Results");


        }
    }
}
