using Caliburn.Micro;
using PLCSiemensSymulatorHMI.CustomControls.Models;
using PLCSiemensSymulatorHMI.Messages;
using PLCSiemensSymulatorHMI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace PLCSiemensSymulatorHMI.Repository
{
    public class XmlPlcRepository : IBasePlcRepository, IHandle<ChangeFilePathMessage>
    {
        private List<Plc> plcList;
        private string FilePath;
        private readonly IEventAggregator _eventAggregator;
        public event EventHandler RepositoryReloaded;

        public XmlPlcRepository(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            plcList = new List<Plc>();

            //take a path from config file
            FilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];

            LoadConfiguration(FilePath);
        }
        
        // event to inform PlcListVM to reload list
        protected virtual void OnRepositoryReloaded()
        {
            RepositoryReloaded?.Invoke(this, EventArgs.Empty);
        }

        private void LoadConfiguration(string FilePath)
        {
            try
            {
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                plcList = new List<Plc>();
            }
        }
        
        public void SaveChanges()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Plc>));
       
            try
            {
                if (File.Exists(FilePath)) File.Delete(FilePath); //remove old XML file

                using (TextWriter myWriter = new StreamWriter(FilePath)) //create new one
                {
                    serializer.Serialize(myWriter, plcList);
                    myWriter.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public void Handle(ChangeFilePathMessage message)
        {
            if (!String.IsNullOrEmpty(message?.FilePath))
            {
                FilePath = message.FilePath; // change path for new one

                // save new path for configuration file
                try
                {
                    Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    configuration.AppSettings.Settings["ConfigFilePath"].Value = FilePath;
                    configuration.Save(ConfigurationSaveMode.Full, true);
                    ConfigurationManager.RefreshSection("appSettings");

                    // load new configuraton
                    LoadConfiguration(FilePath);

                    OnRepositoryReloaded();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                    FilePath = System.Configuration.ConfigurationManager.AppSettings["ConfigFilePath"];
                }
            }
        }

        #region PLC
        public IList<Plc> GetAllPlc()
        {
            return plcList;
        }
        public bool AddPlc(Plc plc)
        {
            plcList?.Add(plc);
            return true;
        }
        public bool EditPlc(Plc plc)
        {
            plcList?.Where(x => x.Id == plc.Id)
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
            return plcList?.Remove(plc) == null ? false : true;
        }
        #endregion

        #region Controls
        public IList<DefaultControl> GetAllControls(int plcId)
        {
            return plcList?.Where(x => x.Id == plcId).FirstOrDefault().ControlList;
        }
        public bool AddControl(DefaultControl control, int plcId)
        {
            plcList?.Where(x => x.Id == plcId).FirstOrDefault()
                   .ControlList.Add(control);
            return true;
        }
        public bool EditControl(DefaultControl control, int plcId)
        {
            // there is no need to implement edit control because ediition is made
            // in runetime through DefaultControl _defaultControl object in particular VM's
            // and saved when program is collapsing
            //plcList.Where(x => x.Id == plcId).FirstOrDefault()
            //       .ControlList.Where(x => x.Id == control.Id)
            //       .Select(x => {
            //           x.ControlName = control.ControlName;
            //           x.DataBlock = control.DataBlock;
            //           x.Index = control.Index;
            //           x.Offset = control.Offset;
            //           x.X = control.X;
            //           x.Y = control.Y;
            //           return x;
            //       });
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
