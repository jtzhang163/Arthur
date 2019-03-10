using System;
using System.Collections.Generic;
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
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var ret = Arthur.App.Account.Login(this.username.Text, this.password.Password);
            if (ret.IsOk)
            {
                Window parentWindow = Window.GetWindow(this);
                Type type = parentWindow.GetType();
                MethodInfo mi = type.GetMethod("LoginSuccessInvoke");
                mi.Invoke(parentWindow, new object[] { });
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
            //if(sender is TextBox)
            //{
            //    var username = sender as TextBox;
                
            //}
            //else
            //{

            //}
        }
    }

    public class PasswordBoxMonitor : DependencyObject
    {
        public static bool GetIsMonitoring(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMonitoringProperty);
        }

        public static void SetIsMonitoring(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMonitoringProperty, value);
        }

        public static readonly DependencyProperty IsMonitoringProperty =
            DependencyProperty.RegisterAttached("IsMonitoring", typeof(bool), typeof(PasswordBoxMonitor), new UIPropertyMetadata(false, OnIsMonitoringChanged));

        public static readonly DependencyProperty PasswordLengthProperty =
          DependencyProperty.RegisterAttached("PasswordLength", typeof(int), typeof(PasswordBoxMonitor), new UIPropertyMetadata(0));

        public static int GetPasswordLength(DependencyObject obj)
        {
            int length = (int)obj.GetValue(PasswordLengthProperty);
            System.Diagnostics.Debug.WriteLine("Password Length:{0}!!!!!!!!!!!!!!", length.ToString());
            return length;
        }

        public static void SetPasswordLength(DependencyObject obj, int value)
        {
            obj.SetValue(PasswordLengthProperty, value);
        }

        private static void OnIsMonitoringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pb = d as PasswordBox;
            if (pb == null)
            {
                return;
            }
            if ((bool)e.NewValue)
            {
                pb.PasswordChanged += PasswordChanged;
            }
            else
            {
                pb.PasswordChanged -= PasswordChanged;
            }
        }

        static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var pb = sender as PasswordBox;
            if (pb == null)
            {
                return;
            }
            SetPasswordLength(pb, pb.Password.Length);
        }
    }
}
