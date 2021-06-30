using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ControlsHolderViewModel:Screen
    {
        private readonly Sharp7PlcService _plcService;
        private readonly PlcViewModel _plcViewModel;
        private readonly IEventAggregator _eventAggregator;

        public ControlsHolderViewModel(Sharp7PlcService plcService, PlcViewModel plcViewModel, IEventAggregator eventAggregator)
        {
            _plcViewModel = plcViewModel;
            _eventAggregator = eventAggregator;
            _plcService = plcService;
            HmiStatusBar = new HmiStatusBarViewModel(_plcService, _plcViewModel, _eventAggregator);
            // for initialzie purpose
            OnPlcServiceValueUpdated(null, null);
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;

            //Items = new BindableCollection<Screen>();

            //Items.Add(new SemaphoreViewModel(_plcService,Converters.BrushConverterColours.Green));
        }

        public BindableCollection<Screen> Items { get; set; }


        public HmiStatusBarViewModel HmiStatusBar { get; }

        private void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            //TODO: implement update!
        }
    }
}
