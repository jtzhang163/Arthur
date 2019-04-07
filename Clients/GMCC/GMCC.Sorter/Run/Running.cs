using Arthur;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Run
{
    public class Running
    {
        public static Result Start()
        {
            var commors = Factory.CommorHelper.GetCommors();
            for (var i = 0; i < commors.Count; i++)
            {
                if (commors[i].IsEnabled)
                {
                    var result = commors[i].Commor.Connect();
                    if (!result.IsOk)
                    {
                        Current.App.RunStatus = RunStatus.异常;
                        return result;
                    }
                    commors[i].IsAlive = true;
                    commors[i].RealtimeStatus = "连接成功！";
                }
            }

            Current.App.RunStatus = RunStatus.运行;
            TimerExec.IsRunning = true;
            return Result.OK;
        }

        public static Result Stop()
        {
            var commors = Factory.CommorHelper.GetCommors();
            for (var i = 0; i < commors.Count; i++)
            {
                if (commors[i].IsEnabled)
                {
                    commors[i].RealtimeStatus = "运行停止！";
                }
            }
            TimerExec.IsRunning = false;
            Current.App.RunStatus = RunStatus.停止;
            return Result.OK;
        }

        public static Result Reset()
        {
            var commors = Factory.CommorHelper.GetCommors();
            for (var i = 0; i < commors.Count; i++)
            {
                if (commors[i].IsEnabled)
                {
                    commors[i].Commor.EndConnect();
                    commors[i].RealtimeStatus = "尚未连接！";
                    commors[i].IsAlive = false;
                }
            }
            TimerExec.IsRunning = false;
            Current.App.RunStatus = RunStatus.闲置;
            Current.App.ErrorMsg = "";
            return Result.OK;
        }

        public static void StopRunAndShowMsg(string msg)
        {
            Current.App.ErrorMsg = msg;
            Current.App.RunStatus = RunStatus.异常;
            TimerExec.IsRunning = false;
        }
    }
}
