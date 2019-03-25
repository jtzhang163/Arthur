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
    public class MesViewModel : CommorViewModel
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
                    ((Server)this.Commor.Communicator).Host = value;
                    Context.AppContext.SaveChanges();
                    SetProperty(ref host, value);

                    this.CommorInfo = null;
                }
            }
        }

        public MesViewModel(Commor commor) : base(commor)
        {

        }

        public override void Comm(object o)
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
