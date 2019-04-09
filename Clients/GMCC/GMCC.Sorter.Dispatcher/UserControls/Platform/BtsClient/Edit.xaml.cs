using Arthur.Utils;
using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Utils;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Platform.BtsClient
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private ShareDataViewModel ShareData;

        public Edit(int id)
        {
            InitializeComponent();
            this.ShareData = Current.ShareDatas.Single(o => o.Id == id);
            this.DataContext = this.ShareData;
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
            Helper.ExecuteParentUserControlMethod(this, "BtsClient", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var value = this.value.Text.Trim();
            var status = this.status.Text.Trim();

            tip.Background = new SolidColorBrush(Colors.Red);

            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(status))
            {
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {
                    // 校验 Value 字段是否是 Json 字符串
                    if (this.ShareData.Key == "SortingResults")
                    {
                        JsonHelper.DeserializeJsonToObject<SortingResult>(value);
                    }
                    else
                    {
                        JsonHelper.DeserializeJsonToObject<BindCode>(value);
                    }

                    Current.ShareDatas.Single(o => o.Id == this.ShareData.Id).Value = value;
                    Current.ShareDatas.Single(o => o.Id == this.ShareData.Id).Status = Convert.ToInt32(status);

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
