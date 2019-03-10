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
    public partial class RegisterUC : UserControl
    {
        public RegisterUC()
        {
            InitializeComponent();

        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var ret = Arthur.Business.Account.Register(this.username.Text, this.password.Password, this.password_confirm.Password);
            if (ret.IsOk)
            {
                new Thread(() => {

                    this.Dispatcher.Invoke(new Action(() => {
                       // this.btnRegister.IsEnabled = false;
                        this.btnRegister.Content = "注册成功！稍后请登录...";
                    }));

                    Thread.Sleep(3000);

                    this.Dispatcher.Invoke(new Action(() => {
                        Helper.ExecuteParentWindowMethod(this, "SwitchWindow");
                    }));

                }).Start();
                
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

        private void login_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentWindowMethod(this, "SwitchWindow");
        }
    }
}
