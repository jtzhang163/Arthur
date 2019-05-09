using Arthur.App;
using Arthur.App.Data;
using Arthur.App.Model;
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

namespace Arthur.View.Account.User
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private readonly App.AppContext _AppContext = new App.AppContext();
        private Arthur.App.Model.User User;

        public Edit(int id)
        {
            InitializeComponent();
            this.User = _AppContext.Users.FirstOrDefault(o => o.Id == id);
            this.DataContext = this.User;

            this.role.SelectedIndex = _AppContext.Roles.ToList().IndexOf(this.User.Role);

            if (this.User.Role.Level >= Current.User.Role.Level)
            {
                this.role.IsHitTestVisible = false;
                this.isEnabled.IsEnabled = false;
            }
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
            Helper.ExecuteParentUserControlMethod(this, "UserManage", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var gender = GetGender();
            var number = this.number.Text.Trim();
            var phoneNumber = this.phoneNumber.Text.Trim();
            var email = this.email.Text.Trim();
            var isEnabled = this.isEnabled.IsChecked;
            var role = _AppContext.Roles.ToList()[this.role.SelectedIndex];

            try
            {
                if (role.Level > Current.User.Role.Level)
                {
                    throw new Exception("用户角色等级不能大于当前用户角色！");
                }    
                this.User.Gender = gender;
                this.User.Number = number;
                this.User.PhoneNumber = phoneNumber;
                this.User.Email = email;
                this.User.IsEnabled = isEnabled.Value;
                this.User.Role = role;

                _AppContext.SaveChanges();
                tip.Background = new SolidColorBrush(Colors.Green);
                tip.Text = "修改信息成功！";

            }
            catch (Exception ex)
            {
                tip.Background = new SolidColorBrush(Colors.Red);
                tip.Text = "修改信息失败：" + ex.Message;
            }

            tip.Visibility = Visibility.Visible;
        }

        public Gender GetGender()
        {
            for (int i = 0; i < 3; i++)
            {
                var radioButton = ControlsSearchHelper.GetChildObject<RadioButton>(gender, string.Format("gender{0}", i));
                if (radioButton.IsChecked.Value)
                {
                    var gender = radioButton.Content.ToString();
                    return gender == "男" ? Gender.Male : gender == "女" ? Gender.Female : Gender.Unknown;
                }
            }
            return Gender.Unknown;
        }
    }
}
