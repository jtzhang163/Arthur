using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Data;
using GMCC.Sorter.Run;
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
    public class MainMachineViewModel : CommorViewModel
    {

        private string ip = null;
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
                    ((EthernetCommor)this.Commor.Communicator).IP = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0} IP地址: [{1}] 修改为 [{2}]", Name, ip, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref ip, value);
                    this.CommorInfo = null;
                }
            }
        }

        private int? port = null;
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
                    ((EthernetCommor)this.Commor.Communicator).Port = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0} 端口: [{1}] 修改为 [{2}]", Name, port, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref port, value);
                    this.CommorInfo = null;
                }
            }
        }

        /// <summary>
        /// 电池扫码准备就绪
        /// </summary>
        public bool IsBatteryScanReady { get; set; }

        /// <summary>
        /// 绑盘托盘扫码准备就绪
        /// </summary>
        public bool IsBindTrayScanReady { get; set; }

        /// <summary>
        /// 解盘托盘扫码准备就绪
        /// </summary>
        public bool IsUnbindTrayScanReady { get; set; }

        /// <summary>
        /// 横移上料完成
        /// </summary>
        public bool IsFeedingFinished { get; set; }

        /// <summary>
        /// 横移下料完成
        /// </summary>
        public bool IsBlankingFinished { get; set; }



        public MainMachineViewModel(Commor commor) : base(commor)
        {

        }

        public void Comm()
        {

            Console.WriteLine("MainMachineViewModel: Comm");
            if (this.Commor.Connected)
            {
                if (this.Commor.Comm("").IsOk)
                {
                   // this.IsBatteryScanReady = true;
                    //...
                }
            }
            else
            {
                this.IsAlive = false;
            }

            this.IsBatteryScanReady = true;
            this.IsAlive = true;
        }
    }
}
