using Arthur.Core;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    /// <summary>
    /// 主设备
    /// </summary>
    public sealed class TrayScanerViewModel : SerialCommorViewModel, IScanerViewModel
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
                    scanCommand = Arthur.App.Business.Setting.GetOption("ScanCommand_TrayScaner_" + this.Id);
                    if (scanCommand == null)
                    {
                        scanCommand = "16 54 0D";
                        Arthur.App.Business.Setting.SetOption("ScanCommand_TrayScaner_" + this.Id, scanCommand, this.Name + "扫码指令");
                    }
                }
                return scanCommand;
            }
            set
            {
                if (scanCommand != value)
                {
                    Arthur.App.Business.Setting.SetOption("ScanCommand_TrayScaner_" + this.Id, value);
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}扫码指令: [{1}] 修改为 [{2}]", Name, scanCommand, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref scanCommand, value);
                }
            }
        }

        public TrayScanerViewModel(Commor commor) : base(commor)
        {
            Console.WriteLine(this.ScanCommand);
        }

        public void Comm()
        {
            if (!Current.MainMachine.IsAlive) return;

            if (this == Current.BindTrayScaner && Current.Option.IsBindTrayScanReady && !Current.Option.IsAlreadyBindTrayScan)
            {
                LogHelper.WriteInfo("开始绑盘托盘扫码。。。");
                var ret = this.Commor.Comm(this.ScanCommand);
                if (ret.IsSucceed)
                {
                    var result = true;
                    var code = ret.Data.ToString();
                    if (code.StartsWith("NR"))
                    {
                        var ret2 = this.Commor.Comm(this.ScanCommand);
                        if (ret2.IsSucceed && !ret2.Data.ToString().StartsWith("NR"))
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

                        LogHelper.WriteInfo("绑盘托盘扫码OK。。。");

                        this.RealtimeStatus = "+" + code;
                        Current.MainMachine.Commor.Write("D434", (ushort)1);


                        var saveRet = Result.Success;
                        if (Current.Option.Tray11_Id < 1)
                        {
                            //把电池条码保存进数据库
                            saveRet = new Business.ProcTrayManage().Create(new Model.ProcTray() { Code = code }, true);
                            Current.Option.Tray11_Id = (int)saveRet.Data;
                        }

                        if (saveRet.IsSucceed)
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
                        LogHelper.WriteInfo("绑盘托盘扫码NG。。。");

                        Current.MainMachine.Commor.Write("D434", (ushort)2);
                        this.RealtimeStatus = "扫码失败！";
                    }

                    Current.Option.IsAlreadyBindTrayScan = true;
                    this.IsAlive = true;
                }
                else
                {
                    LogHelper.WriteInfo("绑盘托盘扫码NG。。。");

                    Current.MainMachine.Commor.Write("D434", (ushort)2);
                    this.RealtimeStatus = ret.Msg;
                    this.IsAlive = false;
                }
            }
            else if (this == Current.UnbindTrayScaner && Current.Option.IsUnbindTrayScanReady && !Current.Option.isAlreadyUnbindTrayScan)
            {
                LogHelper.WriteInfo("开始解盘托盘扫码。。。");
                var ret = this.Commor.Comm(this.ScanCommand);
                if (ret.IsSucceed)
                {
                    var result = true;
                    var code = ret.Data.ToString();
                    if (code.StartsWith("NR"))
                    {
                        var ret2 = this.Commor.Comm(this.ScanCommand);
                        if (ret2.IsSucceed && !ret2.Data.ToString().StartsWith("NR"))
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
                        LogHelper.WriteInfo("解盘托盘扫码OK。。。");
                        this.RealtimeStatus = "+" + code;
                        Current.MainMachine.Commor.Write("D435", (ushort)1);


                        var saveRet = Result.Success;

                        //逻辑处理
                        var procTrayId = GetObject.GetByCode<ProcTray>(code).Id;
                        if (Current.Option.Tray21_Id < 1 || Current.Option.Tray21_Id != procTrayId)
                        {
                            Current.Option.Tray21_Id = procTrayId;
                        }

                        if (saveRet.IsSucceed)
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
                        LogHelper.WriteInfo("解盘托盘扫码NG。。。");
                        Current.MainMachine.Commor.Write("D435", (ushort)(Common.PROJ_NO == "0079" ? 2 : 1));
                        this.RealtimeStatus = "扫码失败！";
                    }

                    Current.Option.IsAlreadyBindTrayScan = true;
                    this.IsAlive = true;
                }
                else
                {
                    LogHelper.WriteInfo("解盘托盘扫码NG。。。");
                    Current.MainMachine.Commor.Write("D435", (ushort)(Common.PROJ_NO == "0079" ? 2 : 1));
                    this.RealtimeStatus = ret.Msg;
                    this.IsAlive = false;
                }
            }
        }
    }
}
