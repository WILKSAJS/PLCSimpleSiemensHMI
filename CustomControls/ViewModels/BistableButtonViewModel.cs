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

            _plcService.ConnectionStateUpdated += _plcService_ConnectionStateUpdated;
        }
        // Event rised to perform CanBistableButtonClick check - if connection is estabilished button usecase is available
        private void _plcService_ConnectionStateUpdated(object sender, EventArgs e)
        {
            this.NotifyOfPropertyChange(nameof(CanBistableButtonClick));
        }

        private bool _state;
        public bool State
        {
            get { return _state; }
            set => Set(ref _state, value);
        }

        public override async Task PerformControlOperation(Sharp7PlcService plcService)
        {
            //Button doesn't need to read sate, because it should be independend from PLC behaviour
            //State = await plcService.ReadBit(_DbBlockAdress);
        }

        public bool CanBistableButtonClick
        {
            get { return _plcService.ConnectionState == ConnectionStates.Online; }
        }

        public async Task BistableButtonClick()
        {
            var result = await _plcService.WriteBit(_DbBlockAdress, !State);
            if (result == 0)
            {
                State = !State;
            }
            else
            {
                // TODO: Implement this with NLog
                Debug.WriteLine(DateTime.Now.ToString() + "\t WRITE BistableButton State Error");
            }    
            
        }
    }
}

