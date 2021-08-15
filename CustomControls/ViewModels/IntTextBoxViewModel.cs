using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using System.Windows;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class IntTextBoxViewModel : BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;
        public IntTextBoxViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
            : base(plcRepository, defaultControl, plcViewModel)
        {
            _plcService = plcService;
        }

        private short _value;
        public short Value
        {
            get { return _value; }
            set => Set(ref _value, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            Value = await _plcService.ReadInt(_DbBlockAdress);
        }

        public async Task SaveNewValue(TextBox source, KeyEventArgs keyArgs)
        {
            if (keyArgs.Key == Key.Enter)
            {
                string value = source.Text.Replace(" ", "");
                short var;
                if (short.TryParse(value, out var))
                {
                    await _plcService.WriteInt(_DbBlockAdress, var);
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

