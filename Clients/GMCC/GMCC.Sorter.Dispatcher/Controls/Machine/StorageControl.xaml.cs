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
    /// StorageUC.xaml 的交互逻辑
    /// </summary>
    public partial class StorageControl : UserControl
    {
        private StorageViewModel storage;

        public int Col;
        public int Floor;

        public StorageControl(int id)
        {

            InitializeComponent();
            storage = Current.Storages.FirstOrDefault(o => o.Id == id);
            this.DataContext = storage;
            this.Col = storage.Column;
            this.Floor = storage.Floor;
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShowWindows.ShowTrayBatteryWin(storage.ProcTrayId);
        }
    }
}
