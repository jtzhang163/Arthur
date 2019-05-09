using Arthur.App;
using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Utils;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Platform.BtsClient
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {
        private readonly Arthur.App.AppContext _ArthurContext = new Arthur.App.AppContext();
        private ShareDataViewModel ShareData;

        public Details(int id)
        {
            InitializeComponent();
            this.ShareData = Current.ShareDatas.Single(o => o.Id == id);
            this.DataContext = this.ShareData;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "BtsClient", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (Current.User.Role.Level < _ArthurContext.Roles.Single(r => r.Name == "管理员").Level)
            {
                MessageBox.Show("当前用户权限不足！", "异常提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var id = Convert.ToInt32((sender as Button).Tag);
            Helper.ExecuteParentUserControlMethod(this, "BtsClient", "SwitchWindow", "Edit", id);
        }
    }
}
