using Arthur;
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
    /// 电池扫码枪
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
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}IP地址: [{1}] 修改为 [{2}]", Name, ip, value), Arthur.App.Model.OpType.编辑);
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
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}端口: [{1}] 修改为 [{2}]", Name, port, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref port, value);
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
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}扫码指令: [{1}] 修改为 [{2}]", Name, scanCommand, value), Arthur.App.Model.OpType.编辑);
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
            //绑盘位电池已满，不扫码，直到出现新托盘再扫
            if (Current.Option.BindBatteriesCount >= Common.TRAY_BATTERY_COUNT)
            {
                return;
            }

            if (Current.MainMachine.IsAlive && Current.Option.IsBatteryScanReady && !Current.Option.IsAlreadyBatteryScan && Current.Option.BindProcTrayId > 0)
            {
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
                        var t = new Thread(() =>
                        {
                            //把电池条码保存进数据库
                            var saveRet = new Business.BatteryManage().Create(new Model.Battery() { Code = code }, true);
                            if (!saveRet.IsOk)
                            {
                                Running.StopRunAndShowMsg(saveRet.Msg);
                                return;
                            }

                            //界面交替显示扫码状态
                            Thread.Sleep(this.CommInterval / 2);
                            this.RealtimeStatus = "等待扫码...";
                            Current.Option.BindBatteriesCount++;
                        });
                        t.Start();
                    }
                    else
                    {
                        this.RealtimeStatus = "扫码失败！";
                    }

                    Current.Option.IsAlreadyBatteryScan = true;
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
}
