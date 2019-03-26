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
                    Context.AppContext.SaveChanges();
                    SetProperty(ref portName, value);

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
                    scanCommand = Arthur.Business.Application.GetOption("ScanCommand_TrayScaner_" + this.Id);
                    if (scanCommand == null)
                    {
                        scanCommand = "T";
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

                    SetProperty(ref scanCommand, value);
                }
            }
        }

        public TrayScanerViewModel(Commor commor) : base(commor)
        {

        }

        public void Comm()
        {
            //if (Current.MainMachine.IsAlive && Current.MainMachine.IsBatteryScanReady)
            //{
            var ret = this.Commor.Comm(this.ScanCommand);
            if (ret.IsOk)
            {
                this.RealtimeStatus = "+" + ret.Data;
                var t = new Thread(() =>
                {
                    var saveRet = Result.OK;

                    if(this == Current.BindTrayScaner)
                    {
                        //把托盘条码保存进数据库
                        saveRet = new Business.ProcTrayManage().Create(new Model.ProcTray() { Code = ret.Data.ToString() }, true);
                    }

                    if (!saveRet.IsOk)
                    {
                        Current.App.ErrorMsg = saveRet.Msg;
                        Current.App.RunStatus = RunStatus.异常;
                        TimerExec.IsRunning = false;
                    }

                    //界面交替显示扫码状态
                    Thread.Sleep(this.CommInterval / 2);
                    this.RealtimeStatus = "等待扫码...";
                });
                t.Start();
                this.IsAlive = true;
            }
            else
            {
                this.RealtimeStatus = ret.Msg;
                this.IsAlive = false;
            }
            //}
        }
    }
}
