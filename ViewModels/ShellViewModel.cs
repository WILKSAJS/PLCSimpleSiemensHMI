using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ShellViewModel: PropertyChangedBase
    {
        #region Properties
        // First method
        private string _ipAdress;
        public string IpAdress
        {
            get => _ipAdress; 
            set => Set(ref _ipAdress, value);
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

        private ConnectionStates _connectionState;
        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set {
                _connectionState = value;
                NotifyOfPropertyChange(() => ConnectionState);
            }
        }

        private TimeSpan timeSpan;

        public TimeSpan TimeScan
        {
            get { return timeSpan; }
            set { 
                timeSpan = value;
                NotifyOfPropertyChange(() => TimeScan);
            }
        }

        #endregion

        #region Commands
        public void Connect()
        {

        }
        public void Disconnect()
        {

        }

        public void Start()
        {
            
        }

        public void Stop()
        {
           
        }
        #endregion


    }
}
