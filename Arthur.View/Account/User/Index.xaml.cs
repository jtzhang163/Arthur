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

namespace Arthur.View.Account.User
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

        private IQueryable<Arthur.App.Model.User> Users
        {
            get
            {
                var queryText = this.queryText.Text.Trim();
                if (string.IsNullOrWhiteSpace(queryText))
                {
                    return _AppContext.Users.OrderBy(o => o.Id);
                }
                else
                {
                    return _AppContext.Users.Where(r => r.Name.Contains(queryText) 
                    || r.Role.Name.Contains(queryText)
                    || r.Number.Contains(queryText)
                    || r.PhoneNumber.Contains(queryText)
                    || r.Email.Contains(queryText)
                    ).OrderBy(o => o.Id);
                }
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid(PageIndex);
        }

        private void UpdateDataGrid(int index)
        {
            var dtos = PaginatedList<Arthur.App.Model.User>.Create(Users, PageIndex, Current.Option.DataGridPageSize);

            this.count.Content = Users.Count();
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
            var user = _AppContext.Users.SingleOrDefault(r => r.Id == id);

            if (user == null)
            {
                MessageBox.Show("不存在该用户，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (user == Current.User)
            {
                MessageBox.Show("不能删除当前用户！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Role.Level <= _AppContext.Users.Single(r => r.Id == id).Role.Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show(string.Format("确定要删除用户【{0}】吗？", user.Name), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                _AppContext.Users.Remove(user);
                _AppContext.SaveChanges();
                Arthur.Business.Logging.AddOplog(string.Format("删除用户[{0}]", user.Name), App.Model.OpType.删除);
                UpdateDataGrid(PageIndex);
            }
        }

        private void edit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            if (Current.User != _AppContext.Users.Single(r => r.Id == id) && Current.User.Role.Level <= _AppContext.Users.Single(r => r.Id == id).Role.Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Helper.ExecuteParentUserControlMethod(this, "UserManage", "SwitchWindow", "Edit", id);
        }

        private void details_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var id = Convert.ToInt32((sender as TextBlock).Tag);
            Helper.ExecuteParentUserControlMethod(this, "UserManage", "SwitchWindow", "Details", id);
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
