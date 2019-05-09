using Arthur.App;
using Arthur.App.Data;
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

namespace Arthur.View.Account.Role
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
        }

        private int PageIndex = 1;

        private IQueryable<Arthur.App.Model.Role> Roles
        {
            get
            {
                var queryText = this.queryText.Text.Trim();
                if (string.IsNullOrWhiteSpace(queryText))
                {
                    return _AppContext.Roles.OrderBy(o => o.Id);
                }
                else
                {
                    return _AppContext.Roles.Where(r => r.Name.Contains(queryText)).OrderBy(o => o.Id);
                }
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<Arthur.App.Model.Role>.Create(Roles, PageIndex, Current.Option.DataGridPageSize);

            this.count.Content = Roles.Count();
            this.pageIndex.Content = PageIndex;
            this.totalPages.Content = dtos.TotalPages;
            this.size.Content = Current.Option.DataGridPageSize;
            this.tbPageIndex.Text = PageIndex.ToString();
            this.preview_page.IsEnabled = dtos.HasPreviousPage;
            this.next_page.IsEnabled = dtos.HasNextPage;

            this.dataGrid.ItemsSource = dtos;
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Role.Level < _AppContext.Roles.Single(r => r.Name == "系统管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Create", 0);
        }


        private void query_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void delete_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            var role = _AppContext.Roles.SingleOrDefault(r => r.Id == id);

            if (role == null)
            {
                MessageBox.Show("不存在该角色，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level < _AppContext.Roles.Single(r => r.Name == "系统管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            } 

            if (role == Current.User.Role)
            {
                MessageBox.Show("不能删除当前用户所在角色！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var preTip = string.Empty;
            var userCount = role.Users.Count;
            if (userCount > 0)
            {
                preTip = string.Format("该角色关联着{0}个用户，删除角色时这些用户也会被删除！\r\n", userCount);
            }

            if (MessageBox.Show(preTip + string.Format("确定要删除角色【{0}】吗？", role.Name), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _AppContext.Roles.Remove(role);
                _AppContext.SaveChanges();
                Arthur.Business.Logging.AddOplog(string.Format("删除角色[{0}]", role.Name), App.Model.OpType.删除);
                UpdateDataGrid(PageIndex);
            }
        }

        private void edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Current.User.Role.Level < _AppContext.Roles.Single(r => r.Name == "系统管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Edit", id);
        }

        private void details_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Details", id);
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
