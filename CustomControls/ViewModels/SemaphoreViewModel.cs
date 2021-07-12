using Caliburn.Micro;
using PLCSiemensSymulatorHMI.Converters;
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
using System.Windows.Media;
using System.Windows.Shapes;

namespace PLCSiemensSymulatorHMI.CustomControls.ViewModels
{
    public class SemaphoreViewModel:Screen
    {
        //private readonly Sharp7PlcService plcService;
        private readonly PlcRepository _plcRepository;
        private readonly DefaultControl _defaultControl;
        private readonly IEventAggregator _eventAggregator;
        private readonly PlcViewModel _plcViewModel;

        public SemaphoreViewModel(BrushConverterColours brushConverterColours, PlcRepository plcRepository, DefaultControl defaultControl, IEventAggregator eventAggregator, PlcViewModel plcViewModel)
        {
            // set colour for semaphore every time if crate new
            //SemaphoreColour = brushConverterColours;
            //this.plcService = plcService;
            _plcRepository = plcRepository;
            _defaultControl = defaultControl;
            _eventAggregator = eventAggregator;
            _plcViewModel = plcViewModel;
            this.SemaphoreColour = brushConverterColours;

            Y = _defaultControl.Y;
            X = _defaultControl.X;

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

        private bool _semaphoreState;
        public bool SemaphoreState
        {
            get { return _semaphoreState; }
            set => Set(ref _semaphoreState, value);
        }

        private BrushConverterColours _semaphoreColour;
        public BrushConverterColours SemaphoreColour
        {
            get { return _semaphoreColour; }
            set => Set(ref _semaphoreColour, value);
        }


        UIElement dragObject = null;
        Canvas MyCanvas;
        Point start;
        Point origin;

        public void MouseDown(object sender, MouseButtonEventArgs eventArgs, object dataContext, object source, object view, ActionExecutionContext executionContext)
        {
            MyCanvas = UIHelper.UIHelper.FindChild<Canvas>(Application.Current.MainWindow, "MyCanvas");
            dragObject = executionContext.Source as UIElement;
            dragObject.CaptureMouse();
            start = eventArgs.GetPosition(MyCanvas);
            origin = new Point(X,Y);
        }


        public void MouseMove(object sender, MouseEventArgs eventArgs, object dataContext, object source, object view, ActionExecutionContext executionContext)
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

        public void MouseUp(object sender, MouseButtonEventArgs eventArgs, object dataContext, object source, object view, ActionExecutionContext executionContext)
        {
            dragObject.ReleaseMouseCapture();
            dragObject = null;

            // SAVE NEW X Y POSITIONS
            _plcRepository.EditControl(_defaultControl, _plcViewModel.Id);

        }

        //protected override void OnActivate()
        //{
        //    //plcService.ValuesUpdated += PlcService_ValuesUpdated;

        //    base.OnActivate();
        //}

        //protected override void OnDeactivate(bool close)
        //{
        //    plcService.ValuesUpdated -= PlcService_ValuesUpdated;
        //    base.OnDeactivate(close);
        //}


        //private void PlcService_ValuesUpdated(object sender, EventArgs e)
        //{
        //    var service = (Sharp7PlcService)sender;
        //    SemaphoreState = service.ReadBitValue(dbAdress);
        //}



    }
}
