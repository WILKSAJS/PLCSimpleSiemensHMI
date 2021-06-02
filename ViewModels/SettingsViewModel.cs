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

        
    }
}
