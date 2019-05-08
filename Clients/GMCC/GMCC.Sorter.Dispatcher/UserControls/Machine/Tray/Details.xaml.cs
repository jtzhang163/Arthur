using Arthur.App;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Machine.Tray
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {
        private readonly Data.AppContext _AppContext = new Data.AppContext();
        private GMCC.Sorter.Model.Tray Tray;
        public Details(int id)
        {
            InitializeComponent();
            this.Tray = _AppContext.Trays.Single(t => t.Id == id);
            this.DataContext = this.Tray;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "Tray", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Role.Level < Arthur.App.Data.Context.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = Convert.ToInt32((sender as Button).Tag);
            Helper.ExecuteParentUserControlMethod(this, "Tray", "SwitchWindow", "Edit", id);
        }
    }
}
