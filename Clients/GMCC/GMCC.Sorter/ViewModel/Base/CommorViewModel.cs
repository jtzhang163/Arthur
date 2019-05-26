using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using Arthur.Core;
using Arthur.Core.Transfer;
using GMCC.Sorter.Data;
using GMCC.Sorter.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public abstract class CommorViewModel : BindableObject
    {

        public int Id => this.Commor.Communicator.Id;

        public Commor Commor { get; private set; }

        public CommorViewModel(Commor commor)
        {
            this.name = commor.Communicator.Name;
            this.company = commor.Communicator.Company;
            this.location = commor.Communicator.Location;
            this.modelNumber = commor.Communicator.ModelNumber;
            this.serialNumber = commor.Communicator.SerialNumber;
            this.isEnabled = commor.Communicator.IsEnabled;

            this.Commor = commor;

            this.Timer = new System.Threading.Timer(new TimerCallback(this.Comm), null, 5000, this.CommInterval);
        }

        private string name = null;
        public string Name
        {
            get
            {
                if(name == null)
                {
                    name = this.Commor.Communicator.Name;
                }
                return name;
            }
            set
            {
                if (name != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).Name = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).Name = value;
                        }
                        else if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).Name = value;
                        }
                        else if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).Name = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. [{0}] 名称修改为 [{1}]", name, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref name, value);
                }
            }
        }

        private string company = null;
        public string Company
        {
            get
            {
                if (company == null)
                {
                    company = this.Commor.Communicator.Company;
                }
                return company;
            }
            set
            {
                if (company != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).Company = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).Company = value;
                        }
                        else if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).Company = value;
                        }
                        else if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).Company = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}品牌: [{1}] 修改为 [{2}]", Name, company, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref company, value);
                }
            }
        }


        private string location = null;
        public string Location
        {
            get
            {
                if (location == null)
                {
                    location = this.Commor.Communicator.Location;
                }
                return location;
            }
            set
            {
                if (location != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).Location = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).Location = value;
                        }
                        else if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).Location = value;
                        }
                        else if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).Location = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}位置: [{1}] 修改为 [{2}]", Name, location, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref location, value);
                }
            }
        }


        private string modelNumber = null;
        public string ModelNumber
        {
            get
            {
                if (modelNumber == null)
                {
                    modelNumber = this.Commor.Communicator.ModelNumber;
                }
                return modelNumber;
            }
            set
            {
                if (modelNumber != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).ModelNumber = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).ModelNumber = value;
                        }
                        else if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).ModelNumber = value;
                        }
                        else if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).ModelNumber = value;
                        }
                        db.SaveChanges();
                    }

                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}型号: [{1}] 修改为 [{2}]", Name, modelNumber, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref modelNumber, value);
                }
            }
        }



        private string serialNumber = null;
        public string SerialNumber
        {
            get
            {
                if (serialNumber == null)
                {
                    serialNumber = this.Commor.Communicator.SerialNumber;
                }
                return serialNumber;
            }
            set
            {
                if (serialNumber != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).SerialNumber = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).SerialNumber = value;
                        }
                        else if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).SerialNumber = value;
                        }
                        else if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).SerialNumber = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}序列号: [{1}] 修改为 [{2}]", Name, serialNumber, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref serialNumber, value);
                }
            }
        }


        private bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                if (isEnabled != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).IsEnabled = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).IsEnabled = value;
                        }
                        else if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).IsEnabled = value;
                        }
                        else if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).IsEnabled = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}启用状态: [{1}] 修改为 [{2}]", Name, isEnabled, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref isEnabled, value);
                }
            }
        }


        private string commorInfo = null;
        public string CommorInfo
        {
            get
            {
                if (commorInfo == null)
                {
                    if (this.Commor.Communicator is EthernetCommor)
                    {
                        var ethernetCommor = (EthernetCommor)this.Commor.Communicator;
                        commorInfo = string.Format("{0} : {1}", ethernetCommor.IP, ethernetCommor.Port);
                    }
                    else if (this.Commor.Communicator is SerialCommor)
                    {
                        var serialCommor = (SerialCommor)this.Commor.Communicator;
                        commorInfo = string.Format("{0}", serialCommor.PortName);
                    }
                    else if (this.Commor.Communicator is Server)
                    {
                        var server = (Server)this.Commor.Communicator;
                        commorInfo = string.Format("{0}", server.Host);
                    }
                }
                return commorInfo;
            }
            set
            {
                SetProperty(ref commorInfo, value);
            }
        }

        private bool isAlive;
        public bool IsAlive
        {
            get => isAlive;
            set
            {
                SetProperty(ref isAlive, value);
            }
        }

        private string realtimeStatus = "尚未连接";
        public string RealtimeStatus
        {
            get
            {
                return realtimeStatus;
            }
            set
            {
                SetProperty(ref realtimeStatus, value);
            }
        }


        private int commInterval = -1;

        /// <summary>
        /// 上位机通讯时间间隔（单位：ms）
        /// </summary>
        public int CommInterval
        {
            get
            {
                if (commInterval < 0)
                {
                    commInterval = _Convert.To(Arthur.App.Business.Setting.GetOption(string.Format("CommInterval_{0}", this.Name)), -1);
                    if (commInterval < 0)
                    {
                        commInterval = 1000;
                        Arthur.App.Business.Setting.SetOption(string.Format("CommInterval_{0}", this.Name), commInterval.ToString(), string.Format("上位机-{0}通讯时间间隔", this.Name));
                    }
                }
                return commInterval;
            }
            set
            {
                if (commInterval != value)
                {
                    Arthur.App.Business.Setting.SetOption(string.Format("CommInterval_{0}", this.Name), value.ToString());
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}与上位机通讯间隔: [{1}] 修改为 [{2}]", Name, commInterval, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref commInterval, value);
                }
            }
        }


        private System.Threading.Timer Timer = null;// new System.Threading.Timer(new System.Threading.TimerCallback(obj.Method3), null, 0, 100);

        public virtual void Comm(object o)
        {
            if (!TimerExec.IsRunning || !this.IsEnabled)
                return;

            if (this is MainMachineViewModel)
            {
                (this as MainMachineViewModel).Comm();
            }
            else if (this is TrayScanerViewModel)
            {
                (this as TrayScanerViewModel).Comm();
            }
            else if (this is BatteryScanerViewModel)
            {
                (this as BatteryScanerViewModel).Comm();
            }
            else if (this is MesViewModel)
            {
                (this as MesViewModel).Comm();
            }
        }


        private bool selectedEnabled = true;
        public bool SelectedEnabled
        {
            get => selectedEnabled;
            set
            {
                SetProperty(ref selectedEnabled, value);
            }
        }
    }
}
