using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public abstract class CommorViewModel : BindableObject
    {

        public string Name
        {
            get
            {
                return this.Commor.Communicator.Name;
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
        }


        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                SetProperty(ref isEnabled, value);
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


        public Commor Commor { get; private set; }

        public CommorViewModel(Commor commor)
        {
            this.Commor = commor;
        }

        public virtual void Comm()
        {
            if (this.Commor.Connected)
            {

            }
        }
    }
}
