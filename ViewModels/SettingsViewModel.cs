using Caliburn.Micro;
using Microsoft.Win32;
using PLCSiemensSymulatorHMI.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class SettingsViewModel: Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private string _filePath;

        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            // read value from config file at the beginning and put it to textbox
            FilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];
        }

        public string FilePath
        {
            get { return _filePath; }
            set => Set(ref _filePath, value);
        }

        public void OpenFileDialogClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML config file (*.xml)|*.xml";
            openFileDialog.Multiselect = false;

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    //Get the path of specified file
                    FilePath = openFileDialog.FileName;
                    if (!String.IsNullOrEmpty(FilePath))
                    {
                        _eventAggregator.PublishOnUIThread(new ChangeFilePathMessage() { FilePath = FilePath });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
