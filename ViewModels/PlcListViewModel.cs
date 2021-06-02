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
    public class PlcListViewModel: IHandle<EditPlcMessage>
    {
        private readonly PlcRepository _plcRepository;
        private readonly IEventAggregator _eventAggregator;

        public PlcListViewModel(PlcRepository plcRepository, IEventAggregator eventAggregator)
        {
            _plcRepository = plcRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            // retrieve Plcs from Repo, and pass it for create each PlcViewModel then add to Item list - FOR THIS NESTED VM THERE IS NO BootstrapContener Registry!
            PlcList.AddRange(_plcRepository.GetAllPlc().Select(x => CreateNewPlcVewModel(x)));
        }

        private PlcViewModel CreateNewPlcVewModel(Plc plc)
        {
            var plcViewModel = new PlcViewModel(plc, _plcRepository, _eventAggregator);
            plcViewModel.PlcRemoved += OnPlcRemoved;
            return plcViewModel;
        }

        private void OnPlcRemoved(object sender, EventArgs e)
        {
            var plcViewModel = (PlcViewModel)sender;

            // _plcRepository.RemovePlc(); -- gona be async so is no need to perform this here, better in PlcViewModel

            // Need to be unsubscribe after delete operation
            plcViewModel.PlcRemoved -= OnPlcRemoved;

            PlcList.Remove(plcViewModel);
        }

        public BindableCollection<PlcViewModel> PlcList { get; set;  } = new BindableCollection<PlcViewModel>();

        public void NaviToPLCreatorView()
        {
            _eventAggregator.PublishOnUIThread(new NavigateMessage() { CurrentPage = CurrentPage.CreatePlcPage });
        }

        public void AddNewPlcViewModel(Plc plc)
        {
            // Find out last Id and assign next one to new plc
            var nextId = PlcList.Count == 0 ? 1 : PlcList.OrderBy(x => x.Id).LastOrDefault().Id + 1;
            plc.Id = nextId;
            // add to repo
            _plcRepository.AddPlc(plc);
            //add to list
            var plcViewModel = CreateNewPlcVewModel(plc);
            PlcList.Add(plcViewModel);
        }


        public void EditPlcViewModel(Plc plc, PlcViewModel plcViewModel)
        {
            
        }

        public void Handle(EditPlcMessage message)
        {
            _plcRepository.EditPlc(message.EditedPlc);
            var index = PlcList.IndexOf(message.PlcViewModel);
            PlcList.Remove(PlcList.SingleOrDefault(x => x.Id == message.EditedPlc.Id));
            //safety
            index = index >= 0 ? index : PlcList.Count() - 1;

            PlcList.Insert(index, CreateNewPlcVewModel(message.EditedPlc));
        }
    }
}
