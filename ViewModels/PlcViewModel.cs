using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class PlcViewModel: Screen
    {
        private readonly Plc _plc;
        private readonly IBasePlcRepository _plcRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        public PlcViewModel(Plc plc, IBasePlcRepository plcRepository, IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            _plc = plc;
            _plcRepository = plcRepository;
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
        }
        public int Id => _plc.Id;
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
            _plcRepository.SaveChanges();
            PlcRemoved?.Invoke(this, EventArgs.Empty);
        }

        public void AddControl()
        {
            _windowManager.ShowWindow(new CreateControlViewModel(_plc, _eventAggregator, _windowManager), null, null);
        }

        public void NaviToPLCEditView()
        {
            _eventAggregator.PublishOnUIThread(new NavigateMessage()
            {
                CurrentPage = CurrentPage.EditPlcPage,
                Plc = _plc,
                Sender = this
            });
        }

        public void NavigateToControlPage()
        {
            _eventAggregator.PublishOnUIThread(new NavigateMessage()
            {
                CurrentPage = CurrentPage.ControlPage,
                Plc = _plc,
                Sender = this
            });
        }
    }
}
