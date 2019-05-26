using Arthur.App.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.ViewModel;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Machine.Storage
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private readonly Data.AppContext _AppContext = new Data.AppContext();
        private StorageViewModel Storage;

        public Edit(int id)
        {
            InitializeComponent();
            this.Storage = Current.Storages.Single(t => t.Id == id);
            this.DataContext = this.Storage;
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
            Helper.ExecuteParentUserControlMethod(this, "Storage", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var name = this.name.Text.Trim();
            var company = this.company.Text.Trim();
            var isEnabled = this.isEnabled.IsChecked;

            tip.Background = new SolidColorBrush(Colors.Red);

            if (string.IsNullOrWhiteSpace(name))
            {
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {
                    this.Storage.Name = name;
                    this.Storage.Company = company;
                    //确保每层设置一次，且不出现死循环
                    Current.Storages.FirstOrDefault(o => o.Column == Storage.Column && o.Floor == Common.STOR_FLOOR_COUNT).IsEnabled = isEnabled.Value;

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
