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
using System.Windows;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ControlsHolderViewModel:Screen, IHandle<CreateControlMessage>
    {
        private readonly IBasePlcRepository _plcRepository;
        private readonly Sharp7PlcService _plcService;
        private readonly PlcViewModel _plcViewModel;
        private readonly IEventAggregator _eventAggregator;

        public ControlsHolderViewModel(IBasePlcRepository plcRepository, Sharp7PlcService plcService, PlcViewModel plcViewModel, IEventAggregator eventAggregator)
        {
            _plcViewModel = plcViewModel;
            _eventAggregator = eventAggregator;
            
            _plcRepository = plcRepository;
            _plcService = plcService;
            HmiStatusBar = new HmiStatusBarViewModel(_plcService, _plcViewModel, _eventAggregator);

            _plcViewModel.PlcRemoved += OnPlcRemoved;

            ControlList.AddRange(_plcRepository.GetAllControls(_plcViewModel.Id).Select(x => CreateNewControlVewModel(x)));
        }

        private void OnPlcRemoved(object sender, EventArgs e)
        {
            var plcViewModel = (PlcViewModel)sender;

            plcViewModel.PlcRemoved -= OnPlcRemoved;

            ControlList.Clear();
            Disconnect();
        }

        public BindableCollection<BaseControlViewModel> ControlList { get; set; } = new BindableCollection<BaseControlViewModel>();

        public HmiStatusBarViewModel HmiStatusBar { get; }

        protected override async void OnActivate()
        {
            base.OnActivate();
            _eventAggregator.Subscribe(this);
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;
            await Connect();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _eventAggregator.Unsubscribe(this);
            _plcService.ValuesUpdated -= OnPlcServiceValueUpdated;
            _plcRepository.SaveChanges();
            Disconnect();
        }


        #region Create new control region
        // HERE NEW CONTROL IS CREATED
        public void Handle(CreateControlMessage message)
        {
            // Get list to retrieve next id in following steps
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
                _plcRepository.SaveChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // add to list
            ControlList.Add(CreateNewControlVewModel(newControl));
        }

        private BaseControlViewModel CreateNewControlVewModel(DefaultControl defaultControl)
        {
            // default control gona be green semaphore in case of some cast problems
            BaseControlViewModel newControlVm = new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _plcViewModel);
            switch (defaultControl.ControlType)
            {
                case Messages.ControlType.GreenSemaphore:
                    newControlVm = new SemaphoreViewModel(BrushConverterColours.Green, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.OrangeSemaphore:
                    newControlVm = new SemaphoreViewModel(BrushConverterColours.Orange, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.RedSemaphore:
                    newControlVm = new SemaphoreViewModel(BrushConverterColours.Red, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.MonostableButton:
                    newControlVm = new MonostableButtonViewModel(_plcService ,_plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.BistableButton:
                    newControlVm = new BistableButtonViewModel(_plcService, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.RealTextBox:
                    newControlVm = new RealTextBoxViewModel(_plcService, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.IntegerTextbox:
                    newControlVm = new IntTextBoxViewModel(_plcService, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.EmergencyButton:
                    newControlVm = new EmergencyButtonViewModel(_plcService, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                case Messages.ControlType.TankProgressBar:
                    newControlVm = new TankProgressBarViewModel(_plcService, _plcRepository, defaultControl, _plcViewModel);
                    goto default;
                default:
                    //Default behaviour - subscribe event and return new VM
                    newControlVm.ControlRemoved += OnControlRemoved;
                    return newControlVm;
            }
        }

        private void OnControlRemoved(object sender, EventArgs e)
        {
            var ControlVm = (BaseControlViewModel)sender;
            // remove control from list
            ControlList.Remove(ControlVm);
            // unsubscribe
            ControlVm.ControlRemoved -= OnControlRemoved;
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
            if (_plcService.ConnectionState == ConnectionStates.Online && ControlList.Count > 0)
            {
                foreach (BaseControlViewModel control in ControlList.ToList())
                {
                    await control.PerformControlOperation(_plcService);
                }
            }
        }
        #endregion

    }
}
