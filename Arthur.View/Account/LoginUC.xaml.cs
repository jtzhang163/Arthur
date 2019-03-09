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
            //lbTip.Content = "用户名或密码错误！";
            //lbTip.Visibility = Visibility.Visible;

            Window parentWindow = Window.GetWindow(this);
            Console.WriteLine(parentWindow.Title);

            Type type = parentWindow.GetType();
            MethodInfo mi = type.GetMethod("LoginSuccessInvoke");

            mi.Invoke(parentWindow, new object[] { });
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            lbTip.Visibility = Visibility.Hidden;
        }
    }
}
