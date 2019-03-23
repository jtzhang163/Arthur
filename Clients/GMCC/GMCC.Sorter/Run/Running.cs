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
                }
            }

            //暂时放置在此处
            Current.App.IsTerminalInitFinished = true;
            Current.App.RunStatus = RunStatus.运行;
            TimerExec.IsRunning = true;
            return Result.OK;
        }

        public static Result Stop()
        {
            TimerExec.IsRunning = false;
            Current.App.RunStatus = RunStatus.停止;
            return Result.OK;
        }

        public static Result Reset()
        {
            TimerExec.IsRunning = false;
            Current.App.RunStatus = RunStatus.闲置;
            return Result.OK;
        }
    }


}
