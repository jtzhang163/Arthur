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
    public abstract class CommorViewModel : BindableObject
    {

        public int Id => this.Commor.Communicator.Id;


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
                    this.Commor.Communicator.Name = value;
                    Context.AppContext.SaveChanges();
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
                    this.Commor.Communicator.Company = value;
                    Context.AppContext.SaveChanges();
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
                    this.Commor.Communicator.Location = value;
                    Context.AppContext.SaveChanges();
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
                    this.Commor.Communicator.ModelNumber = value;
                    Context.AppContext.SaveChanges();
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
                    this.Commor.Communicator.SerialNumber = value;
                    Context.AppContext.SaveChanges();
                    SetProperty(ref serialNumber, value);
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


        private bool isEnabled;
        public bool IsEnabled
        {
            get
            {
                isEnabled = this.Commor.Communicator.IsEnabled;
                return isEnabled;
            }
            set
            {
                if(isEnabled != value)
                {
                    this.Commor.Communicator.IsEnabled = value;
                    Context.AppContext.SaveChanges();
                    SetProperty(ref isEnabled, value);
                }
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
