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

namespace Arthur.View.Account.CurrentUser
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private Arthur.App.Model.User User;

        public Edit(int id)
        {
            InitializeComponent();
            this.User = Arthur.Business.Account.GetUser(id);
            this.DataContext = this.User;

            this.role.SelectedIndex = Context.Roles.ToList().IndexOf(this.User.Role);
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Hidden;
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            var id = Convert.ToInt32((sender as Button).Tag);
            Helper.ExecuteParentUserControlMethod(this, "CurrentUser", "SwitchWindow", "Details", id);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var gender = GetGender();
            var number = this.number.Text.Trim();
            var phoneNumber = this.phoneNumber.Text.Trim();
            var email = this.email.Text.Trim();

            try
            {
                this.User.Gender = gender;
                this.User.Number = number;
                this.User.PhoneNumber = phoneNumber;
                this.User.Email = email;

                Arthur.App.Data.Context.AccountContext.SaveChanges();
                tip.Foreground = new SolidColorBrush(Colors.Green);
                tip.Text = "修改信息成功！";

            }
            catch (Exception ex)
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
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
