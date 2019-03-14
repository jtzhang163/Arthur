﻿using Arthur.App;
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

namespace SzYitong.Bis.App.UserControls
{
    /// <summary>
    /// CurrentUserInfoUC.xaml 的交互逻辑
    /// </summary>
    public partial class CurrentUserInfoUC : UserControl
    {
        public CurrentUserInfoUC()
        {
            InitializeComponent();
            this.DataContext = Current.User;
        }

        private void change_pwd_Click(object sender, RoutedEventArgs e)
        {
            new ChangePasswordWindow().ShowDialog();
        }

        private void change_info_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}