using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.ExternalComponents.Services;
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
        public TankProgressBarViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel, IWindowManager windowManager)
            : base(plcRepository, defaultControl, plcViewModel, windowManager)
        {
            _plcService = plcService;
            Minimum = Double.Parse(_defaultControl.AdditionalControlInfo[0]);
            Maximum = Double.Parse(_defaultControl.AdditionalControlInfo[1]);
            ProcessColor = (Color)ColorConverter.ConvertFromString(_defaultControl.AdditionalControlInfo[2]);
        }

        #region properties
        private double _Value;
        public double Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                this.NotifyOfPropertyChange(() => Value);
            }
        }

        private double _Minimum;
        public double Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                this.NotifyOfPropertyChange(() => Minimum);
            }
        }

        private double _Maximum;
        public double Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                this.NotifyOfPropertyChange(() => Maximum);
            }
        }

        private Color _ProcessColor;
        public Color ProcessColor
        {
            get { return _ProcessColor; }
            set => Set(ref _ProcessColor, value);
        }

        #endregion

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            if (_defaultControl.Index.Contains("DBW"))
            {
                Value = Convert.ToDouble(await _plcService.ReadInt(DbBlockAdress));
            }

            if (_defaultControl.Index.Contains("DBD"))
            {
                Value = Convert.ToDouble(await _plcService.ReadReal(DbBlockAdress));
            } 
        }
        
    }
}
