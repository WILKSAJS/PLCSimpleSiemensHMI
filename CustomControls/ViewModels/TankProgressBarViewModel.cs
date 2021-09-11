using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class TankProgressBarViewModel:BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;
        public TankProgressBarViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
            : base(plcRepository, defaultControl, plcViewModel)
        {
            _plcService = plcService;
            MinimumValue = Double.Parse(defaultControl.AdditionalControlInfo[0]);
            MaximumValue = Double.Parse(defaultControl.AdditionalControlInfo[1]);
            ProgressBarColor = (Color)ColorConverter.ConvertFromString(defaultControl.AdditionalControlInfo[2]);

        }
        private double _Value;
        public double Value
        {
            get { return _Value; }
            set => Set(ref _Value, value);
        }

        private double _minimumValue;
        public double MinimumValue
        {
            get { return _minimumValue; }
            set => Set(ref _minimumValue, value);
        }

        private double _maximumValue;
        public double MaximumValue
        {
            get { return _maximumValue; }
            set => Set(ref _maximumValue, value);
        }

        private Color _ProgressBarColor;
        public Color ProgressBarColor
        {
            get { return _ProgressBarColor; }
            set => Set(ref _ProgressBarColor, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            if (_defaultControl.Index.Contains("DBW"))
            {
                Value = Convert.ToDouble(await _plcService.ReadInt(_DbBlockAdress));
            }

            if (_defaultControl.Index.Contains("DBD"))
            {
                Value = Convert.ToDouble(await _plcService.ReadReal(_DbBlockAdress));
            } 
        }
        
    }
}
