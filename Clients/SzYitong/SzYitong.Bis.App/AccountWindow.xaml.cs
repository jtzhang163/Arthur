﻿using Arthur.View.Account;
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

namespace SzYitong.Bis.App
{
    /// <summary>
    /// AccountWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AccountWindow : Window, IAccountView
    {
        public string Option { get; set; }

        public AccountWindow() : this("login")
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">登录：login; 注册：register</param>
        public AccountWindow(string option)
        {
            this.Option = option;
            InitializeComponent();

            //----------------------动态添加用户控件 登录/注册-------------------------
            if(this.Option == "login")
            {
                LoginUC loginUC = new LoginUC();
                loginUC.Height = 300;
                loginUC.Width = 450;
                loginUC.HorizontalAlignment = HorizontalAlignment.Center;
                loginUC.VerticalAlignment = VerticalAlignment.Center;
                grid.Children.Add(loginUC);
            }

        }


        public void LoginSuccessInvoke()
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
