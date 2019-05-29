﻿using Arthur.Core;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
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
using GMCC.Sorter.Business;

namespace GMCC.Sorter.ViewModel
{
    /// <summary>
    /// 主设备
    /// </summary>
    public sealed class MainMachineViewModel : EthernetCommorViewModel
    {
        private int CurrentPackBatteryPos;
        private bool IsPackEnabled = false;
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
                Current.Option.JawPos = ((int)ret3.Data + 5542000) / 11900;

                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }

            Current.App.IsTerminalInitFinished = true;

            if (IsPackEnabled)
            {
                var ret4 = this.Commor.Read("Dxxx");
                if (ret4.IsOk)
                {
                    var recv = (ushort[])ret4.Data;
                    var currentPackBatteryPos = Convert.ToInt32(recv[0]);

                    if (this.CurrentPackBatteryPos > 0 && currentPackBatteryPos == 0)
                    {
                        //电池放入拉带完成
                        AfterPack(this.CurrentPackBatteryPos);
                    }

                    if (currentPackBatteryPos > 0)
                    {
                        this.Commor.Write("Dxxx", (ushort)0);
                    }

                    this.CurrentPackBatteryPos = currentPackBatteryPos;
                }
            }
        }

        public void AfterPack(int pos)
        {
            var procTrayId = Current.Option.Tray23_Id > 0 ? Current.Option.Tray23_Id : Current.Option.Tray23_PreId;
            var result = BatteryManage.GetBattery(procTrayId, pos);
            if (result.IsFailed)
            {
                return;
            }

            var battery = (Battery)result.Data;
            if (battery.SortResult == SortResult.Unknown || (int)battery.SortResult > 5)
            {
                return;
            }

            var sortResult = battery.SortResult;

            var sortPack = Current.SortPacks.FirstOrDefault(o => o.SortResult == sortResult);


            result = BatteryManage.GetFillBatteryCount(sortPack.PackId);
            if (result.IsFailed)
            {
                return;
            }

            var fillCount = (int)result.Data;

            //新建箱体
            if (sortPack.PackId == 0 || fillCount == Current.Option.PACK_FILL_COUNT)
            {
                if (fillCount == Current.Option.PACK_FILL_COUNT)
                {
                    PackManage.Finish(sortPack);
                }

                var code = ""; //箱体号
                result = new PackManage().Create(new Pack(code, sortResult));
                if (result.IsFailed)
                {
                    return;
                }
                sortPack.PackId = (int)result.Data;
                sortPack.Count = 0;
            }

            result = BatteryManage.SetPacking(battery.Id, sortPack.PackId);
            if (result.IsFailed)
            {
                return;
            }

            sortPack.Count++;

            if (sortPack.Count % Current.Option.PACK_ALARM_COUNT == 0)
            {
                if (IsPackEnabled)
                {
                    this.Commor.Write("Dxxx", (ushort)0);
                }
            }

        }
    }
}
