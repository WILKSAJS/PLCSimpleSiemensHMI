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
    public class CreateControlViewModel: Screen
    {
        private readonly Plc _plc;
        private readonly IEventAggregator _eventAggregator;

        public CreateControlViewModel(Plc plc, IEventAggregator eventAggregator)
        {
            _plc = plc;
            _eventAggregator = eventAggregator;
            ControlType = Enum.GetValues(typeof(ControlType)).Cast<ControlType>().ToList();
        }
        public IReadOnlyList<ControlType> ControlType { get; }

        private string _controlName;

        public string ControlName
        {
            get { return _controlName; }
            set => Set(ref _controlName, value);
        }

        private string _dataBlock;

        public string DataBlock
        {
            get { return _dataBlock; }
            set => Set(ref _dataBlock, value);
        }

        private string _index;

        public string Index
        {
            get { return _index; }
            set => Set(ref _index, value);
        }

        private string _offset;

        public string Offset
        {
            get { return _offset; }
            set => Set(ref _offset, value);
        }

        private ControlType _selectedcontrolType;

        public ControlType SelectedControlType
        {
            get { return _selectedcontrolType; }
            set => Set(ref _selectedcontrolType, value);
        }

        public void Submit()
        {
            _eventAggregator.PublishOnUIThread(new CreateControlMessage()
            {
                ControlName = this.ControlName,
                DataBlock = this.DataBlock.ToUpper(),
                Index = this.Index.ToUpper(),
                Offset = this.Offset == null ? "" : this.Offset.ToUpper(),
                ControlType = this.SelectedControlType
            });
            TryClose();
        }
    }
}
