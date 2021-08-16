using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class CreatePlcViewModel: Screen
    {
        private readonly PlcListViewModel _plcListViewModel;

        public CreatePlcViewModel(PlcListViewModel plcListViewModel)
        {
            _plcListViewModel = plcListViewModel;
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

        protected override void OnActivate()
        {
            base.OnActivate();

            Name = "Default Name";
            IpAdress = "127.0.0.1";
            Rack = "0";
            Slot = "1";
        }

        public void ConfirmCreatePlc()
        {
            var plc = new Plc()
            {
                IpAdress = this.IpAdress,
                Name = this.Name,
                Rack = this.Rack,
                Slot = this.Slot,
                ControlList = new List<DefaultControl>()
            };
            _plcListViewModel.AddNewPlcViewModel(plc);

            TryClose();
        }
    }
}
