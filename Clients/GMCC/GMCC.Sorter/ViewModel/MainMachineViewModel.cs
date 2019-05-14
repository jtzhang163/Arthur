using Arthur;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using Arthur.Utils;
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
    public sealed class MainMachineViewModel : CommorViewModel
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
            this.Commor.Write("D450", (ushort)toMoveInfo.Col);
            this.Commor.Write("D451", (ushort)toMoveInfo.Row);
            this.Commor.Write("D452", (ushort)toMoveInfo.Floor);

            var d400 = toMoveInfo.Type == TaskType.上料 ? 1 : 2;

            this.Commor.Write("D400", (ushort)d400);
            LogHelper.WriteInfo(string.Format("------ 给PLC发送{4}信号 D400：{3}，D450：{0}，D451：{1}，D452：{2}-----", toMoveInfo.Col, toMoveInfo.Row, toMoveInfo.Floor, d400, toMoveInfo.Type));
        }


        private System.Threading.Timer TaskTimer;
        private System.Threading.Timer GetShareDataTimer;

        public MainMachineViewModel(Commor commor) : base(commor)
        {
            TaskTimer = new System.Threading.Timer(new TimerCallback(TimerExec.TaskExec), null, 5000, Current.Option.TaskExecInterval);
            GetShareDataTimer = new System.Threading.Timer(new TimerCallback(TimerExec.GetShareDataExec), null, 5000, Current.Option.GetShareDataExecInterval);
        }

        public void Comm()
        {
            //发送给PLC上位机在线心跳
            Current.MainMachine.Commor.Write("D441", (ushort)1);

            var ret = this.Commor.Read("D400", (ushort)60);
            if (ret.IsOk)
            {
                this.RealtimeStatus = "通信中...";

                var recv = (ushort[])ret.Data;

                Current.Option.JawMoveInfo.Col = Convert.ToInt32(recv[50]);
                Current.Option.JawMoveInfo.Row = Convert.ToInt32(recv[51]);
                Current.Option.JawMoveInfo.Floor = Convert.ToInt32(recv[52]);

                Current.Option.IsTaskFinished = recv[36] == (ushort)1;

                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }

            var ret2 = this.Commor.Read("W400", (ushort)10);
            if (ret2.IsOk)
            {
                this.RealtimeStatus = "通信中...";

                var recv = (ushort[])ret2.Data;
                var bitStr1 = Convert.ToString(recv[0], 2).PadLeft(16, '0');

                Current.Option.IsBatteryScanReady = bitStr1[13] == '1';
                Current.Option.IsBindTrayScanReady = bitStr1[12] == '1';
                Current.Option.IsUnbindTrayScanReady = bitStr1[11] == '1';


                var bitStr2 = Convert.ToString(recv[5], 2).PadLeft(16, '0');

                Current.Option.IsHasTray11 = bitStr2[15] == '1';
                Current.Option.IsHasTray12 = bitStr2[14] == '1';
                Current.Option.IsHasTray13 = bitStr2[13] == '1';
                Current.Option.IsHasTray21 = bitStr2[12] == '1';
                Current.Option.IsHasTray22 = bitStr2[11] == '1';
                Current.Option.IsHasTray23 = bitStr2[10] == '1';

                Current.Option.IsJawHasTray = bitStr2[9] == '1';

                Current.Option.IsTaskReady = bitStr2[8] == '1';

                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }

            var ret3 = this.Commor.ReadInt("D439");
            if (ret3.IsOk)
            {
                this.RealtimeStatus = "通信中...";

                // 最小值 -5540111 最大值 805192
                Current.Option.JawPos = ((int)ret3.Data + 5542000)/ 11900;

                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }

            Current.App.IsTerminalInitFinished = true;

            ////发送横移运动指令
            //if (Current.Task.Status == Model.TaskStatus.就绪)
            //{

            //}


        }
    }
}
