using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ControlsViewModel: Screen
    {
        private readonly Sharp7PlcService _plcService;
        private readonly PlcViewModel _plcViewModel;
        private readonly IEventAggregator _eventAggregator;

        public ControlsViewModel(Sharp7PlcService plcService, PlcViewModel plcViewModel, IEventAggregator eventAggregator)
        {
            _plcViewModel = plcViewModel;
            _eventAggregator = eventAggregator;
            _plcService = plcService;
            HmiStatusBar = new HmiStatusBarViewModel(_plcService, _plcViewModel, _eventAggregator);
            // for initialzie purpose
            OnPlcServiceValueUpdated(null, null);
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;
        }
        public HmiStatusBarViewModel HmiStatusBar { get; }

        protected override void OnActivate()
        {
            base.OnActivate();
            Name = _plcViewModel.Name;
            IpAdress = _plcViewModel.IpAdress;
            Rack = _plcViewModel.Rack;
            Slot = _plcViewModel.Slot;

            Connect();

        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _plcService.ValuesUpdated -= OnPlcServiceValueUpdated;
            Disconnect();
        }


        #region Properties
        // First method
        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string IpAdress;

        private string Rack;

        private string Slot;

        // Second Method
        private bool _highSensor;
        public bool HighSensor
        {
            get { return _highSensor; }
            set
            {
                _highSensor = value;
                NotifyOfPropertyChange(() => HighSensor);
            }
        }

        private bool _lowSensor;
        public bool LowSensor
        {
            get { return _lowSensor; }
            set
            {
                _lowSensor = value;
                NotifyOfPropertyChange(() => LowSensor);
            }
        }

        private bool _pumpState;
        public bool PumpState
        {
            get { return _pumpState; }
            set
            {
                _pumpState = value;
                NotifyOfPropertyChange(() => PumpState);
            }
        }

        private int _tankLevel;
        public int TankLevel
        {
            get { return _tankLevel; }
            set
            {
                _tankLevel = value;
                NotifyOfPropertyChange(() => TankLevel);
            }
        }

        private short _inletPumpSpeed;
        public short InletPumpSpeed
        {
            get { return _inletPumpSpeed; }
            set
            {
                _inletPumpSpeed = value;
                NotifyOfPropertyChange(() => InletPumpSpeed);
                _plcService.WriteInletPumpSpeed(value).AsResult();
            }
        }
        private short _outletPumpSpeed;
        public short OutletPumpSpeed
        {
            get { return _outletPumpSpeed; }
            set
            {
                _outletPumpSpeed = value;
                NotifyOfPropertyChange(() => OutletPumpSpeed);
                _plcService.WriteOutletPumpSpeed(value).AsResult();
            }
        }

        #endregion

        #region Commands
        public void Connect()
        {
            int rackResult; // default 0
            int slotResult; // default 1
            rackResult = Int32.TryParse(Rack, out rackResult) ? rackResult : 0;
            slotResult = Int32.TryParse(Slot, out slotResult) ? slotResult : 1;

            _plcService.ConnectToPlc(IpAdress, rackResult, slotResult);
        }
        public void Disconnect()
        {
            _plcService.Disconnect();
        }

        public async void Start()
        {
            await _plcService.WriteStartMonostable();
        }

        public async void Stop()
        {
            await _plcService.WriteStopMonostable();
        }
        #endregion

        private void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            //ConnectionState = _plcService.ConnectionState;
            //TimeScan = _plcService.ScanTtime;
            PumpState = _plcService.PumpState;
            HighSensor = _plcService.HighLimit;
            LowSensor = _plcService.LowLimit;
            TankLevel = _plcService.TankLevel;
        }
    }
}
