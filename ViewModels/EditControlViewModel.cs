using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.CustomControls.ViewModels;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class EditControlViewModel: Screen
    {
        protected readonly DefaultControl _defaultControl;
        protected readonly BaseControlViewModel _baseControlViewModel;

        public EditControlViewModel(BaseControlViewModel baseControlViewModel, DefaultControl defaultControl)
        {
            _baseControlViewModel = baseControlViewModel;
            _defaultControl = defaultControl;
        }

        #region properties

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
        #endregion


        protected override void OnActivate()
        {
            base.OnActivate();
            // initialize
            ControlName = _defaultControl.ControlName;
            DataBlock = _defaultControl.DataBlock;
            Index = _defaultControl.Index;
            Offset = _defaultControl.Offset;
        }

        public void Submit()
        {
            //Edit in repo
            _defaultControl.ControlName = this.ControlName;
            _defaultControl.DataBlock = this.DataBlock;
            _defaultControl.Index = this.Index;
            _defaultControl.Offset = this.Offset;
            //Edit in VM - only displayed name 
            _baseControlViewModel.ControlName = this.ControlName;
            _baseControlViewModel.DbBlockAdress = _defaultControl.DbBlockAdress;
            TryClose();
        }

        #region Validation
        Regex DataBlockRegex = new Regex("DB[0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexALLBlockRegex = new Regex("DB[XWD][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexBOOLBlockRegex = new Regex("DB[X][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexINTBlockRegex = new Regex("DB[W][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexREALBlockRegex = new Regex("DB[D][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexForIntRealControlBlockRegex = new Regex("DB[WD][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex OffsetlockRegex = new Regex("[0-9]{1,}", RegexOptions.IgnoreCase);

        public bool CanSubmit
        {
            get { return AreInputsValid(); }
        }

        public void TextChanged()
        {
            this.NotifyOfPropertyChange(nameof(CanSubmit));
        }

        private bool AreInputsValid()
        {
            return !String.IsNullOrEmpty(ControlName)
                && (String.IsNullOrEmpty(DataBlock) ? false : DataBlockRegex.IsMatch(DataBlock))
                && (String.IsNullOrEmpty(Index) ? false : IsIndexValid(_defaultControl.ControlType,Index))
                && (String.IsNullOrEmpty(Offset) ? true : OffsetlockRegex.IsMatch(Offset));
        }

        // check indexValidation which depends on Control.Type
        private bool IsIndexValid(ControlType controlType, string Index)
        {
            switch (controlType)
            {
                case ControlType.GreenSemaphore:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.OrangeSemaphore:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.RedSemaphore:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.BlueSemaphore:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.BistableButton:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.MonostableButton:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.RealTextBox:
                    return IndexREALBlockRegex.IsMatch(Index);
                case ControlType.IntegerTextbox:
                    return IndexINTBlockRegex.IsMatch(Index);
                case ControlType.EmergencyButton:
                    return IndexBOOLBlockRegex.IsMatch(Index);
                case ControlType.TankProgressBar:
                    return IndexForIntRealControlBlockRegex.IsMatch(Index);
                case ControlType.SliderControl:
                    return IndexForIntRealControlBlockRegex.IsMatch(Index);
                default:
                    return true;
            }
        }
        #endregion
    }
}
