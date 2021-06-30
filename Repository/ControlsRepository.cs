using PLCSiemensSymulatorHMI.CustomControls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Repository
{
    public class ControlsRepository
    {
        // TEMPORARY
        private IList<DefaultControl> ControlList = new List<DefaultControl> {
            new SemaphoreControl()
            {
                 Id = 1, ControlName="SemGreen1", ControlType = Messages.ControlType.GreenSemaphore, DataBlock = "DB1", Index = "Index1", Offset = "Offset1", SemaphoreColour = Converters.BrushConverterColours.Green, SemaphoreState = false, X = 0, Y = 0
            },
            new SemaphoreControl()
            {
                 Id = 2, ControlName="SemRed1", ControlType = Messages.ControlType.RedSemaphore, DataBlock = "DB2", Index = "Index2", Offset = "Offset2", SemaphoreColour = Converters.BrushConverterColours.Red, SemaphoreState = false, X = 100, Y = 100
            }
        };

        // TODO need to be async if perform I/O operations
        public IList<DefaultControl> GetAllControls()
        {
            return ControlList;
        }

        public bool RemoveControl(DefaultControl control)
        {
            // TODO REMOVE PLC FORM Db/xml file

            return ControlList.Remove(control);
        }

        public bool AddControl(DefaultControl control)
        {
            // TODO ADD PLC TO Db/xml file
            ControlList.Add(control);

            return true;
        }

        public bool EditControl(DefaultControl control)
        {
            // TODO ADD PLC TO Db/xml file
            ControlList.Where(x => x.Id == control.Id)
                   .Select(x => {
                       x.ControlName = control.ControlName;
                       x.DataBlock = control.DataBlock;
                       x.Index = control.Index;
                       x.Offset = control.Offset;
                       return x;
                   });

            return true;
        }
    }
}
