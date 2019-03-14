using Arthur.App.Data;
using Arthur.View.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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

namespace Arthur.View.Account.Role
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        public Index()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = Context.AccountContext.Roles.ToList();
            //var item = new ListBoxItem();

            //var label = new Label();
            //label.Content = "aaaaa";

            //item.Content = label;
            //listView.Items.Add(item);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Create", 0);
        }



        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Hyperlink).Tag);
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Edit", id);
        }

        private void details_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Hyperlink).Tag);
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Details", id);
        }


        private void delete_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Hyperlink).Tag);
            var role = Context.AccountContext.Roles.SingleOrDefault(r => r.Id == id);
            // int count = Context.AccountContext.Roles.Count(r => r.Id == id);
            if (role == null)
            {
                MessageBox.Show("不存在该角色，删除失败！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show(string.Format("确定要删除角色【{0}】吗？", role.Name), "删除确认", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                //var role = new Arthur.App.Model.Role() { Id = id };
                //Context.AccountContext.Entry(role).State = EntityState.Deleted;
                Context.AccountContext.Roles.Remove(role);
                Context.AccountContext.SaveChanges();
                UserControl_Loaded(null, null);
            }
        }
    }
}
