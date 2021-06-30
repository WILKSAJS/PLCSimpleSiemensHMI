using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class SemaphoreViewModel:Screen
    {
        //private readonly Sharp7PlcService plcService;
        private readonly PlcRepository _plcRepository;
        private readonly DefaultControl _defaultControl;
        private readonly IEventAggregator _eventAggregator;

        public SemaphoreViewModel(BrushConverterColours brushConverterColours, PlcRepository plcRepository, DefaultControl defaultControl, IEventAggregator eventAggregator)
        {
            // set colour for semaphore every time if crate new
            //SemaphoreColour = brushConverterColours;
            //this.plcService = plcService;
            _plcRepository = plcRepository;
            _defaultControl = defaultControl;
            _eventAggregator = eventAggregator;
            this.SemaphoreColour = brushConverterColours;

        }

        public double X => _defaultControl.X;
        public double Y => _defaultControl.Y;

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

        //protected override void OnActivate()
        //{
        //    plcService.ValuesUpdated += PlcService_ValuesUpdated;
        //    base.OnActivate();
        //}
        //protected override void OnDeactivate(bool close)
        //{
        //    plcService.ValuesUpdated -= PlcService_ValuesUpdated;
        //    base.OnDeactivate(close);
        //}


        //private void PlcService_ValuesUpdated(object sender, EventArgs e)
        //{
        //    var service = (Sharp7PlcService)sender;
        //    SemaphoreState = service.ReadBitValue(dbAdress);
        //}

      

    }
}
