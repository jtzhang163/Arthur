﻿using Arthur.View.Utils;
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

namespace Arthur.View.Account.CurrentUser
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {
        public Details(int id)
        {
            InitializeComponent();
            this.DataContext = Arthur.Business.Account.GetUser(id);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Button).Tag);
            Helper.ExecuteParentUserControlMethod(this, "CurrentUser", "SwitchWindow", "Edit", id);
        }

        private void changePwd_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Button).Tag);
            var win = new ChangePasswordWindow(id);
            win.ShowDialog();
        }
    }
}