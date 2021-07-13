using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ControlsHolderViewModel:Screen, IHandle<CreateControlMessage>
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

        protected override void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);

        }
        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
        }

        private Screen CreateNewControlVewModel(DefaultControl defaultControl)
        {
            
            switch (defaultControl.ControlType)
            {
                case Messages.ControlType.GreenSemaphore:
                    var controlViewModel = new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _eventAggregator, _plcViewModel);
                    //plcViewModel.PlcRemoved += OnPlcRemoved;
                    return controlViewModel;
                case Messages.ControlType.OrangeSemaphore:
                    controlViewModel = new SemaphoreViewModel(BrushConverterColours.Orange, _plcRepository, defaultControl, _eventAggregator, _plcViewModel);
                    //plcViewModel.PlcRemoved += OnPlcRemoved;
                    return controlViewModel;
                case Messages.ControlType.RedSemaphore:
                    controlViewModel = new SemaphoreViewModel(BrushConverterColours.Red, _plcRepository, defaultControl, _eventAggregator, _plcViewModel);
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
                    return new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _eventAggregator, _plcViewModel);
            }     
        }

        // HERE NEW CONTROL IS CREATES
        public void Handle(CreateControlMessage message)
        {
            var existedList = _plcRepository.GetAllControls(_plcViewModel.Id);

            // Find last Id and assign next to new Control
            var nextId = existedList.Count == 0 ? 1 : existedList.OrderBy(x => x.Id).LastOrDefault().Id + 1;

            var newControl = new DefaultControl()
            {
                Id = nextId,
                ControlName = message.ControlName,
                DataBlock = message.DataBlock,
                Index = message.Index,
                Offset = message.Offset,
                ControlType = message.ControlType,
                X = 0,
                Y = 0
            };

            // Add to repo
            try
            {
                _plcRepository.AddControl(newControl, _plcViewModel.Id);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            // add to list
            ControlList.Add(CreateNewControlVewModel(newControl));



        }

        private void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            //TODO: implement update!
        }


    }
}
