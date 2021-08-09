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

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class RealTextBoxViewModel : BaseControlViewModel
    {
        Regex regex = new Regex("[+-]?([0-9]*[.])?[0-9]+");

        private readonly Sharp7PlcService _plcService;
        public RealTextBoxViewModel(Sharp7PlcService plcService, PlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
            : base(plcRepository, defaultControl, plcViewModel)
        {
            _plcService = plcService;
        }

        private float _value;
        public float Value
        {
            get { return _value; }
            set => Set(ref _value, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
           Value  = await _plcService.ReadReal(_DbBlockAdress);
        }

        //public bool CanSaveNewValue(string name)
        //{
        //    return !string.IsNullOrWhiteSpace(name) && regex.IsMatch(name);
        //}

        public async Task SaveNewValue(TextBox source)
        {
            string value = source.Text.Replace(',', '.');


            if (!string.IsNullOrWhiteSpace(value) && regex.IsMatch(value))
            {
                float var = float.Parse(value,CultureInfo.InvariantCulture);
                await _plcService.WriteReal(_DbBlockAdress, var);
            }
            else
            {
                MessageBox.Show($"Wrong format of {value}");
            }
        }
    }
}
