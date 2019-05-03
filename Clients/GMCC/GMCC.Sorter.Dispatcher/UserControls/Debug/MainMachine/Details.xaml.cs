using Arthur.App;
using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Debug.MainMachine
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {

        public Details(int id)
        {
            InitializeComponent();
        }


        private void ReadClear_Click(object sender, RoutedEventArgs e)
        {
            this.read_value.Text = "";
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }


        private void Read_Click(object sender, RoutedEventArgs e)
        {
            new Thread(() => {
                this.Dispatcher.Invoke(new Action(() => {
                    var result = Current.MainMachine.Commor.Read(this.read_addr.Text.Trim());
                    if (result.IsOk)
                    {
                        this.read_value.Text = result.Data.ToString();
                    }
                    else
                    {
                        this.tip.Content = result.Msg;
                        this.tip.Visibility = Visibility.Visible;
                    }
                }));
            }).Start();
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(this.write_value.Text))
            {
                this.tip.Content = "请输入要写入的数值！";
                this.tip.Visibility = Visibility.Visible;
                return;
            }

            if (ushort.TryParse(this.write_value.Text.ToString(), out ushort val))
            {
                this.tip.Content = "输入数值有误！";
                this.tip.Visibility = Visibility.Visible;
                return;
            }

            new Thread(() => {
                this.Dispatcher.Invoke(new Action(() => {
                    var result = Current.MainMachine.Commor.Write(this.write_addr.Text.Trim(), val);
                    if (result.IsOk)
                    {
                        this.write_result.Content = "成功";
                        return;
                    }
                    else
                    {
                        this.tip.Content = result.Msg;
                        this.tip.Visibility = Visibility.Visible;
                    }
                    this.write_result.Content = "失败";
                }));
            }).Start();
        }

        private void Tip_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.tip.Content = "";
            this.tip.Visibility = Visibility.Collapsed;
        }
    }
}
