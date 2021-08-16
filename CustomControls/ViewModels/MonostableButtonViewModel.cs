using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class MonostableButtonViewModel : BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;

        public MonostableButtonViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
            : base(plcRepository, defaultControl, plcViewModel)
        {
            _plcService = plcService;
        }
        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            // thre is no need for cycle operation e.g. read status in Button Method
        }
        public bool CanMonostableButtonClick
        {
            get { return _plcService.ConnectionState == ConnectionStates.Online; }
        }
        public async void MonostableButtonClick()
        {
            await _plcService.WriteStartStopMonostable(_DbBlockAdress);
        }
    }
}
