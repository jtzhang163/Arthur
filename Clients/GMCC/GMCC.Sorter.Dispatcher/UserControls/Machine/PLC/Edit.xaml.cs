﻿using Arthur.Utility;
using Arthur.View.Utils;
using GMCC.Sorter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Machine.PLC
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        public Edit(int id)
        {
            InitializeComponent();
            this.DataContext = Current.MainMachine;
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Collapsed;
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "PlcView", "SwitchWindow", "Details", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var company = this.company.Text.Trim();
            var modelNumber = this.model_number.Text.Trim();
            var ip = this.ip.Text.Trim();
            var port = _Convert.StrToInt(this.port.Text.Trim(), -1);
            var comm_interval = this.comm_interval.Text.Trim();

            if (string.IsNullOrWhiteSpace(ip)|| string.IsNullOrWhiteSpace(this.port.Text))
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {
                    Current.MainMachine.Company = company;
                    Current.MainMachine.ModelNumber = modelNumber;
                    Current.MainMachine.IP = ip;
                    Current.MainMachine.Port = port;
                    Current.MainMachine.CommInterval = Convert.ToInt32(comm_interval);

                    Context.AppContext.SaveChanges();
                    tip.Foreground = new SolidColorBrush(Colors.Green);
                    tip.Text = "修改信息成功！";
                }
                catch (Exception ex)
                {
                    tip.Foreground = new SolidColorBrush(Colors.Red);
                    tip.Text = "修改信息失败：" + ex.Message;
                }
            }
            tip.Visibility = Visibility.Visible;
        }
    }
}
