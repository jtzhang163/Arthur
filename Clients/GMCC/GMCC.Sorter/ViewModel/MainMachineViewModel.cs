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


        private System.Threading.Timer TaskTimer;// = new System.Threading.Timer(new TimerCallback(TaskExec), null, 5000, Current.MainMachine.TaskExecInterval);
        private System.Threading.Timer GetShareDataTimer;

        public MainMachineViewModel(Commor commor) : base(commor)
        {
            TaskTimer = new System.Threading.Timer(new TimerCallback(TimerExec.TaskExec), null, 5000, Current.Option.TaskExecInterval);
            GetShareDataTimer = new System.Threading.Timer(new TimerCallback(TimerExec.GetShareDataExec), null, 5000, Current.Option.GetShareDataExecInterval);
        }

        public void Comm()
        {
            var ret = this.Commor.Read("D400", (ushort)60);
            if (ret.IsOk)
            {
                this.RealtimeStatus = "通信中...";

                var recv = (ushort[])ret.Data;

                Current.Option.JawMoveInfo.Row = Convert.ToInt32(recv[50]);
                Current.Option.JawMoveInfo.Col = Convert.ToInt32(recv[51]);
                Current.Option.JawMoveInfo.Floor = Convert.ToInt32(recv[52]);

                Current.App.IsTerminalInitFinished = true;
                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }

            var ret2 = this.Commor.Read("W400", (ushort)2);
            if (ret2.IsOk)
            {
                this.RealtimeStatus = "通信中...";

                var recv = (ushort[])ret2.Data;
                var bitStr = Convert.ToString(recv[0], 2).PadLeft(8, '0');

                Current.Option.IsBatteryScanReady = bitStr[5] == '1';
                Current.Option.IsBindTrayScanReady = bitStr[4] == '1';
                Current.Option.IsUnbindTrayScanReady = bitStr[3] == '1';

                Current.Option.JawPos = Convert.ToInt32("11");

                Current.Option.IsFeedingFinished = bitStr[1] == '1';
                Current.Option.IsBlankingFinished = bitStr[0] == '1';

                //Current.Option.IsHasChargeTray = retData[6] == "1";
                //Current.Option.IsHasDisChargeTray = retData[7] == "1";


                //Current.Option.IsDischargePutReady = retData[12] == "1";

                Current.App.IsTerminalInitFinished = true;
                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }


            ////发送横移运动指令
            //if (Current.Task.Status == Model.TaskStatus.就绪)
            //{

            //}


        }
    }
}
