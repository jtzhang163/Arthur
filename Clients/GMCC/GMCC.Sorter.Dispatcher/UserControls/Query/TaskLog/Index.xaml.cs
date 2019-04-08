using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Query.TaskLog
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        public Index(int id)
        {
            InitializeComponent();

            this.start_time.Value = DateTime.Now.AddDays(-1);
            this.end_time.Value = DateTime.Now.AddDays(1);
        }

        private int PageIndex = 1;

        private List<GMCC.Sorter.Model.TaskLog> TaskLogs
        {
            get
            {
                var startTime = this.start_time.Value;
                var endTime = this.end_time.Value;
                return Context.TaskLogs.Where(o => o.Time > startTime && o.Time < endTime).ToList();
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<GMCC.Sorter.Model.TaskLog>.Create(TaskLogs, PageIndex, Current.Option.DataGridPageSize);

            this.count.Content = TaskLogs.Count();
            this.pageIndex.Content = PageIndex;
            this.totalPages.Content = dtos.TotalPages;
            this.size.Content = Current.Option.DataGridPageSize;
            this.tbPageIndex.Text = PageIndex.ToString();
            this.preview_page.IsEnabled = dtos.HasPreviousPage;
            this.next_page.IsEnabled = dtos.HasNextPage;

            this.dataGrid.ItemsSource = dtos;
        }


        private void query_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            var taskLog = Context.TaskLogs.SingleOrDefault(r => r.Id == id);

            if (taskLog == null)
            {
                MessageBox.Show("不存在该任务，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level < Arthur.App.Data.Context.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 


            if (MessageBox.Show(string.Format("确定要删除该任务日志吗？", ""), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Context.TaskLogs.Remove(taskLog);
                Context.AppContext.SaveChanges();
                Arthur.Business.Logging.AddOplog(string.Format("删除任务日志[ID:{0}]", taskLog.Id), Arthur.App.Model.OpType.删除);
                UpdateDataGrid(PageIndex);
            }
        }

        private void go_page_Click(object sender, RoutedEventArgs e)
        {

            if (int.TryParse(this.tbPageIndex.Text.Trim(), out int index))
            {
                if (index > 0 && index <= Convert.ToUInt32(this.totalPages.Content))
                {
                    PageIndex = index;
                }
            }
            UpdateDataGrid(PageIndex);
        }

        private void preview_page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PageIndex--;
            UpdateDataGrid(PageIndex);
        }

        private void next_page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PageIndex++;
            UpdateDataGrid(PageIndex);
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

    }
}
