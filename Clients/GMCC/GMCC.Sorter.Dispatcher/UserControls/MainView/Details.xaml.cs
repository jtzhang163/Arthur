using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Dispatcher.UserControls.Machine;
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
            //;

            for (var i = 0; i < Common.STOR_COL_COUNT; i++)
            {
                for (var j = 0; j < Common.STOR_FLOOR_COUNT; j++)
                {

                    var storageUC = new StorageUC(i, j);
                    this.grid.Children.Add(storageUC);
                    SetRowCol(storageUC);

                }
            }
        }

        private void SetRowCol(StorageUC storageUC)
        {

            var row = 0;
            var col = 0;

            if (storageUC.Col < Common.STOR_COL_COUNT / 2)
            {
                row = 5 - storageUC.Floor;
                col = 2 + storageUC.Col;
            }
            else
            {
                row = 12 - storageUC.Floor;
                col = storageUC.Col - 7;
            }

            Grid.SetRow(storageUC, row);
            Grid.SetColumn(storageUC, col);
        }

    }
}
