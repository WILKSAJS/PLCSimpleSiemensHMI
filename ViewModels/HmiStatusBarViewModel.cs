using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.PlcService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class HmiStatusBarViewModel: PropertyChangedBase, IHandle<EditPlcMessage>
    {
        private readonly Sharp7PlcService _plcService;
        private readonly PlcViewModel _plcViewModel;
        private readonly IEventAggregator _eventAggregator;

        public HmiStatusBarViewModel(Sharp7PlcService plcService, PlcViewModel plcViewModel, IEventAggregator eventAggregator)
        {
            _plcService = plcService;
            _plcViewModel = plcViewModel;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            _plcService.ValuesUpdated += OnPlcServiceValueUpdated;

            Name = _plcViewModel.Name;
            IpAdress = _plcViewModel.IpAdress;
            Rack = _plcViewModel.Rack;
            Slot = _plcViewModel.Slot;
        }

        private void OnPlcServiceValueUpdated(object sender, EventArgs e)
        {
            ConnectionState = _plcService.ConnectionState;
            TimeScan = _plcService.ScanTtime;
        }

        public void Handle(EditPlcMessage message)
        {
            Name = message.EditedPlc.Name;
            IpAdress = message.EditedPlc.IpAdress;
            Rack = message.EditedPlc.Rack;
            Slot = message.EditedPlc.Slot;
        }

        private ConnectionStates _connectionState;
        public ConnectionStates ConnectionState
        {
            get { return _connectionState; }
            set
            {
                _connectionState = value;
                NotifyOfPropertyChange(() => ConnectionState);
            }
        }

        private TimeSpan timeSpan;
        public TimeSpan TimeScan
        {
            get { return timeSpan; }
            set
            {
                timeSpan = value;
                NotifyOfPropertyChange(() => TimeScan);
            }
        }

        private string _name;
        public string Name
        {
            get => $"Name: {_name} ";
            set => Set(ref _name, value);
        }

        private string _ipAdress;
        public string IpAdress
        {
            get => $"IpAdress: {_ipAdress} ";
            set => Set(ref _ipAdress, value);
        }

        private string _rack;
        public string Rack
        {
            get => $"Rack: {_rack} ";
            set => Set(ref _rack, value);
        }

        private string _slot;
        public string Slot
        {
            get => $"Slot: {_slot} ";
            set => Set(ref _slot, value);
        }
    }
}
