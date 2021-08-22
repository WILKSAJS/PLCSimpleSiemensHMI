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
        private readonly IBasePlcRepository _plcRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        public PlcListViewModel(IBasePlcRepository plcRepository, IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            _windowManager = windowManager;
            _plcRepository = plcRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            // retrieve Plcs from Repo, and pass it for create each PlcViewModel then add to Item list - FOR THIS NESTED VM THERE IS NO BootstrapContener Registry!
            var repoPlcList = _plcRepository.GetAllPlc();
            if (repoPlcList.Count > 0)
            {
                PlcList.AddRange(repoPlcList.Select(x => CreateNewPlcVewModel(x)));
            }
        }

        private PlcViewModel CreateNewPlcVewModel(Plc plc)
        {
            var plcViewModel = new PlcViewModel(plc, _plcRepository, _eventAggregator, _windowManager);
            plcViewModel.PlcRemoved += OnPlcRemoved;
            return plcViewModel;
        }

        private void OnPlcRemoved(object sender, EventArgs e)
        {
            var plcViewModel = (PlcViewModel)sender;

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
