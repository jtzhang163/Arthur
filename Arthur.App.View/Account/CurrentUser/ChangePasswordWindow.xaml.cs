using Arthur.App;
using Arthur.App.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Arthur.App.View.Account
{
    /// <summary>
    /// ChangePasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private readonly App.AppContext _AppContext = new App.AppContext();
        private Arthur.App.Model.User User;

        public ChangePasswordWindow(int id)
        {
            InitializeComponent();
            this.User = _AppContext.Users.FirstOrDefault(o => o.Id == id);
            this.DataContext = this.User;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var old_pwd = this.old_pwd.Password.Trim();
            var new_pwd = this.new_pwd.Password.Trim();
            var confirm_new_pwd = this.confirm_new_pwd.Password.Trim();

            var ret = Arthur.App.Business.Account.ChangePassword(this.User.Name, old_pwd, new_pwd, confirm_new_pwd);
            if (ret.IsSucceed)
            {
                lbTip.Text = "修改密码成功！";
                lbTip.Foreground = new SolidColorBrush(Colors.Green);
                Arthur.App.Business.Logging.AddOplog(string.Format("用户中心. 修改密码", "", ""), Arthur.App.Model.OpType.编辑);
            }
            else
            {
                lbTip.Text = ret.Msg;
                lbTip.Foreground = new SolidColorBrush(Colors.Red);
            }
            lbTip.Visibility = Visibility.Visible;
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            lbTip.Visibility = Visibility.Hidden;
        }
    }
}
