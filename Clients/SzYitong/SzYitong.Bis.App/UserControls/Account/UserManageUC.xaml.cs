using Arthur.View.Account.User;
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

namespace SzYitong.Bis.App.UserControls
{
    /// <summary>
    /// UserManageUC.xaml 的交互逻辑
    /// </summary>
    public partial class UserManageUC : UserControl
    {

        public string Option { get; set; }

        public UserManageUC() : this("Index")
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">页面选项 Index, Create, Details ,Edit</param>
        public UserManageUC(string option)
        {
            InitializeComponent();
            SwitchWindow(option, 0);
        }

        public void SwitchWindow(string option, int id)
        {
            this.Option = option;
            Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "Arthur.View.dll");
            Type type = assembly.GetType("Arthur.View.Account.User." + this.Option);
            object page = Activator.CreateInstance(type, new object[] { id });
            grid.Children.Clear();
            this.grid.Children.Add((UIElement)page);
        }
    }
}
