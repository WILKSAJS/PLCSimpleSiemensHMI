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
    public class TankProgressBarViewModel:BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;
        public TankProgressBarViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
            : base(plcRepository, defaultControl, plcViewModel)
        {
            _plcService = plcService;

        }     

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            //cyclical operation e.g. read PLC data field status
            // State = await _plcService.ReadBit(_DbBlockAdress);
        }
        
    }
}
