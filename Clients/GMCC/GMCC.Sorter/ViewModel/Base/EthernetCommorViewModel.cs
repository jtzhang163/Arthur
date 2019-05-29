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
    public abstract class EthernetCommorViewModel : CommorViewModel
    {

        public EthernetCommorViewModel(Commor commor) : base(commor)
        {
            this.ip = ((EthernetCommor)commor.Communicator).IP;
            this.port = ((EthernetCommor)commor.Communicator).Port;
        }

        private string ip;
        public string IP
        {
            get
            {
                if(ip == null)
                {
                    ip = ((EthernetCommor)this.Commor.Communicator).IP;
                }
                return ip;
            }
            set
            {
                if (ip != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).IP = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).IP = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}IP: [{1}] 修改为 [{2}]", Name, ip, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref ip, value);
                    this.CommorInfo = null;
                }
            }
        }

        private int? port;
        public int Port
        {
            get
            {
                if (port == null)
                {
                    port = ((EthernetCommor)this.Commor.Communicator).Port;
                }
                return port.Value;
            }
            set
            {
                if (port != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.MainMachine)
                        {
                            db.PLCs.FirstOrDefault(o => o.Id == this.Id).Port = value;
                        }
                        else if (this == Current.BatteryScaner)
                        {
                            db.BatteryScaners.FirstOrDefault(o => o.Id == this.Id).Port = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}端口: [{1}] 修改为 [{2}]", Name, port, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref port, value);
                    this.CommorInfo = null;
                }
            }
        }

    }
}
