using GMCC.Sorter.Business;
using GMCC.Sorter.Dispatcher.Utils;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
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
    public partial class SortPackControl : UserControl
    {
        public SortPackControl()
        {
            InitializeComponent();
            this.sort_pack_list.ItemsSource = Current.SortPacks;
        }

        private void btnNgBatteryOutFromPack_Click(object sender, RoutedEventArgs e)
        {
            ShowWindows.NgBatteryOutFromPack();
        }

        private void btnCreateQRCode_Click(object sender, RoutedEventArgs e)
        {
            //var result = QRCoderManage.Create(1);

        }
    }
}
