using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GMCC.Sorter.Run
{
    public class TimerExec
    {
        public static bool IsRunning { get; set; }

        public static void CheckMainMachineInfo(object sender, ElapsedEventArgs e)
        {
            if (IsRunning && Current.MainMachine.IsEnabled)
            {
                Current.MainMachine.Comm();
            }
        }
    }
}
