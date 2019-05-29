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
    public abstract class ServerCommorViewModel : CommorViewModel
    {

        public ServerCommorViewModel(Commor commor) : base(commor)
        {
            this.host = ((ServerCommor)commor.Communicator).Host;
        }

        private string host;
        public string Host
        {
            get
            {
                if(host == null)
                {
                    host = ((ServerCommor)this.Commor.Communicator).Host;
                }
                return host;
            }
            set
            {
                if (host != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (this == Current.Mes)
                        {
                            db.MESs.FirstOrDefault(o => o.Id == this.Id).Host = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}服务器: [{1}] 修改为 [{2}]", Name, host, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref host, value);
                    this.CommorInfo = null;
                }
            }
        }
    }
}
