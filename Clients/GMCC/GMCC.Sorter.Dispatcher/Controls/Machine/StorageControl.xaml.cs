using GMCC.Sorter.Data;
using GMCC.Sorter.Dispatcher.Utils;
using GMCC.Sorter.ViewModel;
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

namespace GMCC.Sorter.Dispatcher.Controls.Machine
{
    /// <summary>
    /// StorageUC.xaml 的交互逻辑
    /// </summary>
    public partial class StorageControl : UserControl
    {
        private StorageViewModel Storage;

        public int Col;
        public int Floor;

        public StorageControl(int id)
        {

            InitializeComponent();
            this.Storage = Current.Storages.FirstOrDefault(o => o.Id == id);
            this.DataContext = this.Storage;
            this.Col = this.Storage.Column;
            this.Floor = this.Storage.Floor;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowWindows.ShowTrayBatteryWin(this.Storage.ProcTrayId);
        }


        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //右击时
            if(e.ChangedButton == MouseButton.Right)
            {
                if (Current.App.TaskMode == TaskMode.自动任务)
                    return;

                ContextMenu cm = this.FindResource("manu_get_put") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            var header = (sender as MenuItem).Header.ToString();
            if (header == "手动【上料】" || header == "手动【下料】")
            {
                if (Current.Task.Status != Model.TaskStatus.完成)
                {
                    MessageBox.Show("当前任务尚未完成！","提示");
                }
                Current.Task.StorageId = this.Storage.Id;
                Current.Task.Type = header == "手动【上料】" ? Model.TaskType.上料 : Model.TaskType.下料;
                Current.Task.StartTime = DateTime.Now;
                Current.Task.ProcTrayId = Current.Task.Type == Model.TaskType.上料 ? Current.Option.ChargeProcTrayId : this.Storage.ProcTrayId;
                Current.Task.Status = Model.TaskStatus.就绪;
                Context.AppContext.SaveChanges();
            }
        }
    }
}
