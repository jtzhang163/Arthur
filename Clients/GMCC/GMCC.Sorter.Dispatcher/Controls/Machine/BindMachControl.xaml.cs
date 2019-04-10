using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using GMCC.Sorter.Dispatcher.Utils;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.ViewModel;

namespace GMCC.Sorter.Dispatcher.Controls.Machine
{
    /// <summary>
    /// JawControl.xaml 的交互逻辑
    /// </summary>
    public partial class BindMachControl : UserControl
    {
        private MainMachineViewModel MainMachine;

        public BindMachControl()
        {
            InitializeComponent();
            this.MainMachine = Current.MainMachine;
            this.DataContext = this.MainMachine;
            this.Timer = new System.Threading.Timer(new TimerCallback(this.SetBackground), null, 2000, 1000);
        }

        private System.Threading.Timer Timer = null;

        private void SetBackground(object obj)
        {
            this.grid.Dispatcher.BeginInvoke(new Action(() =>{

                var batteryCount = Current.MainMachine.BindBatteriesCount;

                this.grid.Children.Clear();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        var border = new Border()
                        {
                            Background = i * 4 + 3 - j < batteryCount ? Brushes.LightGreen : Brushes.White,
                            // Margin = new Thickness(0.1),
                            BorderThickness = new Thickness(0.5),
                            BorderBrush = Brushes.Black
                        };
                        this.grid.Children.Add(border);
                        Grid.SetRow(border, i);
                        Grid.SetColumn(border, j);
                    }
                }
            }));

        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var label = sender as Label;
            ShowWindows.ShowTrayBatteryWin(Convert.ToInt32(label.Tag));
        }
    }
}
