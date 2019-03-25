using Arthur.View.Utils;
using GMCC.Sorter.Data;
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
                var storageUC = new Controls.Machine.StorageUC(o.Id);
                this.grid.Children.Add(storageUC);
                SetRowCol(storageUC);
            });
        }

        private void SetRowCol(Controls.Machine.StorageUC storageUC)
        {

            var row = 0;
            var col = 0;

            if (storageUC.Col < Common.STOR_COL_COUNT / 2 + 1)
            {
                row = 6 - storageUC.Floor;
                col = 1 + storageUC.Col;
            }
            else
            {
                row = 13 - storageUC.Floor;
                col = storageUC.Col - 8;
            }

            Grid.SetRow(storageUC, row);
            Grid.SetColumn(storageUC, col);
        }

    }
}
