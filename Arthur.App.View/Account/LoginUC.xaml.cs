using Arthur.App;
using Arthur.Core.Security;
using Arthur.App.View.Utils;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Arthur.App.View.Account
{
    /// <summary>
    /// LoginUC.xaml 的交互逻辑
    /// </summary>
    public partial class LoginUC : UserControl
    {
        public LoginUC()
        {
            InitializeComponent();

            if (Current.Option.RememberUserId < 1)
            {
                this.remember_me.IsChecked = false;
            }
            else
            {
                var user = new App.Model.User();
                using (var db = new Arthur.App.AppContext())
                {
                    user = db.Users.FirstOrDefault(o => o.Id == Current.Option.RememberUserId);
                }
                if (user.Id > 0)
                {
                    this.username.Text = user.Name;
                    this.password.Password = EncryptBase64Helper.DecodeBase64(user.Password);
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var ret = Arthur.App.Business.Account.Login(this.username.Text, this.password.Password);
            if (ret.IsSucceed)
            {
                if (this.remember_me.IsChecked.Value)
                {
                    Current.Option.RememberUserId = Current.User.Id;
                }
                else
                {
                    Current.Option.RememberUserId = -1;
                }
                Arthur.App.Business.Logging.AddEvent(string.Format("{0}登录", this.username.Text), App.Model.EventType.信息);
                Helper.ExecuteParentWindowMethod(this, "LoginSuccessInvoke");
            }
            else
            {
                lbTip.Content = ret.Msg;
                lbTip.Visibility = Visibility.Visible;
            }
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            lbTip.Visibility = Visibility.Hidden;
        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentWindowMethod(this, "SwitchWindow");
        }

        private void remember_me_Click(object sender, RoutedEventArgs e)
        {
            if (!this.remember_me.IsChecked.Value)
            {
                Current.Option.RememberUserId = -1;
            }
        }
    }
}
