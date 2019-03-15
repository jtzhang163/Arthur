using Arthur.View.Utils;
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

namespace Arthur.View.Account.User
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
            Helper.ExecuteParentUserControlMethod(this, "UserManage", "SwitchWindow", "Index", 0);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            var level = this.level.Text.Trim();
            var name = this.name.Text.Trim();

            if (string.IsNullOrWhiteSpace(level) || string.IsNullOrWhiteSpace(name))
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
                tip.Text = "请填写数据！";
            }
            else if (!int.TryParse(level, out int iLevel))
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
                tip.Text = "用户等级输入有误！";
            }
            else
            {
                var ret = new Arthur.Business.UserManage().Create(new Arthur.App.Model.User() { Name = name });
                if (ret.IsOk)
                {
                    tip.Foreground = new SolidColorBrush(Colors.Green);
                    tip.Text = "新增用户成功！";
                }
                else
                {
                    tip.Foreground = new SolidColorBrush(Colors.Red);
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
            tip.Visibility = Visibility.Hidden;
        }
    }
}
