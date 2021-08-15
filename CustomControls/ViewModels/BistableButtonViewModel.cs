using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    class BistableButtonViewModel : BaseControlViewModel
    {
        private readonly Sharp7PlcService _plcService;

        public BistableButtonViewModel(Sharp7PlcService plcService, IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
            : base(plcRepository, defaultControl, plcViewModel)
        {
            _plcService = plcService;
        }

        private bool _state;
        public bool State
        {
            get { return _state; }
            set => Set(ref _state, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            State = await plcService.ReadBit(_DbBlockAdress);
        }

        public async void BistableButtonClick()
        {
            var result = await _plcService.WriteBit(_DbBlockAdress, !State);
            if (result != 0)
            {
                // TODO: Implement this with NLog
                Debug.WriteLine(DateTime.Now.ToString() + "\t WRITE BistableButton State Error");
            }
            else
            {
                State = !State;
            }    
            
        }
    }
}

