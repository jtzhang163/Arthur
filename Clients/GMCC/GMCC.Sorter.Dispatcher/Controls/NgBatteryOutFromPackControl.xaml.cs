using GMCC.Sorter.Business;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GMCC.Sorter.Dispatcher.Controls
{
    /// <summary>
    /// TrayBattery.xaml 的交互逻辑
    /// </summary>
    public partial class NgBatteryOutFromPackControl : UserControl
    {
        private SortResult sortResult;
        private int packId;
        public NgBatteryOutFromPackControl()
        {
            InitializeComponent();
            this.btnConfirm.IsEnabled = false;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            var code = this.battery_code.Text.Trim();
            var result = BatteryManage.NgBatteryOutFromPack(code);
            if (result.IsOk)
            {
                tip.Background = new SolidColorBrush(Colors.Green);
                tip.Text = "移除成功";
                tip.Visibility = Visibility.Visible;

                var sortPack = Current.SortPacks.FirstOrDefault(o => o.SortResult == sortResult);
                if (sortPack != null)
                {
                    sortPack.Count = PackManage.GetPackCount(this.packId);
                }

                this.sort_result.Content = "";
                this.pack_code.Content = "";
                this.pack_battery_count.Content = "";
            }
            else
            {
                tip.Background = new SolidColorBrush(Colors.Red);
                tip.Text = result.Msg;
                tip.Visibility = Visibility.Visible;
            }
        }

        private void Battery_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var code = (sender as TextBox).Text.Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    var battery = GetObject.GetByCode<Battery>(code);
                    if (battery.Id > 0)
                    {
                        this.sortResult = battery.SortResult;
                        this.sort_result.Content = this.sortResult;
                        packId = battery.PackId;
                        var pack = GetObject.GetById<Pack>(packId);
                        if (pack.Id > 0)
                        {
                            this.pack_code.Content = pack.Code;
                            this.pack_battery_count.Content = PackManage.GetPackCount(packId);
                            tip.Visibility = Visibility.Collapsed;
                            this.btnConfirm.IsEnabled = true;
                        }
                        else
                        {
                            tip.Background = new SolidColorBrush(Colors.Red);
                            tip.Text = "该电池尚未绑定箱体！";
                            tip.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        tip.Background = new SolidColorBrush(Colors.Red);
                        tip.Text = "系统中不存在该电池！";
                        tip.Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                tip.Visibility = Visibility.Collapsed;
            }
        }

        private void Battery_code_GotFocus(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Collapsed;
            this.btnConfirm.IsEnabled = false;
        }
    }
}
