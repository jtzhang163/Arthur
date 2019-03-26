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
                    SetProperty(ref ip, value);

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

                    SetProperty(ref scanCommand, value);
                }
            }
        }

        public BatteryScanerViewModel(Commor commor) : base(commor)
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
                        //把电池条码保存进数据库
                        var saveRet = new Business.BatteryManage().Create(new Model.Battery() { Code = ret.Data.ToString() }, true);
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
