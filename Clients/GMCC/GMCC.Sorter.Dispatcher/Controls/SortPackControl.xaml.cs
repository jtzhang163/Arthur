using GMCC.Sorter.Business;
using GMCC.Sorter.Dispatcher.Utils;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using GMCC.Sorter.ViewModel;
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

        private void btnFinishPack_Click(object sender, RoutedEventArgs e)
        {
            var sortPack = (SortPackViewModel)sort_pack_list.SelectedItem;
            if (sortPack == null)
            {
                Running.ShowErrorMsg("请选中要打包的类型！");
                return;
            }

            var text = string.Format("当前您选中的打包类型为：{0}, 箱体中包含电池个数：{1}\r\n确定要结束打包?", sortPack.SortResult, sortPack.Count);
            if (MessageBox.Show(text, "结束打包确认", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
            {
                return;
            }

            var result = PackManage.Finish(sortPack);
            if (result.IsFailed)
            {
                Running.ShowErrorMsg(result.Msg);
                return;
            }

            //打开保存二维码的文件夹
            var dirPath = QRCoderManage.GetSaveQRCodeDirPath(sortPack.SortResult);
            try
            {
                System.Diagnostics.Process.Start(dirPath);
            }
            catch(Exception ex)
            {
                Running.ShowErrorMsg(ex.Message);
            }
        }
    }
}
