using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Dispatcher.Controls.Machine;
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

namespace GMCC.Sorter.Dispatcher.UserControls.MainView
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {
        public Details(int id)
        {
            InitializeComponent();

            Current.Storages.ForEach(o =>
            {
                var storageUC = new Controls.Machine.StorageControl(o.Id);
                this.grid.Children.Add(storageUC);
                SetRowCol(storageUC);
            });

            this.jaw.Children.Add(new JawControl());

            this.bind_mach.Children.Add(new BindMachControl());

            this.unbind_mach.Children.Add(new UnbindMachControl());
        }

        private void SetRowCol(Controls.Machine.StorageControl storageControl)
        {

            var row = 0;
            var col = 0;

            if (storageControl.Col < Common.STOR_COL_COUNT / 2 + 1)
            {
                row = 6 - storageControl.Floor;
                col = 1 + storageControl.Col;
            }
            else
            {
                row = 13 - storageControl.Floor;
                col = storageControl.Col - 8;
            }

            Grid.SetRow(storageControl, row);
            Grid.SetColumn(storageControl, col);
        }

    }


}
