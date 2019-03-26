using Arthur.View.Account;
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
using GMCC.Sorter.Data;

namespace GMCC.Sorter.Dispatcher
{
    /// <summary>
    /// AccountWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AccountWindow : Window, IAccountView
    {
        public string Option { get; set; }

        public AccountWindow() : this("login")
        {
            // this.Title = new Utility().AppTitle;
            this.DataContext = Current.App;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">登录：login; 注册：register</param>
        public AccountWindow(string option)
        {
            //往数据库播种数据
            new DbInitializer().Initialize();

            //如果重复启动该程序，弹窗提示
            if (Arthur.App.Application.ThisAppIsAlreadyRunning())
            {
                MessageBox.Show("程序已经在运行，请勿重复启动！");
                Application.Current.Shutdown();
                return;
            }

            this.Option = option;
            InitializeComponent();

            if (this.Option == "login")
            {
                LoadLoginUC();
            }

        }

        public void LoginSuccessInvoke()
        {
            new MainWindow().Show();
            this.Close();
        }

        public void SwitchWindow()
        {
            if (this.Option == "login")
            {
                grid.Children.Clear();
                LoadRegisterUC();
                this.Option = "register";
            }
            else
            {
                grid.Children.Clear();
                LoadLoginUC();
                this.Option = "login";
            }
        }

        private void LoadRegisterUC()
        {
            RegisterUC registerUC = new RegisterUC();
            registerUC.Height = 350;
            registerUC.Width = 450;
            registerUC.HorizontalAlignment = HorizontalAlignment.Center;
            registerUC.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(registerUC);
        }

        private void LoadLoginUC()
        {
            LoginUC loginUC = new LoginUC();
            loginUC.Height = 350;
            loginUC.Width = 450;
            loginUC.HorizontalAlignment = HorizontalAlignment.Center;
            loginUC.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(loginUC);
        }
    }
}
