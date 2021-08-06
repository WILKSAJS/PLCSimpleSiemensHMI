using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

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

            

            ControlList.AddRange(_plcRepository.GetAllControls(_plcViewModel.Id).Select(x => CreateNewControlVewModel(x)));
        }

        public BindableCollection<BaseControlViewModel> ControlList { get; set; } = new BindableCollection<BaseControlViewModel>();

        public HmiStatusBarViewModel HmiStatusBar { get; }

        protected override async void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);

            //// for initialzie purpose
            //OnPlcServiceValueUpdated(null, null);
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;

            await Connect();
            
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
            _plcService.ValuesUpdated -= OnPlcServiceValueUpdated;

            Disconnect();
        }


        #region Create new control region
        // HERE NEW CONTROL IS CREATED
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

        private BaseControlViewModel CreateNewControlVewModel(DefaultControl defaultControl)
        {

            switch (defaultControl.ControlType)
            {
                
                case Messages.ControlType.GreenSemaphore:
                    return new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _plcViewModel);
                case Messages.ControlType.OrangeSemaphore:
                    return new SemaphoreViewModel(BrushConverterColours.Orange, _plcRepository, defaultControl, _plcViewModel);
                case Messages.ControlType.RedSemaphore:
                    return new SemaphoreViewModel(BrushConverterColours.Red, _plcRepository, defaultControl, _plcViewModel);
                case Messages.ControlType.MonostableButton:
                    return new MonostableButtonViewModel(_plcService ,_plcRepository, defaultControl, _plcViewModel);
                case Messages.ControlType.BistableButton:
                    return new BistableButtonViewModel(_plcService, _plcRepository, defaultControl, _plcViewModel);
                //case Messages.ControlType.Label:
                //    break;
                default:
                    //TODO: DEFAULT BEHAVIOUR........
                    return new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _plcViewModel);
            }
        }
        #endregion


        #region PLCService methods
        public async Task Connect()
        {
            int rackResult; // default 0
            int slotResult; // default 1
            rackResult = Int32.TryParse(_plcViewModel.Rack, out rackResult) ? rackResult : 0;
            slotResult = Int32.TryParse(_plcViewModel.Slot, out slotResult) ? slotResult : 1;

             await _plcService.ConnectToPlc(_plcViewModel.IpAdress, rackResult, slotResult);
        }

        public void Disconnect()
        {
            _plcService.Disconnect();
        }

        private async void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            if (HmiStatusBar.ConnectionState == ConnectionStates.Online)
            {
                foreach (BaseControlViewModel control in ControlList)
                {
                    await control.PerformControlOperation(_plcService);
                }
            }
        }
        #endregion

    }
}
