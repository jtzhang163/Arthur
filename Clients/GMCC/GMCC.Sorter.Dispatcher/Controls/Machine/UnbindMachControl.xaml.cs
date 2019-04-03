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
    public partial class UnbindMachControl : UserControl
    {
        private MainMachineViewModel MainMachine;
        public UnbindMachControl()
        {
            InitializeComponent();
            this.MainMachine = Current.MainMachine;
            this.DataContext = this.MainMachine;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowWindows.ShowTrayBatteryWin(this.MainMachine.UnbindProcTrayId);
        }
    }
}
