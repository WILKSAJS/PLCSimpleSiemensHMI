using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ControlsHolderViewModel:Screen
    {
        private readonly PlcRepository _plcRepository;
        private readonly Sharp7PlcService _plcService;
        private readonly PlcViewModel _plcViewModel;
        private readonly IEventAggregator _eventAggregator;

        public ControlsHolderViewModel(PlcRepository plcRepository, Sharp7PlcService plcService, PlcViewModel plcViewModel, IEventAggregator eventAggregator)
        {
            _plcViewModel = plcViewModel;
            _eventAggregator = eventAggregator;
            _plcRepository = plcRepository;
            _plcService = plcService;
            HmiStatusBar = new HmiStatusBarViewModel(_plcService, _plcViewModel, _eventAggregator);
            // for initialzie purpose
            OnPlcServiceValueUpdated(null, null);
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;

            //Items = new BindableCollection<Screen>();

            //Items.Add(new SemaphoreViewModel(_plcService,Converters.BrushConverterColours.Green));
            ControlList.AddRange(_plcRepository.GetAllControls(_plcViewModel.Id).Select(x => CreateNewControlVewModel(x)));
        }

        public BindableCollection<Screen> ControlList { get; set; } = new BindableCollection<Screen>();

        public HmiStatusBarViewModel HmiStatusBar { get; }

        private Screen CreateNewControlVewModel(DefaultControl defaultControl)
        {
            
            switch (defaultControl.ControlType)
            {
                case Messages.ControlType.GreenSemaphore:
                    var controlViewModel = new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _eventAggregator);
                    //plcViewModel.PlcRemoved += OnPlcRemoved;
                    return controlViewModel;
                case Messages.ControlType.OrangeSemaphore:
                    controlViewModel = new SemaphoreViewModel(BrushConverterColours.Orange, _plcRepository, defaultControl, _eventAggregator);
                    //plcViewModel.PlcRemoved += OnPlcRemoved;
                    return controlViewModel;
                case Messages.ControlType.RedSemaphore:
                    controlViewModel = new SemaphoreViewModel(BrushConverterColours.Red, _plcRepository, defaultControl, _eventAggregator);
                    //plcViewModel.PlcRemoved += OnPlcRemoved;
                    return controlViewModel;
                //case Messages.ControlType.BistableButton:
                //    break;
                //case Messages.ControlType.MonostableButton:
                //    break;
                //case Messages.ControlType.Label:
                //    break;
                default:
                    //TODO: DEFAULT BEHAVIOUR........
                    return new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _eventAggregator);
            }
            
            
        }


        private void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            //TODO: implement update!
        }
    }
}
