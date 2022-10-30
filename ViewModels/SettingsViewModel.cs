using Caliburn.Micro;
using Microsoft.Win32;
using PLCSiemensSymulatorHMI.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class SettingsViewModel: Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private string _filePath;
        public event EventHandler<ChangeIntervalTimeArgs> IntervalTimeChanged;

        public SettingsViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            // read value from config file at the beginning and put it to textbox
            FilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];
            SetIntervalTimeLatel(System.Configuration.ConfigurationManager.AppSettings["InitialReadPLCInterval"]);
        }

        #region Config. File Path
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
        #endregion

        #region Interval scan time config


        private string _IntervalTimeLatel;
        public string IntervalTimeLatel
        {
            get { return _IntervalTimeLatel; }
            set => Set(ref _IntervalTimeLatel, value);
        }

        private void SetIntervalTimeLatel(string newValue)
        {
            IntervalTimeLatel = "Set scan time interval - default: " + newValue + " [ms]";
        }

        // event informs PLCService to change interval time  
        protected virtual void OnIntervalTimeChanged(int NewIntervalTime)
        {
            IntervalTimeChanged?.Invoke(this, new ChangeIntervalTimeArgs() { NewIntervalTime = NewIntervalTime });
        }

        public async Task SaveNewValue(TextBox source, KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Enter)
            {
                string value = source.Text.Replace(" ", "");
                int var;
                if (int.TryParse(value, out var))
                {
                    // save new value in appConfig
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configuration.AppSettings.Settings["InitialReadPLCInterval"].Value = value;
                    configuration.Save(ConfigurationSaveMode.Full, true);
                    ConfigurationManager.RefreshSection("appSettings");

                    // rise event to change value in PLC service as well!
                    OnIntervalTimeChanged(var);

                    //set new value at label:
                    SetIntervalTimeLatel(value);

                    source.Text = "";
                }
                else
                {
                    MessageBox.Show($"Wrong input format. Insert integer value e.g.: 300");
                }
            }
        }
        #endregion
    }
}
