using Arthur.View.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Arthur.View.Account.Role
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : UserControl
    {
        public Index()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var item = new ListBoxItem();

            var label = new Label();
            label.Content = "aaaaa";

            item.Content = label;
            listView.Items.Add(item);
        }

        private void create_Click(object sender, RoutedEventArgs e)
        {

            UserControl uc = ControlsSearchHelper.GetParentObject<UserControl>(this, "RoleManage");
            Type type = uc.GetType();
            MethodInfo mi = type.GetMethod("SwitchWindow");
            mi.Invoke(uc, new object[] { "Create" });
        }
    }
}
