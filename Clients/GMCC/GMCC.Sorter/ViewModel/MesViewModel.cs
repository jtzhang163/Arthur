using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    /// <summary>
    /// 主设备
    /// </summary>
    public sealed class MesViewModel : CommorViewModel
    {

        private string host = null;
        public string Host
        {
            get
            {
                host = ((Server)this.Commor.Communicator).Host;
                return host;
            }
            set
            {
                if (host != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (Current.Mes == this)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).Host = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("交互平台. MES: [{0}] 修改为 [{1}]", host, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref host, value);
                    this.CommorInfo = null;
                }
            }
        }

        public MesViewModel(Commor commor) : base(commor)
        {

        }

        public void Comm()
        {
            if (this.Commor.Connected)
            {
                if (this.Commor.Comm("").IsOk)
                {

                }
                this.IsAlive = true;
            }
            else
            {
                this.IsAlive = false;
            }
        }
    }
}
