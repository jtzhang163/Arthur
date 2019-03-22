using Arthur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Run
{
    public class Running
    {
        public Result Start()
        {
            if (Current.MainMachine.Commor.Connect().IsOk)
            {

            }

            TimerExec.IsRunning = true;
            return Result.OK;
        }

        public Result Pause()
        {

            TimerExec.IsRunning = false;
            return Result.OK;
        }

        public Result Stop()
        {
            TimerExec.IsRunning = false;
            return Result.OK;
        }

        public Result Reset()
        {
            TimerExec.IsRunning = false;
            return Result.OK;
        }
    }


}
