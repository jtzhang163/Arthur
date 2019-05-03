using GMCC.Sorter.Dispatcher.Utils;
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

namespace GMCC.Sorter.Dispatcher.Controls.Machine
{
    /// <summary>
    /// JawControl.xaml 的交互逻辑
    /// </summary>
    public partial class JawControl : UserControl
    {
        private AppOption Option;

        public JawControl()
        {
            InitializeComponent();
            this.Option = Current.Option;
            this.DataContext = this.Option;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowWindows.ShowTrayBatteryWin(this.Option.JawProcTrayId);
        }
    }
}
