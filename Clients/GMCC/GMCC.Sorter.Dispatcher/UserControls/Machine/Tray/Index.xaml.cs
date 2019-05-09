using Arthur.View.Utils;
using GMCC.Sorter.Data;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Machine.Tray
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
        }

        private int PageIndex = 1;

        private List<Model.Tray> Trays
        {
            get
            {
                var queryText = this.queryText.Text.Trim();
                if (string.IsNullOrWhiteSpace(queryText))
                {
                    return _AppContext.Trays.ToList();
                }
                else
                {
                    return _AppContext.Trays.Where(r => r.Code.Contains(queryText)).ToList();
                }
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<Model.Tray>.Create(Trays, PageIndex, Arthur.App.Current.Option.DataGridPageSize);

            this.count.Content = Trays.Count();
            this.pageIndex.Content = PageIndex;
            this.totalPages.Content = dtos.TotalPages;
            this.size.Content = Arthur.App.Current.Option.DataGridPageSize;
            this.tbPageIndex.Text = PageIndex.ToString();
            this.preview_page.IsEnabled = dtos.HasPreviousPage;
            this.next_page.IsEnabled = dtos.HasNextPage;

            this.dataGrid.ItemsSource = dtos;
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Role.Level < _ArthurContext.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Helper.ExecuteParentUserControlMethod(this, "Tray", "SwitchWindow", "Create", 0);
        }


        private void query_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            var tray = _AppContext.Trays.SingleOrDefault(r => r.Id == id);

            if (tray == null)
            {
                MessageBox.Show("不存在该托盘，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level < _ArthurContext.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 


            if (MessageBox.Show(string.Format("确定要删除托盘【{0}】吗？", tray.Code), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _AppContext.Trays.Remove(tray);
                _AppContext.SaveChanges();
                Arthur.Business.Logging.AddOplog(string.Format("删除托盘[{0}]", tray.Code), Arthur.App.Model.OpType.删除);
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
            Helper.ExecuteParentUserControlMethod(this, "Tray", "SwitchWindow", "Edit", id);
        }

        private void details_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            Helper.ExecuteParentUserControlMethod(this, "Tray", "SwitchWindow", "Details", id);
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
