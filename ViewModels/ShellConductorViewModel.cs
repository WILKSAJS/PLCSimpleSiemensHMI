using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ShellConductorViewModel: Conductor<Screen>.Collection.OneActive
    {
        private readonly Sharp7PlcService _plcService;
        public ShellConductorViewModel(HmiStatusBarViewModel hmiStatusBarViewModel, Sharp7PlcService plcService)
        {
            _plcService = plcService;
            // for initialzie purpose
            OnPlcServiceValueUpdated(null,null);
            IpAdress = "127.0.0.1";
            Rack = "0";
            Slot = "1";
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;
            HmiStatusBar = hmiStatusBarViewModel;
        }

        public HmiStatusBarViewModel HmiStatusBar { get; }

        #region Properties
        // First method
        private string _ipAdress;
        public string IpAdress
        {
            get => _ipAdress;
            set => Set(ref _ipAdress, value);
        }

        private string _rack;
        public string Rack
        {
            get => _rack; 
            set => Set(ref _rack, value);
        }

        private string _slot;
        public string Slot
        {
            get => _slot;
            set => Set(ref _slot, value);
        }

        // Second Method
        private bool _highSensor;
        public bool HighSensor
        {
            get { return _highSensor; }
            set { 
                _highSensor = value;
                NotifyOfPropertyChange(()=> HighSensor);
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
            set {
                _pumpState = value;
                NotifyOfPropertyChange(()=>PumpState);
            }
        }

        private int _tankLevel;
        public int TankLevel
        {
            get { return _tankLevel; }
            set { 
                _tankLevel = value;
                NotifyOfPropertyChange(() => TankLevel);
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
