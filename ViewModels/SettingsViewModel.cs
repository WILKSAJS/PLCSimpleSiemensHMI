using Caliburn.Micro;
using System;
using PLCSiemensSymulatorHMI.PlcService;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class SettingsViewModel: Screen
    {
        private readonly Sharp7PlcService _plcService;
        public SettingsViewModel(Sharp7PlcService plcService)
        {
            _plcService = plcService;
        }

        private short _inletPumpSpeed;

        public short InletPumpSpeed
        {
            get { return _inletPumpSpeed; }
            set {
                _inletPumpSpeed = value;
                NotifyOfPropertyChange(()=>InletPumpSpeed);
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
    }
}
