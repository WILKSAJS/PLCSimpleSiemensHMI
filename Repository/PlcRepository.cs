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
            new Plc{ Id = 1, Name = "Plc1", IpAdress = "127.0.0.1", Rack = "0", Slot = "1" },
            new Plc{ Id = 2, Name = "Plc2 Test", IpAdress = "127.0.0.1", Rack = "0", Slot = "1" },
            new Plc{ Id = 3, Name = "Plc3 Test", IpAdress = "127.0.0.1", Rack = "0", Slot = "1" }
        };

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
    }
}
