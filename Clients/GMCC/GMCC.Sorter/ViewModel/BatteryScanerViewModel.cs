using Arthur.Core;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Business;
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
    /// 电池扫码枪
    /// </summary>
    public sealed class BatteryScanerViewModel : EthernetCommorViewModel, IScanerViewModel
    {

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
                    scanCommand = Arthur.App.Business.Setting.GetOption("ScanCommand_BatteryScaner");
                    if (scanCommand == null)
                    {
                        scanCommand = "T";
                        Arthur.App.Business.Setting.SetOption("ScanCommand_BatteryScaner", scanCommand, this.Name + "扫码指令");
                    }
                }
                return scanCommand;
            }
            set
            {
                if (scanCommand != value)
                {
                    Arthur.App.Business.Setting.SetOption("ScanCommand_BatteryScaner", value);
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}扫码指令: [{1}] 修改为 [{2}]", Name, scanCommand, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref scanCommand, value);
                }
            }
        }

        public BatteryScanerViewModel(Commor commor) : base(commor)
        {
            Console.WriteLine(this.ScanCommand);
        }

        public void Comm()
        {
            if (Current.MainMachine.IsAlive && Current.Option.IsBatteryScanReady && !Current.Option.IsAlreadyBatteryScan && Current.Option.Tray11_Id > 0)
            {
                //绑盘位电池已满，不扫码，直到出现新托盘再扫
                if (ProcTrayManage.GetBatteryCount(Current.Option.Tray11_Id) >= Common.TRAY_BATTERY_COUNT)
                {
                    Running.ShowErrorMsg("绑盘位扫码电池数超过最大值：" + Common.TRAY_BATTERY_COUNT);
                    return;
                }
                var ret = this.Commor.Comm(this.ScanCommand);
                if (ret.IsOk)
                {
                    var result = true;
                    var code = ret.Data.ToString();
                    if (code.StartsWith("NG"))
                    {
                        var ret2 = this.Commor.Comm(this.ScanCommand);
                        if (ret2.IsOk && !ret2.Data.ToString().StartsWith("NG"))
                        {
                            code = ret2.Data.ToString();
                        }
                        else
                        {
                            result = false;
                            LogHelper.WriteError(this.Name + " 扫码失败！");
                            Running.ShowErrorMsg(this.Name + " 扫码失败！");
                        }
                    }

                    if (result)
                    {
                        this.RealtimeStatus = "+" + code;
                        Current.MainMachine.Commor.Write("D433", (ushort)1);

                        //把电池条码保存进数据库
                        var saveRet = new Business.BatteryManage().Create(new Model.Battery() { Code = code }, true);
                        if (saveRet.IsOk)
                        {
                            var t = new Thread(() =>
                            {
                                //界面交替显示扫码状态
                                Thread.Sleep(2000);
                                this.RealtimeStatus = "等待扫码...";
                            });
                            t.Start();
                        }
                        else
                        {
                            Running.StopRunAndShowMsg(saveRet.Msg);
                        }

                    }
                    else
                    {
                        Current.MainMachine.Commor.Write("D433", (ushort)2);
                        this.RealtimeStatus = "扫码失败！";
                    }

                    Current.Option.IsAlreadyBatteryScan = true;
                    this.IsAlive = true;
                }
                else
                {
                    Current.MainMachine.Commor.Write("D433", (ushort)2);
                    this.RealtimeStatus = ret.Msg;
                    this.IsAlive = false;
                }
            }
        }
    }
}
