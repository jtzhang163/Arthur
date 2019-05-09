using Arthur.App.Data;
using Arthur.View.Utils;
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

namespace Arthur.View.Account.Role
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private readonly App.AppContext _AppContext = new App.AppContext();
        private Arthur.App.Model.Role Role;

        public Edit(int id)
        {
            InitializeComponent();
            this.Role = _AppContext.Roles.FirstOrDefault(o => o.Id == id);
            this.DataContext = this.Role;
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
            Helper.ExecuteParentUserControlMethod(this, "RoleManage", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var name = this.name.Text.Trim();
            var level = this.level.Text.Trim();

            tip.Background = new SolidColorBrush(Colors.Red);

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(level))
            {  
                tip.Text = "请填写数据！";
            }
            else if (!int.TryParse(level, out int iLevel))
            {  
                tip.Text = "角色等级输入有误！";
            }
            else if (iLevel > _AppContext.Roles.Single(r => r.Name == "系统管理员").Level)
            {    
                tip.Text = "角色等级不能大于管理员等级！";
            }
            else
            {
                try
                {
                    this.Role.Name = name;
                    this.Role.Level = iLevel;

                    _AppContext.SaveChanges();

                    tip.Background = new SolidColorBrush(Colors.Green);
                    tip.Text = "修改信息成功！";
                }
                catch (Exception ex)
                {                    
                    tip.Text = "修改信息失败：" + ex.Message;
                }
            }
            tip.Visibility = Visibility.Visible;
        }
    }
}
