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

namespace GMCC.Sorter.Dispatcher.UserControls.SystemUC.ParamUC
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        public Edit(int id)
        {
            InitializeComponent();
            this.DataContext = Current.App;
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
            Helper.ExecuteParentUserControlMethod(this, "SystemParam", "SwitchWindow", "Details", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var appName = this.app_name.Text.Trim();
            var companyName = this.company_name.Text.Trim();
            var gridPageSize = this.grid_page_size.Text.Trim();

            if (string.IsNullOrWhiteSpace(appName) || string.IsNullOrWhiteSpace(companyName) || string.IsNullOrWhiteSpace(gridPageSize))
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {
                    var pageSize = int.Parse(gridPageSize);

                    Current.App.AppName = appName;
                    Current.App.CompanyName = companyName;
                    Current.App.DataGridPageSize = pageSize;

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
