using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.Helper.ComServer
{
    public class AppViewModel : BindableObject
    {
        private bool isRunning = false;

        public bool IsRunning
        {
            get => isRunning;
            set => SetProperty(ref isRunning, value);
        }

        private string tip = string.Empty;

        public string Tip
        {
            get => tip;
            set => SetProperty(ref tip, value);
        }


        private string ip = string.Empty;

        public string IP
        {
            get
            {
                if (string.IsNullOrEmpty(ip))
                {
                    ip = ConfigurationManager.AppSettings["IP"];
                }
                return ip;
            }
            set
            {
                if (!ip.Equals(value))
                {
                    AppHelper.SetConfig("IP", value);
                    SetProperty(ref ip, value);
                }
            }
        }

        private int port = -1;

        public int Port
        {
            get
            {
                if (port < 0)
                {
                    port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                }
                return port;
            }
            set
            {
                if (port != value)
                {
                    AppHelper.SetConfig("Port", value.ToString());
                    SetProperty(ref port, value);
                }
            }
        }

        private string receive1 = string.Empty;

        public string Receive1
        {
            get
            {
                if (string.IsNullOrEmpty(receive1))
                {
                    receive1 = ConfigurationManager.AppSettings["Receive1"];
                }
                return receive1;
            }
            set
            {
                if (!receive1.Equals(value))
                {
                    AppHelper.SetConfig("Receive1", value);
                    SetProperty(ref receive1, value);
                }
            }
        }

        private string send1 = string.Empty;

        public string Send1
        {
            get
            {
                if (string.IsNullOrEmpty(send1))
                {
                    send1 = ConfigurationManager.AppSettings["Send1"];
                }
                return send1;
            }
            set
            {
                if (!send1.Equals(value))
                {
                    AppHelper.SetConfig("Send1", value);
                    SetProperty(ref send1, value);
                }
            }
        }


        private string receive2 = string.Empty;

        public string Receive2
        {
            get
            {
                if (string.IsNullOrEmpty(receive2))
                {
                    receive2 = ConfigurationManager.AppSettings["Receive2"];
                }
                return receive2;
            }
            set
            {
                if (!receive2.Equals(value))
                {
                    AppHelper.SetConfig("Receive2", value);
                    SetProperty(ref receive2, value);
                }
            }
        }

        private string send2 = string.Empty;

        public string Send2
        {
            get
            {
                if (string.IsNullOrEmpty(send2))
                {
                    send2 = ConfigurationManager.AppSettings["Send2"];
                }
                return send2;
            }
            set
            {
                if (!send2.Equals(value))
                {
                    AppHelper.SetConfig("Send2", value);
                    SetProperty(ref send2, value);
                }
            }
        }


        private string receive3 = string.Empty;

        public string Receive3
        {
            get
            {
                if (string.IsNullOrEmpty(receive3))
                {
                    receive3 = ConfigurationManager.AppSettings["Receive3"];
                }
                return receive3;
            }
            set
            {
                if (!receive3.Equals(value))
                {
                    AppHelper.SetConfig("Receive3", value);
                    SetProperty(ref receive3, value);
                }
            }
        }

        private string send3 = string.Empty;

        public string Send3
        {
            get
            {
                if (string.IsNullOrEmpty(send3))
                {
                    send3 = ConfigurationManager.AppSettings["Send3"];
                }
                return send3;
            }
            set
            {
                if (!send3.Equals(value))
                {
                    AppHelper.SetConfig("Send3", value);
                    SetProperty(ref send3, value);
                }
            }
        }


        private string receive4 = string.Empty;

        public string Receive4
        {
            get
            {
                if (string.IsNullOrEmpty(receive4))
                {
                    receive4 = ConfigurationManager.AppSettings["Receive4"];
                }
                return receive4;
            }
            set
            {
                if (!receive4.Equals(value))
                {
                    AppHelper.SetConfig("Receive4", value);
                    SetProperty(ref receive4, value);
                }
            }
        }

        private string send4 = string.Empty;

        public string Send4
        {
            get
            {
                if (string.IsNullOrEmpty(send4))
                {
                    send4 = ConfigurationManager.AppSettings["Send4"];
                }
                return send4;
            }
            set
            {
                if (!send4.Equals(value))
                {
                    AppHelper.SetConfig("Send4", value);
                    SetProperty(ref send4, value);
                }
            }
        }

    }
}
