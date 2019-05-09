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

namespace GMCC.Sorter.Dispatcher.UserControls.Query.Battery
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        private readonly Arthur.App.AppContext _ArthurContext = new Arthur.App.AppContext();
        private readonly Data.AppContext _AppContext = new Data.AppContext();
        public Index(int id)
        {
            InitializeComponent();

            this.start_time.Value = DateTime.Now.AddDays(-1);
            this.end_time.Value = DateTime.Now.AddDays(1);
        }

        private int PageIndex = 1;

        private IQueryable<Model.Battery> Batteries
        {
            get
            {
                var queryText = this.queryText.Text.Trim();
                var startTime = this.start_time.Value;
                var endTime = this.end_time.Value;
                if (string.IsNullOrWhiteSpace(queryText))
                {
                    return _AppContext.Batteries.Where(r => r.ScanTime > startTime && r.ScanTime < endTime).OrderBy(o => o.Id);
                }
                else
                {
                    return _AppContext.Batteries.Where(r => r.ScanTime > startTime && r.ScanTime < endTime && r.Code.Contains(queryText)).OrderBy(o => o.Id);
                }
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<Model.Battery>.Create(Batteries, PageIndex, Arthur.App.Current.Option.DataGridPageSize);

            this.count.Content = Batteries.Count();
            this.pageIndex.Content = PageIndex;
            this.totalPages.Content = dtos.TotalPages;
            this.size.Content = Arthur.App.Current.Option.DataGridPageSize;
            this.tbPageIndex.Text = PageIndex.ToString();
            this.preview_page.IsEnabled = dtos.HasPreviousPage;
            this.next_page.IsEnabled = dtos.HasNextPage;

            this.dataGrid.ItemsSource = ContextToViewModel.Convert(dtos);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Role.Level < _ArthurContext.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Helper.ExecuteParentUserControlMethod(this, "Battery", "SwitchWindow", "Create", 0);
        }


        private void query_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            var battery = _AppContext.Batteries.SingleOrDefault(r => r.Id == id);

            if (battery == null)
            {
                MessageBox.Show("不存在该电池，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level < _ArthurContext.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 


            if (MessageBox.Show(string.Format("确定要删除电池【{0}】吗？", battery.Code), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _AppContext.Batteries.Remove(battery);
                _AppContext.SaveChanges();
                Arthur.Business.Logging.AddOplog(string.Format("删除电池[{0}]", battery.Code), Arthur.App.Model.OpType.删除);
                UpdateDataGrid(PageIndex);
            }
        }

        private void edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Current.User.Role.Level < _ArthurContext.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            Helper.ExecuteParentUserControlMethod(this, "Battery", "SwitchWindow", "Edit", id);
        }

        private void details_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            Helper.ExecuteParentUserControlMethod(this, "Battery", "SwitchWindow", "Details", id);
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
