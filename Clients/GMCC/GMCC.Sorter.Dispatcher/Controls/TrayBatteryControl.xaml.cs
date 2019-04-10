using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GMCC.Sorter.Dispatcher.Controls
{
    /// <summary>
    /// TrayBattery.xaml 的交互逻辑
    /// </summary>
    public partial class TrayBatteryControl : UserControl
    {
        public TrayBatteryControl(int procTrayId)
        {
            InitializeComponent();

            var procTray = GMCC.Sorter.Utils.GetObject.GetById<ProcTray>(procTrayId);
            this.traycode.Content = procTray.Code;

            var batteries = procTray.GetBatteries();
            this.battery_count.Content = batteries.Count;
            batteries.ForEach(o =>
            {
                var lab = new Label();
                lab.Content = o.Code;
                lab.HorizontalContentAlignment = HorizontalAlignment.Left;
                lab.VerticalContentAlignment = VerticalAlignment.Center;
                this.grid.Children.Add(lab);
                Grid.SetRow(lab, (o.Pos - 1) / 4);
                Grid.SetColumn(lab, 3 - (o.Pos - 1) % 4);
            });
            //  this.
        }
    }
}
