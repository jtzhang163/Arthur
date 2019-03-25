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
    public class BatteryScanerViewModel : CommorViewModel
    {


        private string ip = null;
        public string IP
        {
            get
            {
                ip = ((EthernetCommor)this.Commor.Communicator).IP;
                return ip;
            }
            set
            {
                if (ip != value)
                {
                    ((EthernetCommor)this.Commor.Communicator).IP = value;
                    Context.AppContext.SaveChanges();
                    SetProperty(ref ip, value);

                    this.CommorInfo = null;
                }
            }
        }


        private string scanCommand = null;
        /// <summary>
        /// 扫码指令
        /// </summary>
        public string ScanCommand
        {
            get
            {
                if (scanCommand == null)
                {
                    scanCommand = Arthur.Business.Application.GetOption("ScanCommand_BatteryScaner");
                    if (scanCommand == null)
                    {
                        scanCommand = "T";
                        Arthur.Business.Application.SetOption("ScanCommand_BatteryScaner", scanCommand, this.Name + "扫码指令");
                    }
                }
                return scanCommand;
            }
            set
            {
                if (scanCommand != value)
                {
                    Arthur.Business.Application.SetOption("ScanCommand_BatteryScaner", value);

                    SetProperty(ref scanCommand, value);
                }
            }
        }

        public BatteryScanerViewModel(Commor commor) : base(commor)
        {

        }

        public override void Comm(object o)
        {
            if (this.Commor.Connected)
            {
                if (Current.MainMachine.IsAlive && Current.MainMachine.IsBatteryScanReady)
                {
                    var ret = this.Commor.Comm(this.ScanCommand);
                    if (ret.IsOk)
                    {
                        Console.WriteLine(ret.Data);
                    }
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
