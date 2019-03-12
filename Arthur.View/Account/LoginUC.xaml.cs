using Arthur.App;
using Arthur.Security;
using Arthur.View.Utils;
using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Arthur.View.Account
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
                var user = Arthur.Business.Account.GetUser(Current.Option.RememberUserId);
                if (user.Id > 0)
                {
                    this.username.Text = user.Name;
                    this.password.Password = EncryptHelper.DecodeBase64(user.Password);
                }
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var ret = Arthur.Business.Account.Login(this.username.Text, this.password.Password);
            if (ret.IsOk)
            {
                if (this.remember_me.IsChecked.Value)
                {
                    Current.Option.RememberUserId = Current.User.Id;
                }
                else
                {
                    Current.Option.RememberUserId = -1;
                }

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
