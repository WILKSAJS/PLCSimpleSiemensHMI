using System;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.ExternalComponents.Services;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Caliburn.Micro;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class SliderViewModel : BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;

        public SliderViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel, IWindowManager windowManager)
            : base(plcRepository, defaultControl, plcViewModel, windowManager)
        {
            _plcService = plcService;
            Minimum = float.Parse(_defaultControl.AdditionalControlInfo[0]);
            Maximum = float.Parse(_defaultControl.AdditionalControlInfo[1]);
            ProcessColor = (Color)ColorConverter.ConvertFromString(_defaultControl.AdditionalControlInfo[2]);
            _plcService.ConnectionStateUpdated += _plcService_ConnectionStateUpdated;
        }
        // Event rised to perform CanBistableButtonClick check - if connection is estabilished button usecase is available
        private void _plcService_ConnectionStateUpdated(object sender, EventArgs e)
        {
            this.NotifyOfPropertyChange(nameof(CanWriteValue));
        }

        public bool CanWriteValue
        {
            get { return _plcService.ConnectionState == ConnectionStates.Online; }
        }

        #region properties
        private float _Value;
        public float Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                this.NotifyOfPropertyChange(() => Value);
            }
        }

        private float _Minimum;
        public float Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                this.NotifyOfPropertyChange(() => Minimum);
            }
        }

        private float _Maximum;
        public float Maximum
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
            //Button doesn't need to read sate, because it should be independend from PLC behaviour
            //State = await plcService.ReadBit(_DbBlockAdress);
        }

        public void OnSliderValueChanged(RoutedPropertyChangedEventArgs<double> eventArgs)
        {
            Value = (float)eventArgs.NewValue;
        }

        // TODO: test if can perform operation for INT and REAL
        public async Task OnThumbPreviewMouseUp()
        {
            if (CanWriteValue)
            {
                if (_defaultControl.Index.Contains("DBW"))
                {
                    await _plcService.WriteInt(DbBlockAdress, Convert.ToInt16(Value));
                }

                if (_defaultControl.Index.Contains("DBD"))
                {
                    await _plcService.WriteReal(DbBlockAdress, Value);
                }
            }
        }
    }
}
