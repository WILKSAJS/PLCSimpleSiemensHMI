﻿using Caliburn.Micro;
using ClosedXML.Excel;
using Microsoft.Win32;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.Models;
using PLCSiemensSymulatorHMI.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace PLCSiemensSymulatorHMI.ViewModels
{
    public class CreateControlViewModel: Screen
    {
        private readonly Plc _plc;
        private readonly IEventAggregator _eventAggregator;
        private readonly IWindowManager _windowManager;

        public CreateControlViewModel(Plc plc, IEventAggregator eventAggregator, IWindowManager windowManager)
        {
            _plc = plc;
            _eventAggregator = eventAggregator;
            _windowManager = windowManager;
            ControlType = Enum.GetValues(typeof(ControlType)).Cast<ControlType>().ToList();

            //initialize
            ControlName = "";
            DataBlock = "";
            Index = "";
            Offset = "";
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            // when deactive, unsubscribe all events to free alocated data space
            foreach (ExcelViewModel excelViewModel in excelViewModels)
            {
                excelViewModel.DataChoosen -= OnDataChoosen;
            }
        }

        #region LeftSideOfView - Add control
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
            switch (SelectedControlType)
            {
                case Messages.ControlType.TankProgressBar:
                    // check index validation - rexgex - if not error message box
                    if (IndexForIntRealControlBlockRegex.IsMatch(Index))
                    {
                        _windowManager.ShowWindow(new CreateControlDetailViewModel(_eventAggregator, _windowManager,
                            // In this case DefaultControl object is only container for below listed data, there is no ID needed
                            new DefaultControl() {
                                ControlName = this.ControlName,
                                DataBlock = this.DataBlock.ToUpper(),
                                Index = this.Index.ToUpper(),
                                Offset = this.Offset == null ? "" : this.Offset.ToUpper(),
                                ControlType = this.SelectedControlType
                            }
                            ), null, null);
                    }
                    else
                    {
                        MessageBox.Show("If Tank ProgressBar has been choosen, only Index pattern for either Int or Real values are allowed (\"W\" or \"D\")", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                case Messages.ControlType.SliderControl:
                    // check index validation - rexgex - if not error message box
                    if (IndexForIntRealControlBlockRegex.IsMatch(Index))
                    {
                        _windowManager.ShowWindow(new CreateControlDetailViewModel(_eventAggregator, _windowManager,
                            // In this case DefaultControl object is only container for below listed data, there is no ID needed
                            new DefaultControl()
                            {
                                ControlName = this.ControlName,
                                DataBlock = this.DataBlock.ToUpper(),
                                Index = this.Index.ToUpper(),
                                Offset = this.Offset == null ? "" : this.Offset.ToUpper(),
                                ControlType = this.SelectedControlType
                            }
                            ), null, null);
                    }
                    else
                    {
                        MessageBox.Show("If Slider Control has been choosen, only Index pattern for either Int or Real values are allowed (\"W\" or \"D\")", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                default:
                    PublishOnUIThread();
                    break;
            }
            // Clear textboxes
            ControlName = "";
            DataBlock = "";
            Index = "";
            Offset = "";
            //TryClose(); if there is a need.
        }

        private void PublishOnUIThread()
        {
            _eventAggregator.PublishOnUIThread(new CreateControlMessage()
            {
                ControlName = this.ControlName,
                DataBlock = this.DataBlock.ToUpper(),
                Index = this.Index.ToUpper(),
                Offset = this.Offset == null ? "" : this.Offset.ToUpper(),
                ControlType = this.SelectedControlType
            });
        }

        //Validation
        public bool CanSubmit
        {
            get { return AreInputsValid(); }
        }

        Regex DataBlockRegex = new Regex("DB[0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexBlockRegex = new Regex("DB[XWD][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex IndexForIntRealControlBlockRegex = new Regex("DB[WD][0-9]{1,}", RegexOptions.IgnoreCase);
        Regex OffsetlockRegex = new Regex("[0-9]{1,}", RegexOptions.IgnoreCase);

        private bool AreInputsValid()
        {
            return !String.IsNullOrEmpty(ControlName)
                && (String.IsNullOrEmpty(DataBlock) ? false : DataBlockRegex.IsMatch(DataBlock))
                && (String.IsNullOrEmpty(Index) ? false : IndexBlockRegex.IsMatch(Index))
                && (String.IsNullOrEmpty(Offset) ? true : OffsetlockRegex.IsMatch(Offset));
        }

        public void TextChanged()
        {
            this.NotifyOfPropertyChange(nameof(CanSubmit));
        }

        #endregion

        #region RightSideOfView - Load Excel file
        public BindableCollection<ExcelViewModel> excelViewModels { get; set; } = new BindableCollection<ExcelViewModel>();
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set => Set(ref _filePath, value);
        }

        public void OpenFileDialogClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog.Multiselect = false;

            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    //Get the path of specified file
                    FilePath = openFileDialog.FileName;
                    if (!String.IsNullOrEmpty(FilePath))
                    {
                        var excelData = ExtractDataFromExcel(FilePath);
                        excelViewModels.Clear();
                        excelViewModels.AddRange(CreateExcelViewModels(excelData));
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private IEnumerable<ExcelViewModel> CreateExcelViewModels(string[,] excelData)
        {
            // mozna poprówbowac cos z yield lub z metodą rozszerzoną 
            for (int i = 0; i < excelData.GetLength(0); i++)
            {
                string[] firstCellData = excelData[i,0].Split('.');
                string[] secondCellData = excelData[i, 1].Replace('%', ' ').Trim().Split('.');
                var excelVM = new ExcelViewModel()
                {
                    ControlName = firstCellData.Length > 1 ? firstCellData[1] : String.IsNullOrEmpty(firstCellData[0]) ? "Default Name": firstCellData[0],
                    DataBlock = secondCellData != null ? secondCellData[0] : "",
                    Index = secondCellData.Length > 1 ? secondCellData[1] : "",
                    Offset = secondCellData.Length > 2 ? secondCellData[2] : ""

                };
                // subscribe to event rised when particular ExcelViewModel is choosen
                excelVM.DataChoosen += OnDataChoosen;
                yield return excelVM;
            }
        }

        private void OnDataChoosen(object sender, EventArgs e)
        {
            var excelViewModel = (ExcelViewModel)sender;
            // fill textBoxes with data form excel VM's
            ControlName = excelViewModel.ControlName;
            DataBlock = excelViewModel.DataBlock;
            Index = excelViewModel.Index;
            Offset = excelViewModel.Offset;
        }

        private string[,] ExtractDataFromExcel(string FilePath)
        {
            // this method is intended strictly for .xmls file which can be generated by PLC SIM
            // this is valid only for excel file based on DB strucute(type od data inside PLC)
            // data type like 'Tag' are not processed properly(mainly because method takes only first two columns of each sheet)
            using (XLWorkbook workbook = new XLWorkbook(FilePath))
            {
                var rows = workbook.Worksheet(1).RowsUsed();
                //bool isFirstRow = true;
                string[,] dataArray = new string[rows.Count(), 2];
                int _row = 0;
                int _cell = 0;

                // process each row and add do datalist
                foreach (IXLRow row in rows)
                {
                    foreach (IXLCell cell in row.Cells("1,2"))
                    {
                        dataArray[_row, _cell] = cell.Value.ToString();
                        _cell = _cell == 1 ? 0 : 1;
                    }
                    _row++;
                }

                return dataArray;
            }
        }

        #endregion
    }
}
