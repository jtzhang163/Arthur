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

namespace Arthur.App.View.Account.Role
{
    /// <summary>
    /// Create.xaml 的交互逻辑
    /// </summary>
    public partial class Create : UserControl
    {
        private readonly App.AppContext _AppContext = new App.AppContext();
        public Create(int id)
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Index", 0);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            var level = this.level.Text.Trim();
            var name = this.name.Text.Trim();

            tip.Background = new SolidColorBrush(Colors.Red);

            if (string.IsNullOrWhiteSpace(level) || string.IsNullOrWhiteSpace(name))
            {                
                tip.Text = "请填写数据！";
            }
            else if (!int.TryParse(level, out int iLevel))
            {                 
                tip.Text = "角色等级输入有误！";
            }
            else if (iLevel >= _AppContext.Roles.Single(r => r.Name == "系统管理员").Level)
            {                
                tip.Text = "角色等级必须小于系统管理员！";
            }
            else
            {
                var ret = new Arthur.App.Business.RoleManage().Create(new Arthur.App.Model.Role() { Level = iLevel, Name = name });
                if (ret.IsSucceed)
                {
                    tip.Background = new SolidColorBrush(Colors.Green);
                    tip.Text = "新增角色成功！";
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
