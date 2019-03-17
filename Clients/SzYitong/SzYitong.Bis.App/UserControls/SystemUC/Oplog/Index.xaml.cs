using Arthur.App;
using Arthur.App.Data;
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

namespace SzYitong.Bis.App.UserControls.SystemUC.Oplog
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
            this.end_time.Value = DateTime.Now;

            this.op_type.Items.Add("全部");
            var list = EnumberHelper.EnumToList<Arthur.App.Model.OpType>();
            foreach (var obj in list)
            {
                this.op_type.Items.Add(obj.EnumName);
            }
            this.op_type.SelectedIndex = 0;
        }

        private int PageIndex = 1;

        private List<Arthur.App.Model.Oplog> Oplogs
        {
            get
            {
                var logs = new List<Arthur.App.Model.Oplog>();
                var startTime = this.start_time.Value;
                var endTime = this.end_time.Value;
                if (this.op_type.SelectedIndex > 0)
                {
                    var type = (Arthur.App.Model.OpType)Enum.Parse(typeof(Arthur.App.Model.OpType), this.op_type.SelectedItem.ToString());
                    logs = Context.Oplogs.Where(r => r.Time > startTime && r.Time < endTime && r.OpType == type).ToList();
                }
                else
                {
                    logs = Context.Oplogs.Where(r => r.Time > startTime && r.Time < endTime).ToList();
                }
                return logs;
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<Arthur.App.Model.Oplog>.Create(Oplogs, PageIndex, Current.Option.DataGridPageSize);

            this.count.Content = Oplogs.Count();
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
            var Oplog = Context.Oplogs.SingleOrDefault(r => r.Id == id);

            if (Oplog == null)
            {
                MessageBox.Show("不存在该记录，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level < Context.Roles.FirstOrDefault(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 


            if (MessageBox.Show(string.Format("确定要删除该条日志吗？", ""), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Context.Oplogs.Remove(Oplog);
                Context.LoggingContext.SaveChanges();
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
