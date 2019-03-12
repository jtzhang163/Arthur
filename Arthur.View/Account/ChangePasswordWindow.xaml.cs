using Arthur.App;
using System.Windows;
using System.Windows.Media;

namespace Arthur.View.Account
{
    /// <summary>
    /// ChangePasswordWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
            this.username.Content = Current.User.Name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var old_pwd = this.old_pwd.Password.Trim();
            var new_pwd = this.new_pwd.Password.Trim();
            var confirm_new_pwd = this.confirm_new_pwd.Password.Trim();

            var ret = Arthur.Business.Account.ChangePassword(Current.User.Name, old_pwd, new_pwd, confirm_new_pwd);
            if (ret.IsOk)
            {
                lbTip.Text = "修改密码成功！";
                lbTip.Foreground = new SolidColorBrush(Colors.Green);
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
