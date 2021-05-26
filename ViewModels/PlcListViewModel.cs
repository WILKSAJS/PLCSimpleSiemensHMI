﻿using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class PlcListViewModel
    {
        private readonly PlcRepository _plcRepository;

        public PlcListViewModel(PlcRepository plcRepository)
        {
            _plcRepository = plcRepository;
            // retrieve Plcs from Repo, and pass it for create each PlcViewModel then add to Item list - FOR THIS NESTED VM THERE IS NO BootstrapContener Registry!
            PlcList.AddRange(_plcRepository.GetAllPlc().Select(x => CreateNewPlcVewModel(x)));
        }

        private PlcViewModel CreateNewPlcVewModel(Plc plc)
        {
            var plcViewModel = new PlcViewModel(plc, _plcRepository);
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

        public BindableCollection<PlcViewModel> PlcList { get; } = new BindableCollection<PlcViewModel>();
    }
}