using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class SemaphoreViewModel:Screen
    {
        private readonly Sharp7PlcService plcService;

        public SemaphoreViewModel(Sharp7PlcService plcService)
        {
            // set colour for semaphore every time if crate new
            SemaphoreColour = BrushConverterColours.Green;
            this.plcService = plcService;

        }


        protected override void OnActivate()
        {
            plcService.ValuesUpdated += PlcService_ValuesUpdated;
            base.OnActivate();
        }
        protected override void OnDeactivate(bool close)
        {
            plcService.ValuesUpdated -= PlcService_ValuesUpdated;
            base.OnDeactivate(close);
        }


        private void PlcService_ValuesUpdated(object sender, EventArgs e)
        {
            var service = (Sharp7PlcService)sender;
            SemaphoreState = service.ReadBitValue(DbAdress);
        }

        private bool _semaphoreState;
        public bool SemaphoreState
        {
            get { return _semaphoreState; }
            set => Set(ref _semaphoreState, value);
        }

        private BrushConverterColours _semaphoreColour;
        public BrushConverterColours SemaphoreColour
        {
            get { return _semaphoreColour; }
            set => Set(ref _semaphoreColour, value);
        }

        public string ControlName { get; set; }
        public string DataBlock { get; set; }
        public string Index { get; set; }
        public string Offset { get; set; }

        private string _DbAdress;
        protected string DbAdress
        {
            get { return _DbAdress; }
            set { _DbAdress = $"{this.DataBlock}.{this.Index}.{this.Offset}"; }
        }
    }
}
