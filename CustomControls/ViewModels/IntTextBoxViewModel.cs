using PLCSiemensSymulatorHMI.ExternalComponents.Services;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using System.Windows;
using Caliburn.Micro;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class IntTextBoxViewModel : BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;
        public IntTextBoxViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel, IWindowManager windowManager)
            : base(plcRepository, defaultControl, plcViewModel, windowManager)
        {
            _plcService = plcService;

            _plcService.ConnectionStateUpdated += _plcService_ConnectionStateUpdated;
        }
        // Event rised to perform CanBistableButtonClick check - if connection is estabilished button usecase is available
        private void _plcService_ConnectionStateUpdated(object sender, EventArgs e)
        {
            this.NotifyOfPropertyChange(nameof(CanSaveNewValue));
        }

        private short _value;
        public short Value
        {
            get { return _value; }
            set => Set(ref _value, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            Value = await _plcService.ReadInt(DbBlockAdress);
        }

        public bool CanSaveNewValue
        {
            get { return _plcService.ConnectionState == ConnectionStates.Online; }
        }

        public async Task SaveNewValue(TextBox source, KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Enter)
            {
                string value = source.Text.Replace(" ", "");
                short var;
                if (short.TryParse(value, out var))
                {
                    await _plcService.WriteInt(DbBlockAdress, var);
                    source.Text = "";
                }
                else
                {
                    MessageBox.Show($"Wrong input format. Insert integer value e.g.: 25");
                }
            }
        }
    }
}

