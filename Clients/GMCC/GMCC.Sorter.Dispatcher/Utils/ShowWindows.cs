using GMCC.Sorter.Dispatcher.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GMCC.Sorter.Dispatcher.Utils
{
    public class ShowWindows
    {
        public static void ShowTrayBatteryWin(int procTrayId)
        {
            if (procTrayId < 1)
                return;

            var win = new Window()
            {
                Height = 300,
                Width = 600,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Title = "托盘绑定电池信息"
            };
            win.Content = new TrayBatteryControl(procTrayId);
            win.ShowDialog();
        }
    }
}
