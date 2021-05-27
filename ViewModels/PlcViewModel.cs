using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class PlcViewModel
    {
        private readonly Plc _plc;
        private readonly PlcRepository _plcRepository;

        public PlcViewModel(Plc plc, PlcRepository plcRepository)
        {
            _plc = plc;
            _plcRepository = plcRepository;
        }

        public string Id => _plc.Id;
        public string Name => _plc.Name;
        public string IpAdress => _plc.IpAdress;
        public string Rack => _plc.Rack;
        public string Slot => _plc.Slot;

        // Event to notify if Remove button is clicked - communication Between two VM's
        public event EventHandler PlcRemoved;
        public void Remove()
        {
            // TODO gona be async
            _plcRepository.RemovePlc(_plc);
            PlcRemoved?.Invoke(this, EventArgs.Empty);
        }
    }
}
