using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using Arthur.Utility;
using GMCC.Sorter.Data;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
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

        public void SendCommand(JawMoveInfo toMoveInfo)
        {

           // this.Commor.Comm("SET_Move_Info");

        }

        public JawMoveInfo JawMoveInfo = new JawMoveInfo();

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
        /// 静置时长(min)
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
                        stillTimeSpan = 120;
                        Arthur.Business.Application.SetOption("StillTimeSpan", stillTimeSpan.ToString(), "静置时长(min)");
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

                    //同步设置所有料仓静置时间
                    Current.Storages.ForEach(o => o.StillTimeSpan = value);
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


        private int bindProcTrayId = -2;
        /// <summary>
        /// 绑盘位流程托盘Id
        /// </summary>
        public int BindProcTrayId
        {
            get
            {
                if (bindProcTrayId == -2)
                {
                    bindProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("BindProcTrayId"), -1);
                    if (bindProcTrayId == -1)
                    {
                        bindProcTrayId = 0;
                        Arthur.Business.Application.SetOption("BindProcTrayId", bindProcTrayId.ToString(), "绑盘位流程托盘Id");
                    }
                }
                return bindProcTrayId;
            }
            set
            {
                if (bindProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("BindProcTrayId", value.ToString());
                    SetProperty(ref bindProcTrayId, value);
                }
            }
        }


        private int chargeProcTrayId = -2;
        /// <summary>
        /// 充电位流程托盘Id
        /// </summary>
        public int ChargeProcTrayId
        {
            get
            {
                if (chargeProcTrayId == -2)
                {
                    chargeProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("ChargeProcTrayId"), -1);
                    if (chargeProcTrayId == -1)
                    {
                        chargeProcTrayId = 0;
                        Arthur.Business.Application.SetOption("ChargeProcTrayId", bindProcTrayId.ToString(), "充电位流程托盘Id");
                    }
                }
                return chargeProcTrayId;
            }
            set
            {
                if (chargeProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("ChargeProcTrayId", value.ToString());
                    SetProperty(ref chargeProcTrayId, value);
                }
            }
        }


        private int dischargeProcTrayId = -2;
        /// <summary>
        /// 放电位流程托盘Id
        /// </summary>
        public int DischargeProcTrayId
        {
            get
            {
                if (dischargeProcTrayId == -2)
                {
                    dischargeProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("DischargeProcTrayId"), -1);
                    if (dischargeProcTrayId == -1)
                    {
                        dischargeProcTrayId = 0;
                        Arthur.Business.Application.SetOption("DischargeProcTrayId", dischargeProcTrayId.ToString(), "放电位流程托盘Id");
                    }
                }
                return dischargeProcTrayId;
            }
            set
            {
                if (dischargeProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("DischargeProcTrayId", value.ToString());
                    SetProperty(ref dischargeProcTrayId, value);
                }
            }
        }



        private int unbindProcTrayId = -2;
        /// <summary>
        /// 解盘位流程托盘Id
        /// </summary>
        public int UnbindProcTrayId
        {
            get
            {
                if (unbindProcTrayId == -2)
                {
                    unbindProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("UnbindProcTrayId"), -1);
                    if (unbindProcTrayId == -1)
                    {
                        unbindProcTrayId = 0;
                        Arthur.Business.Application.SetOption("UnbindProcTrayId", unbindProcTrayId.ToString(), "解盘位流程托盘Id");
                    }
                }
                return unbindProcTrayId;
            }
            set
            {
                if (unbindProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("UnbindProcTrayId", value.ToString());
                    SetProperty(ref unbindProcTrayId, value);
                }
            }
        }


        private int jawProcTrayId = -2;
        /// <summary>
        /// 横移流程托盘Id
        /// </summary>
        public int JawProcTrayId
        {
            get
            {
                if (jawProcTrayId == -2)
                {
                    jawProcTrayId = Arthur.Utility._Convert.StrToInt(Arthur.Business.Application.GetOption("JawProcTrayId"), -1);
                    if (jawProcTrayId == -1)
                    {
                        jawProcTrayId = 0;
                        Arthur.Business.Application.SetOption("JawProcTrayId", jawProcTrayId.ToString(), "横移流程托盘Id");
                    }
                }
                return jawProcTrayId;
            }
            set
            {
                if (jawProcTrayId != value)
                {
                    Arthur.Business.Application.SetOption("JawProcTrayId", value.ToString());
                    SetProperty(ref jawProcTrayId, value);
                }
            }
        }


        private int bindBatteriesCount = -2;
        /// <summary>
        /// 当前绑盘位托盘里面电池个数
        /// </summary>
        public int BindBatteriesCount
        {
            get
            {
                if (bindBatteriesCount == -2)
                {
                    bindBatteriesCount = Current.MainMachine.GetBindProcTray().GetBatteries().Count;
                }
                return bindBatteriesCount;
            }
            set
            {
                if (bindBatteriesCount != value)
                {
                    SetProperty(ref bindBatteriesCount, value);
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
                    IsAlreadyBatteryScan = false;
                }
                SetProperty(ref isBatteryScanReady, value);
            }
        }

        public bool IsAlreadyBatteryScan { get; set; }

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
                    IsAlreadyBindTrayScan = false;
                }
                SetProperty(ref isBindTrayScanReady, value);
            }
        }

        public bool IsAlreadyBindTrayScan { get; set; }

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




        private bool isChargeGetReady;
        /// <summary>
        /// 充电模组可取托盘
        /// </summary>
        public bool IsChargeGetReady
        {
            get => isChargeGetReady;
            set
            {
                SetProperty(ref isChargeGetReady, value);
            }
        }


        private bool isDischargePutReady;
        /// <summary>
        /// 放电模组可放托盘
        /// </summary>
        public bool IsDischargePutReady
        {
            get => isDischargePutReady;
            set
            {
                SetProperty(ref isDischargePutReady, value);
            }
        }


        private bool isHasChargeTray;
        /// <summary>
        /// 充电位有托盘
        /// </summary>
        public bool IsHasChargeTray
        {
            get => isHasChargeTray;
            set
            {
                if (!isHasChargeTray && value && this.BindBatteriesCount == Common.TRAY_BATTERY_COUNT)
                {
                    this.ChargeProcTrayId = this.BindProcTrayId;
                    this.BindProcTrayId = 0; 
                }
                SetProperty(ref isHasChargeTray, value);
            }
        }

        private bool isHasDisChargeTray;
        /// <summary>
        /// 放电位有托盘
        /// </summary>
        public bool IsHasDisChargeTray
        {
            get => isHasDisChargeTray;
            set
            {
                SetProperty(ref isHasDisChargeTray, value);
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

        private System.Threading.Timer TaskTimer;// = new System.Threading.Timer(new TimerCallback(TaskExec), null, 5000, Current.MainMachine.TaskExecInterval);
        private System.Threading.Timer GetShareDataTimer;

        public MainMachineViewModel(Commor commor) : base(commor)
        {
            TaskTimer = new System.Threading.Timer(new TimerCallback(TimerExec.TaskExec), null, 5000, Current.Option.TaskExecInterval);
            GetShareDataTimer = new System.Threading.Timer(new TimerCallback(TimerExec.GetShareDataExec), null, 5000, Current.Option.GetShareDataExecInterval);
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

                this.IsFeedingFinished = retData[4] == "1";
                this.IsBlankingFinished = retData[5] == "1";

                this.IsHasChargeTray = retData[6] == "1";
                this.IsHasDisChargeTray = retData[7] == "1";

                this.JawMoveInfo.Row = Convert.ToInt32(retData[9]);
                this.JawMoveInfo.Col = Convert.ToInt32(retData[10]);
                this.JawMoveInfo.Floor = Convert.ToInt32(retData[11]);

                this.IsDischargePutReady = retData[12] == "1";

                var t = new Thread(() =>
                {
                    //界面交替显示扫码状态
                    Thread.Sleep(this.CommInterval / 2);
                    //this.RealtimeStatus = "等待扫码...";
                });
                t.Start();

                Current.App.IsTerminalInitFinished = true;
                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }


            //发送横移运动指令
            if (Current.Task.Status == Model.TaskStatus.就绪)
            {

            }


        }
    }
}
