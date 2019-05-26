using Arthur.App.Data;
using Arthur.App.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Query.Battery
{
    /// <summary>
    /// Create.xaml 的交互逻辑
    /// </summary>
    public partial class Create : UserControl
    {
        public Create(int id)
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "Battery", "SwitchWindow", "Index", 0);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            var code = this.code.Text.Trim();

            tip.Background = new SolidColorBrush(Colors.Red);

            if (string.IsNullOrWhiteSpace(code))
            {                
                tip.Text = "请填写数据！";
            }
            else
            {
                var ret = new Business.BatteryManage().Create(new Model.Battery() { Code = code});
                if (ret.IsOk)
                {
                    tip.Background = new SolidColorBrush(Colors.Green);
                    tip.Text = "新增电池成功！";
                }
                else
                {
                     
                    tip.Text = ret.Msg;
                }
            }

            tip.Visibility = Visibility.Visible;
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Collapsed;
        }
    }
}
