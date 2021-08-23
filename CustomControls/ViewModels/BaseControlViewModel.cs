using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.PlcService;
using PLCSiemensSymulatorHMI.Repository;
using PLCSiemensSymulatorHMI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public abstract class BaseControlViewModel:Screen
    {
        //private readonly Sharp7PlcService plcService;
        protected readonly IBasePlcRepository _plcRepository;
        protected readonly DefaultControl _defaultControl;
        protected readonly PlcViewModel _plcViewModel;
        protected readonly string _DbBlockAdress;

        protected UIElement dragObject = null;
        protected Canvas MyCanvas;
        protected Point start;
        protected Point origin;

        public BaseControlViewModel(IBasePlcRepository plcRepository, DefaultControl defaultControl, PlcViewModel plcViewModel)
        {
            _plcRepository = plcRepository;
            _defaultControl = defaultControl;
            _plcViewModel = plcViewModel;
            // to move and show name of control on desktop
            Y = _defaultControl.Y;
            X = _defaultControl.X;
            ControlName = _defaultControl.ControlName;
            // to perform control oepration by PlcService
            _DbBlockAdress = _defaultControl.DbBlockAdress;
        }
        public abstract Task PerformControlOperation(Sharp7PlcService plcService);
        public event EventHandler ControlRemoved;

        private string _controlName;
        public string ControlName
        {
            get { return _controlName; }
            set => Set(ref _controlName, value);
        }

        private double _Y;
        public double Y
        {
            get { return _Y; }
            set => Set(ref _Y, value);
        }

        private double _X;
        public double X
        {
            get { return _X; }
            set => Set(ref _X, value);
        }

        public void MouseDown(MouseButtonEventArgs eventArgs, ActionExecutionContext executionContext)
        {
            if (eventArgs.LeftButton == MouseButtonState.Pressed)
            {
                MyCanvas = UIHelper.UIHelper.FindChild<Canvas>(Application.Current.MainWindow, "MyCanvas");
                dragObject = executionContext.Source as UIElement;
                dragObject.CaptureMouse();
                start = eventArgs.GetPosition(MyCanvas);
                origin = new Point(X, Y);
            }
        }

        public void MouseMove(MouseEventArgs eventArgs)
        {
            if (dragObject != null)
            {
                if (dragObject.IsMouseCaptured)
                {
                    Vector v = start - eventArgs.GetPosition(MyCanvas);
                    X = origin.X - v.X;
                    Y = origin.Y - v.Y;

                    // ASSIGN NEW X Y TO POINT
                    _defaultControl.X = X;
                    _defaultControl.Y = Y;
                }
            }

        }

        public void MouseUp()
        {
            if (dragObject != null)
            {
                dragObject.ReleaseMouseCapture();
                dragObject = null;

                // SAVE NEW X Y POSITIONS
                _plcRepository.EditControl(_defaultControl, _plcViewModel.Id);
            }
        }

        public void RemoveControl()
        {
            try
            {
                _plcRepository.RemoveControl(_defaultControl, _plcViewModel.Id);
                //Notify ControlHolderVM to remove control from the list
                ControlRemoved?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
