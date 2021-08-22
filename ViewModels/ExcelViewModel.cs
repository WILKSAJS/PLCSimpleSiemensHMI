using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class ExcelViewModel : Screen
    {
        private string _controlName;
        public string ControlName
        {
            get => _controlName;
            set => Set(ref _controlName, value);
        }

        private string _dataBlock;
        public string DataBlock
        {
            get => _dataBlock;
            set => Set(ref _dataBlock, value);
        }

        private string _index;
        public string Index
        {
            get => _index;
            set => Set(ref _index, value);
        }

        private string _offset;
        public string Offset
        {
            get => _offset;
            set => Set(ref _offset, value);
        }
    }
}
