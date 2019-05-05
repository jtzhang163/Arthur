using Arthur;
using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Data;
using GMCC.Sorter.Run;
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
    public class TrayScanerViewModel : CommorViewModel
    {
        private string portName = null;
        public string PortName
        {
            get
            {
                portName = ((SerialCommor)this.Commor.Communicator).PortName;
                return portName;
            }
            set
            {
                if (portName != value)
                {
                    ((SerialCommor)this.Commor.Communicator).PortName = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}端口: [{1}] 修改为 [{2}]", Name, portName, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref portName, value);
                    this.CommorInfo = null;
                }
            }
        }

        private int? baudRate = null;
        public int BaudRate
        {
            get
            {
                if (baudRate == null)
                {
                    baudRate = ((SerialCommor)this.Commor.Communicator).BaudRate;
                }
                return baudRate.Value;
            }
            set
            {
                if (baudRate != value)
                {
                    ((SerialCommor)this.Commor.Communicator).BaudRate = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}波特率: [{1}] 修改为 [{2}]", Name, baudRate, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref baudRate, value);
                    this.CommorInfo = null;
                }
            }
        }


        private Parity parity;
        public Parity Parity
        {
            get
            {
                parity = ((SerialCommor)this.Commor.Communicator).Parity;
                return parity;
            }
            set
            {
                if (parity != value)
                {
                    ((SerialCommor)this.Commor.Communicator).Parity = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}校验位: [{1}] 修改为 [{2}]", Name, parity, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref parity, value);
                }
            }
        }


        private int? dataBits = null;
        public int DataBits
        {
            get
            {
                if (dataBits == null)
                {
                    dataBits = ((SerialCommor)this.Commor.Communicator).DataBits;
                }
                return dataBits.Value;
            }
            set
            {
                if (dataBits != value)
                {
                    ((SerialCommor)this.Commor.Communicator).DataBits = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}数据位: [{1}] 修改为 [{2}]", Name, dataBits, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref dataBits, value);
                }
            }
        }


        private StopBits stopBits;
        public StopBits StopBits
        {
            get
            {
                stopBits = ((SerialCommor)this.Commor.Communicator).StopBits;
                return stopBits;
            }
            set
            {
                if (stopBits != value)
                {
                    ((SerialCommor)this.Commor.Communicator).StopBits = value;
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}停止位: [{1}] 修改为 [{2}]", Name, stopBits, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stopBits, value);
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
                    scanCommand = Arthur.Business.Application.GetOption("ScanCommand_TrayScaner_" + this.Id);
                    if (scanCommand == null)
                    {
                        scanCommand = "16 54 0D";
                        Arthur.Business.Application.SetOption("ScanCommand_TrayScaner_" + this.Id, scanCommand, this.Name + "扫码指令");
                    }
                }
                return scanCommand;
            }
            set
            {
                if (scanCommand != value)
                {
                    Arthur.Business.Application.SetOption("ScanCommand_TrayScaner_" + this.Id, value);
                    Arthur.Business.Logging.AddOplog(string.Format("设备管理. {0}扫码指令: [{1}] 修改为 [{2}]", Name, stopBits, value), Arthur.App.Model.OpType.编辑);
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
                var ret = this.Commor.Comm(this.ScanCommand);
                if (ret.IsOk)
                {
                    Current.Option.IsAlreadyBindTrayScan = true;
                    this.RealtimeStatus = "+" + ret.Data;

                    var save_res = Current.MainMachine.Commor.Write("D434", (ushort)1);

                    if (Current.Option.Tray11_Id > 0)
                    {
                        return;
                    }

                    var t = new Thread(() =>
                    {
                        //把托盘条码保存进数据库
                        var saveRet = new Business.ProcTrayManage().Create(new Model.ProcTray() { Code = ret.Data.ToString() }, true);
                        if (saveRet.IsOk)
                        {
                            //if (Current.MainMachine.BindProcTrayId > 0)
                            //{
                            //    Running.StopRunAndShowMsg("绑盘位已有托盘条码！");
                            //    return;
                            //}
                            Current.Option.Tray11_Id = (int)saveRet.Data;
                        }
                        else
                        {
                    
                            Running.StopRunAndShowMsg(saveRet.Msg);
                            return;
                        }

                        //界面交替显示扫码状态
                        Thread.Sleep(2000);
                        this.RealtimeStatus = "等待扫码...";
                    });
                    t.Start();
                    this.IsAlive = true;
                }
                else
                {
                    var save_res = Current.MainMachine.Commor.Write("D434", (ushort)2);

                    this.RealtimeStatus = ret.Msg;
                    this.IsAlive = false;
                }
            }
            else if (this == Current.UnbindTrayScaner && Current.Option.IsUnbindTrayScanReady && !Current.Option.isAlreadyUnbindTrayScan)
            {
                var ret = this.Commor.Comm(this.ScanCommand);
                if (ret.IsOk)
                {
                    var save_res = Current.MainMachine.Commor.Write("D435", (ushort)1);
                    Current.Option.isAlreadyUnbindTrayScan = true;
                    this.RealtimeStatus = "+" + ret.Data;
                    var t = new Thread(() =>
                    {
                        var saveRet = Result.OK;

                        //处理解盘

                        if (!saveRet.IsOk)
                        {
                            Current.App.ErrorMsg = saveRet.Msg;
                            Current.App.RunStatus = RunStatus.异常;
                            TimerExec.IsRunning = false;
                        }

                        //界面交替显示扫码状态
                        Thread.Sleep(2000);
                        this.RealtimeStatus = "等待扫码...";
                    });
                    t.Start();
                    this.IsAlive = true;
                }
                else
                {
                    Current.MainMachine.Commor.Write("D435", (ushort)2);
                    this.RealtimeStatus = ret.Msg;
                    this.IsAlive = false;
                }
            }
            //}
        }
    }
}
