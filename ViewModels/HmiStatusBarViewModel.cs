using Caliburn.Micro;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class HmiStatusBarViewModel: PropertyChangedBase
    {
        private readonly Sharp7PlcService _plcService;
        public HmiStatusBarViewModel(Sharp7PlcService plcService)
        {
            _plcService = plcService;
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;
        }

        private void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            ConnectionState = _plcService.ConnectionState;
            TimeScan = _plcService.ScanTtime;
        }

        private ConnectionStates _connectionState;
        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set
            {
                _connectionState = value;
                NotifyOfPropertyChange(() => ConnectionState);
            }
        }

        private TimeSpan timeSpan;
        public TimeSpan TimeScan
        {
            get { return timeSpan; }
            set
            {
                timeSpan = value;
                NotifyOfPropertyChange(() => TimeScan);
            }
        }
    }
}
