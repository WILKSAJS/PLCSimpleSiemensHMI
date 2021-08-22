using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PLCSiemensSymulatorHMI.Repository
{
    public class XmlPlcRepository : IBasePlcRepository
    {
        public XmlPlcRepository()
        {
            plcList = new List<Plc>();
            FilePath = "PLCSiemensHMIConfig.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(List<Plc>));
            if (File.Exists(FilePath))
            {
                using (TextReader myWriter = new StreamReader(FilePath))
                {
                    plcList = (List<Plc>)serializer.Deserialize(myWriter);
                    myWriter.Close();
                }
            }
        }
        private List<Plc> plcList;
        private string FilePath;
        public void SaveChanges()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Plc>));
            if (File.Exists(FilePath)) File.Delete(FilePath);
            using (TextWriter myWriter = new StreamWriter(FilePath))
            {
                serializer.Serialize(myWriter, plcList);
                myWriter.Close();
            }
        }

        #region PLC
        public IList<Plc> GetAllPlc()
        {
            return plcList;
        }
        public bool AddPlc(Plc plc)
        {
            plcList.Add(plc);
            return true;
        }
        public bool EditPlc(Plc plc)
        {
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
        public bool RemovePlc(Plc plc)
        {
            return plcList.Remove(plc);
        }
        #endregion

        #region Controls
        public IList<DefaultControl> GetAllControls(int plcId)
        {
            return plcList.Where(x => x.Id == plcId).FirstOrDefault().ControlList;
        }
        public bool AddControl(DefaultControl control, int plcId)
        {
            plcList.Where(x => x.Id == plcId).FirstOrDefault()
                   .ControlList.Add(control);
            return true;
        }
        public bool EditControl(DefaultControl control, int plcId)
        {
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
        public bool RemoveControl(DefaultControl control, int plcId)
        {
            return plcList.Where(x => x.Id == plcId).FirstOrDefault()
                          .ControlList.Remove(control);
        }
        #endregion  
    }
}
