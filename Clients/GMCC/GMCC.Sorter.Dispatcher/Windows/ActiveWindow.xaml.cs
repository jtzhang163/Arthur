using Arthur.Core.Security;
using Arthur.Core.Transfer;
using GMCC.Sorter.Run;
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
using System.Windows.Shapes;

namespace GMCC.Sorter.Dispatcher
{
    /// <summary>
    /// ActiveWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ActiveWindow : Window
    {
        public ActiveWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var todayEncode = EncryptBase64Helper.EncodeBase64(DateTime.Now.ToString("yyyyMMdd"));

            var tmpStr = this.tbActiveCode.Text.Trim();
            if (!tmpStr.Contains(todayEncode))
            {
                MessageBox.Show("输入激活码有误！");
                return;
            }

            try
            {
                var expireMunites = _Convert.To<int>(EncryptBase64Helper.DecodeBase64(EncryptBase64Helper.DecodeBase64(tmpStr.Replace(todayEncode, ""))), 0);
                Arthur.App.Current.Option.RemainingMinutes = expireMunites;
                MessageBox.Show("更新激活码成功！");
                TimerExec.ExpireTimeExec(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入激活码有误！");
            }
        }
    }
}
