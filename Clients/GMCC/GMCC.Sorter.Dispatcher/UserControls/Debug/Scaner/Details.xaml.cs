using Arthur.App;
using Arthur.App.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Debug.Scaner
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {

        public Details(int id)
        {
            InitializeComponent();
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            var btnName = (sender as Button).Name;
            var index = Convert.ToInt32(btnName.Substring(7));

            CommorViewModel scaner;
            
            if (index == 1) scaner = Current.BatteryScaner;
            else if (index == 2) scaner = Current.BindTrayScaner;
            else scaner = Current.UnbindTrayScaner;

            var type = scaner.GetType();
            var scanCommand = type.GetProperty("ScanCommand").GetValue(scaner);


            new Thread(() => {
                this.Dispatcher.Invoke(new Action(() => {
                    var result = scaner.Commor.Comm(scanCommand as string, scaner.ReadTimeout);
                    if (result.IsSucceed)
                    {
                        var tbRetMsg = ControlsSearchHelper.GetChildObject<TextBox>(this, "tbRetMsg" + index);
                        tbRetMsg.Text = result.Data.ToString();
                    }
                    else
                    {
                        this.tip.Content = result.Msg;
                        this.tip.Visibility = Visibility.Visible;
                    }
                }));
            }).Start();

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            var btnName = (sender as Button).Name;
            var index = btnName.Substring(8);
            var tbRetMsg = ControlsSearchHelper.GetChildObject<TextBox>(this, "tbRetMsg" + index);
            tbRetMsg.Text = "";
        }


        private void Tip_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.tip.Content = "";
            this.tip.Visibility = Visibility.Collapsed;
        }
    }
}
