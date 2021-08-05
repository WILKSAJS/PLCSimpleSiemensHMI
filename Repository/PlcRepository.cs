using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Repository
{
    public class PlcRepository
    {
        // TEMPORARY
        private IList<Plc> plcList = new List<Plc> {
            new Plc{ Id = 1, Name = "Plc1", IpAdress = "127.0.0.1", Rack = "0", Slot = "1", ControlList = new List<DefaultControl>(){
                    new SemaphoreControl()
                    {
                         Id = 1, ControlName="SemGreen1", ControlType = Messages.ControlType.GreenSemaphore, DataBlock = "DB1", Index = "DBX8", Offset = "1", SemaphoreColour = Converters.BrushConverterColours.Green, SemaphoreState = false, X = 0, Y = 0
                    },
                    new SemaphoreControl()
                    {
                         Id = 2, ControlName="SemRed1", ControlType = Messages.ControlType.RedSemaphore, DataBlock = "DB1", Index = "DBX8", Offset = "3", SemaphoreColour = Converters.BrushConverterColours.Red, SemaphoreState = false, X = 100, Y = 100
                    }
                    ,
                    new MonostableButton()
                    {
                         Id = 2, ControlName="On Buton", ControlType = Messages.ControlType.MonostableButton, DataBlock = "DB1", Index = "DBX8", Offset = "0", X = 150, Y = 150
                    }
                } 
            },
             new Plc{ Id = 2, Name = "Plc2", IpAdress = "127.0.0.1", Rack = "0", Slot = "1", ControlList = new List<DefaultControl>(){
                    new SemaphoreControl()
                    {
                         Id = 1, ControlName="SemGreen1", ControlType = Messages.ControlType.GreenSemaphore, DataBlock = "DB1", Index = "DBX8", Offset = "1", SemaphoreColour = Converters.BrushConverterColours.Green, SemaphoreState = false, X = 0, Y = 0
                    },
                    new SemaphoreControl()
                    {
                         Id = 2, ControlName="SemRed1", ControlType = Messages.ControlType.RedSemaphore, DataBlock = "DB1", Index = "DBX8", Offset = "3", SemaphoreColour = Converters.BrushConverterColours.Red, SemaphoreState = false, X = 100, Y = 100
                    }
                }
            }
        };

        #region PLC
        // TODO need to be async if perform I/O operations
        public IList<Plc> GetAllPlc()
        {
            return plcList;
        }

        public bool RemovePlc(Plc plc)
        {
            // TODO REMOVE PLC FORM Db/xml file
            return plcList.Remove(plc);
        }

        public bool AddPlc(Plc plc)
        {
            // TODO ADD PLC TO Db/xml file
            plcList.Add(plc);

            return true;
        }

        public bool EditPlc(Plc plc)
        {
            // TODO ADD PLC TO Db/xml file
            plcList.Where(x => x.Id == plc.Id)
                   .Select(x => {
                        x.Name = plc.Name;
                        x.IpAdress = plc.IpAdress;
                        x.Rack = plc.Rack;
                        x.Slot = plc.Slot;
                        return x;
                   });

            return true;
        }

        #endregion

        #region Controls

        // TODO need to be async if perform I/O operations
        public IList<DefaultControl> GetAllControls(int plcId)
        {
            return plcList.Where(x => x.Id == plcId).FirstOrDefault().ControlList;
        }

        public bool RemoveControl(DefaultControl control, int plcId)
        {
            // TODO REMOVE PLC FORM Db/xml file
            return plcList.Where(x => x.Id == plcId).FirstOrDefault()
                    .ControlList.Remove(control);
        }

        public bool AddControl(DefaultControl control, int plcId)
        {
            // TODO ADD PLC TO Db/xml file
            plcList.Where(x => x.Id == plcId).FirstOrDefault()
                .ControlList.Add(control);

            return true;
        }

        public bool EditControl(DefaultControl control, int plcId)
        {
            // TODO ADD PLC TO Db/xml file
            plcList.Where(x => x.Id == plcId).FirstOrDefault()
                   .ControlList.Where(x => x.Id == control.Id)
                   .Select(x => {
                       x.ControlName = control.ControlName;
                       x.DataBlock = control.DataBlock;
                       x.Index = control.Index;
                       x.Offset = control.Offset;
                       x.Y = control.Y;
                       x.X = control.X;
                       return x;
                   });

            return true;
        }
        #endregion
    }
}
