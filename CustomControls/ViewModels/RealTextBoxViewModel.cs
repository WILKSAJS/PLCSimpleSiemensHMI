using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class RealTextBoxViewModel : BaseControlViewModel
    {
        private readonly Regex regex = new Regex("[+-]?([0-9]*[.])?[0-9]+");
        private readonly Sharp7PlcService _plcService;
        public RealTextBoxViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel, IWindowManager windowManager)
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

        private float _value;
        public float Value
        {
            get { return _value; }
            set => Set(ref _value, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
           Value  = await _plcService.ReadReal(DbBlockAdress);
        }

        public bool CanSaveNewValue
        {
            get { return _plcService.ConnectionState == ConnectionStates.Online; }
        }

        public async Task SaveNewValue(TextBox source, KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Enter)
            {
                string value = source.Text.Replace(',', '.');
                if (!string.IsNullOrWhiteSpace(value) && regex.IsMatch(value))
                {
                    float var = float.Parse(value, CultureInfo.InvariantCulture);
                    await _plcService.WriteReal(DbBlockAdress, var);
                    source.Text = "";
                }
                else
                {
                    MessageBox.Show($"Wrong input format. Insert real value e.g.: 1.55");
                }
            }
        }
    }
}
