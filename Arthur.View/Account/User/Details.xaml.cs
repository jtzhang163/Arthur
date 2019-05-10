using Arthur.App;
using Arthur.App.Data;
using Arthur.App.Utils;
using Arthur.View.Utils;
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

namespace Arthur.View.Account.User
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {
        private readonly App.AppContext _AppContext = new App.AppContext();
        public Details(int id)
        {
            InitializeComponent();
            this.DataContext = ContextToViewModel.Convert(_AppContext.Users.FirstOrDefault(o => o.Id == id));
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "UserManage", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Button).Tag);
            var user = _AppContext.Users.SingleOrDefault(r => r.Id == id);
            if (user == null)
            {
                MessageBox.Show("用户不存在！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Current.User.Id != user.Id && Current.User.Role.Level <= user.Role.Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Helper.ExecuteParentUserControlMethod(this, "UserManage", "SwitchWindow", "Edit", id);
        }
    }
}
