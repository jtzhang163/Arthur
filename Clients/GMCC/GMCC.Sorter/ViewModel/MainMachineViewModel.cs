using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
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
                if (ip == null)
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


        private int jawPos = -2;
        /// <summary>
        /// 横移位置
        /// </summary>
        public int JawPos
        {
            get
            {
                if (jawPos == -2)
                {
                    jawPos = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("JawPos"), -1);
                    if (jawPos == -1)
                    {
                        jawPos = 0;
                        Arthur.Business.Application.SetOption("JawPos", jawPos.ToString(), "横移位置");
                    }
                }
                return jawPos;
            }
            set
            {
                if (value > jawPos)
                {
                    this.JawStatus = "» 移动 »";
                }
                else if(value < jawPos)
                {
                    this.JawStatus = "« 移动 «";
                }
                else
                {
                    this.JawStatus = "闲置";
                }

                if (jawPos != value)
                {
                    Arthur.Business.Application.SetOption("JawPos", value.ToString());
                    SetProperty(ref jawPos, value);
                }
            }
        }


        private int stillTimeSpan = -2;
        /// <summary>
        /// 静置时间(min)
        /// </summary>
        public int StillTimeSpan
        {
            get
            {
                if (stillTimeSpan == -2)
                {
                    stillTimeSpan = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("StillTimeSpan"), -1);
                    if (stillTimeSpan == -1)
                    {
                        stillTimeSpan = 0;
                        Arthur.Business.Application.SetOption("StillTimeSpan", jawPos.ToString(), "静置时间(min)");
                    }
                }
                return stillTimeSpan;
            }
            set
            {
                if (stillTimeSpan != value)
                {
                    Arthur.Business.Application.SetOption("StillTimeSpan", value.ToString());
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0} 静置时间: [{1}] 修改为 [{2}]", Name, stillTimeSpan, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stillTimeSpan, value);
                }
            }
        }



        private string jawStatus = "闲置";
        /// <summary>
        /// 横移状态
        /// </summary>
        public string JawStatus
        {
            get
            {
                return jawStatus;
            }
            set
            {
                if (jawStatus != value)
                {
                    SetProperty(ref jawStatus, value);
                }
            }
        }


        private string jawTrayCode = null;
        /// <summary>
        /// 横移托盘条码
        /// </summary>
        public string JawTrayCode
        {
            get
            {
                if (jawTrayCode == null)
                {
                    jawTrayCode = Arthur.Business.Application.GetOption("JawTrayCode");
                    if (jawTrayCode == null)
                    {
                        jawTrayCode = "";
                        Arthur.Business.Application.SetOption("JawTrayCode", jawTrayCode, "横移名称");
                    }
                }
                return jawTrayCode;
            }
            set
            {
                if (jawTrayCode != value)
                {
                    Arthur.Business.Application.SetOption("JawTrayCode", value);
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. 横移托盘条码: [{0}] 修改为 [{1}]", jawTrayCode, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref jawTrayCode, value);
                }
            }
        }

        private bool isBatteryScanReady;
        /// <summary>
        /// 电池扫码准备就绪
        /// </summary>
        public bool IsBatteryScanReady
        {
            get => isBatteryScanReady;
            set
            {
                if (!isBatteryScanReady && value)
                {
                    isAlreadyBatteryScan = false;
                }
                SetProperty(ref isBatteryScanReady, value);
            }
        }

        public bool isAlreadyBatteryScan { get; set; }

        private bool isBindTrayScanReady;
        /// <summary>
        /// 绑盘托盘扫码准备就绪
        /// </summary>
        public bool IsBindTrayScanReady
        {
            get => isBindTrayScanReady;
            set
            {
                if (!isBindTrayScanReady && value)
                {
                    isAlreadyBindTrayScan = false;
                }
                SetProperty(ref isBindTrayScanReady, value);
            }
        }

        public bool isAlreadyBindTrayScan { get; set; }

        private bool isUnbindTrayScanReady;
        /// <summary>
        /// 解盘托盘扫码准备就绪
        /// </summary>
        public bool IsUnbindTrayScanReady
        {
            get => isUnbindTrayScanReady;
            set
            {
                if (!isUnbindTrayScanReady && value)
                {
                    isAlreadyUnbindTrayScan = false;
                }
                SetProperty(ref isUnbindTrayScanReady, value);
            }

        }

        public bool isAlreadyUnbindTrayScan { get; set; }

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
            var ret = this.Commor.Comm("GET_PLC_INFO");
            if (ret.IsOk)
            {
                this.RealtimeStatus = "通信中...";

                var retData = ret.Data.ToString().Split('-');
                this.IsBatteryScanReady = retData[0] == "1";
                this.IsBindTrayScanReady = retData[1] == "1";
                this.IsUnbindTrayScanReady = retData[2] == "1";
                this.JawPos = Convert.ToInt32(retData[3]);

                var t = new Thread(() =>
                {
                    //界面交替显示扫码状态
                    Thread.Sleep(this.CommInterval / 2);
                    //this.RealtimeStatus = "等待扫码...";
                });
                t.Start();
                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }
        }
    }
}
