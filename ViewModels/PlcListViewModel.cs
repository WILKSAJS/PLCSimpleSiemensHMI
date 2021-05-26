using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class PlcListViewModel : Screen
    {
        private readonly PlcRepository _plcRepository;

        public PlcListViewModel(PlcRepository plcRepository)
        {
            _plcRepository = plcRepository;
            // retrieve Plcs from Repo, and pass it for create each PlcViewModel then add to Item list - FOR THIS NESTED VM THERE IS NO BootstrapContener Registry!
            PlcList.AddRange(_plcRepository.GetAllPlc().Select(x => new PlcViewModel(x)));
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            
        }

        public BindableCollection<PlcViewModel> PlcList { get; } = new BindableCollection<PlcViewModel>();
    }
}
