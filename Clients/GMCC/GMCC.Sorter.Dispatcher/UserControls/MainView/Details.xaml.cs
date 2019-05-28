using Arthur.Core;
using Arthur.App.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Dispatcher.Controls;
using GMCC.Sorter.Dispatcher.Controls.Machine;
using GMCC.Sorter.Utils;
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

namespace GMCC.Sorter.Dispatcher.UserControls.MainView
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {
        public Details(int id)
        {
            InitializeComponent();

            Current.Storages.ForEach(o =>
            {
                var storageUC = new Controls.Machine.StorageControl(o.Id);
                this.grid.Children.Add(storageUC);
                SetRowCol(storageUC);
            });

            this.scaner_signal.Children.Add(new ScanerSignalControl());

            this.jaw.Children.Add(new JawControl());

            this.bind_mach.Children.Add(new BindMachControl());

            this.unbind_mach.Children.Add(new UnbindMachControl());

            this.current_task.Children.Add(new CurrentTaskControl());

            this.sort_pack.Children.Add(new SortPackControl());

            this.Timer = new System.Threading.Timer(new TimerCallback(this.CheckStorageStatus), null, 2000, 1000);
        }

        private void SetRowCol(Controls.Machine.StorageControl storageControl)
        {

            var row = 0;
            var col = 0;

            if (storageControl.Col < Common.STOR_COL_COUNT / 2 + 1)
            {
                //row = 6 - storageControl.Floor;
                row = storageControl.Floor;
                col = 1 + storageControl.Col;
            }
            else
            {
                //row = 13 - storageControl.Floor;
                row = 7 + storageControl.Floor;
                col = storageControl.Col - 8;
            }

            Grid.SetRow(storageControl, row);
            Grid.SetColumn(storageControl, col);
        }

        private System.Threading.Timer Timer = null;

        private void CheckStorageStatus(object obj)
        {
            Current.Storages.ForEach(o =>
            {
                if (o.ProcTrayId > 0)
                {
                    if ((DateTime.Now - o.ProcTray.StartStillTime).TotalMinutes < o.StillTimeSpan)
                    {
                        o.Status = ViewModel.StorageStatus.正在静置;
                        var timespan = DateTime.Now - o.ProcTray.StartStillTime;
                        o.ShowInfo = string.Format("{0}∶{1:D2}", (int)timespan.TotalMinutes, timespan.Seconds);
                    }
                    else
                    {
                        o.Status = ViewModel.StorageStatus.静置完成;
                        var timespan = DateTime.Now - o.ProcTray.StartStillTime.AddMinutes(o.StillTimeSpan);
                        o.ShowInfo = string.Format("{0}∶{1:D2}", (int)timespan.TotalMinutes, timespan.Seconds);
                    }
                }
                else
                {
                    o.Status = ViewModel.StorageStatus.无托盘;
                    o.ShowInfo = o.Name;
                }

                if (o.Id == Current.Task.StorageId && Current.Task.Status != Model.TaskStatus.完成 && Current.Task.Status != Model.TaskStatus.未知)
                {
                    o.TaskType = Current.Task.Type;
                }
                else
                {
                    o.TaskType = Model.TaskType.未知;
                }

                o.TimeNow = DateTime.Now;

            });
        }
    }
}
