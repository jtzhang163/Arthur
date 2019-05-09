using Arthur.App;
using Arthur.App.Data;
using Arthur.App.Model;
using Arthur.Utils;
using Arthur.View.Utils;
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

namespace Arthur.View.SystemUC.EventLog
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        private readonly App.AppContext _AppContext = new App.AppContext();
        public Index(int id)
        {
            InitializeComponent();

            this.start_time.Value = DateTime.Now.AddDays(-1);
            this.end_time.Value = DateTime.Now.AddDays(1);

            this.event_type.Items.Add("全部");
            var list = EnumberHelper.EnumToList<EventType>();
            foreach (var obj in list)
            {
                this.event_type.Items.Add(obj.EnumName);
            }
            this.event_type.SelectedIndex = 0;
        }

        private int PageIndex = 1;

        private IQueryable<Arthur.App.Model.EventLog> EventLogs
        {
            get
            {
                var startTime = this.start_time.Value;
                var endTime = this.end_time.Value;
                if (this.event_type.SelectedIndex > 0)
                {
                    var type = (Arthur.App.Model.EventType)Enum.Parse(typeof(Arthur.App.Model.EventType), this.event_type.SelectedItem.ToString());
                    return _AppContext.EventLogs.Where(r => r.Time > startTime && r.Time < endTime && r.EventType == type).OrderBy(o => o.Id);
                }
                else
                {
                    return _AppContext.EventLogs.Where(r => r.Time > startTime && r.Time < endTime).OrderBy(o => o.Id);
                }
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<Arthur.App.Model.EventLog>.Create(EventLogs, PageIndex, Current.Option.DataGridPageSize);

            this.count.Content = EventLogs.Count();
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
            var EventLog = _AppContext.EventLogs.SingleOrDefault(r => r.Id == id);

            if (EventLog == null)
            {
                MessageBox.Show("不存在该记录，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level < _AppContext.Roles.FirstOrDefault(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 


            if (MessageBox.Show(string.Format("确定要删除该记录吗？", ""), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _AppContext.EventLogs.Remove(EventLog);
                _AppContext.SaveChanges();
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
            if (PageIndex <= 1) return;
            PageIndex--;
            UpdateDataGrid(PageIndex);
        }

        private void next_page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (PageIndex >= Convert.ToUInt32(this.totalPages.Content)) return;
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
