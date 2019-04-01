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
using GMCC.Sorter.Extensions;

namespace GMCC.Sorter.Dispatcher.Controls.Machine
{
    /// <summary>
    /// JawControl.xaml 的交互逻辑
    /// </summary>
    public partial class BindMachControl : UserControl
    {
        public BindMachControl()
        {
            InitializeComponent();
            this.DataContext = Current.MainMachine;
            this.Timer = new System.Threading.Timer(new TimerCallback(this.SetBackground), null, 2000, 1000);
        }

        private System.Threading.Timer Timer = null;

        private void SetBackground(object obj)
        {
            this.grid.Dispatcher.BeginInvoke(new Action(() =>{

                var battteryCount = Current.MainMachine.GetBindProcTray().GetBatteries().Count;

                this.grid.Children.Clear();

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        var border = new Border()
                        {
                            Background = i * 4 + j < battteryCount ? Brushes.LightGreen : Brushes.White,
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
    }
}
