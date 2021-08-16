using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSiemensSymulatorHMI.Repository
{
    public interface IBasePlcRepository
    {
        IList<Plc> GetAllPlc();
        bool RemovePlc(Plc plc);
        bool AddPlc(Plc plc);
        bool EditPlc(Plc plc);

        IList<DefaultControl> GetAllControls(int plcId);
        bool RemoveControl(DefaultControl control, int plcId);
        bool AddControl(DefaultControl control, int plcId);
        bool EditControl(DefaultControl control, int plcId);

        void SaveChanges();
    }
}
