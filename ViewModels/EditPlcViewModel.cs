using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class EditPlcViewModel: Screen
    {
        private readonly Plc _plc;
        private readonly PlcListViewModel _plcListViewModel;
        private readonly PlcViewModel _plcViewModel;
        public string Tittle { get; set; }

        public EditPlcViewModel(Plc plc, PlcListViewModel plcListViewModel, PlcViewModel plcViewModel)
        {
            _plc = plc;
            _plcListViewModel = plcListViewModel;
            _plcViewModel = plcViewModel;
        }

        protected override void OnActivate()
        {
            Name = _plc.Name;
            IpAdress = _plc.IpAdress;
            Rack = _plc.Rack;
            Slot = _plc.Slot;

            Tittle = $"Edit PLC: {Name}";
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _ipAdress;
        public string IpAdress
        {
            get => _ipAdress;
            set => Set(ref _ipAdress, value);
        }

        private string _rack;
        public string Rack
        {
            get => _rack;
            set => Set(ref _rack, value);
        }

        private string _slot;

        public string Slot
        {
            get => _slot;
            set => Set(ref _slot, value);
        }

        public void ConfirmEditPlc()
        {
            _plc.IpAdress = IpAdress;
            _plc.Name = Name;
            _plc.Rack = Rack;
            _plc.Slot = Slot;

            _plcListViewModel.EditPlcViewModel(_plc, _plcViewModel);

            TryClose();
        }
    }
}
