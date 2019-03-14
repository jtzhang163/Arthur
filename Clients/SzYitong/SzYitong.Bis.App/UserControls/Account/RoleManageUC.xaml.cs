using Arthur.View.Account.Role;
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

namespace SzYitong.Bis.App.UserControls
{
    /// <summary>
    /// RoleManageUC.xaml 的交互逻辑
    /// </summary>
    public partial class RoleManageUC : UserControl
    {

        public string Option { get; set; }

        public RoleManageUC() : this("Index")
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option">显示选项 Index, Create, Details ,Edit</param>
        public RoleManageUC(string option)
        {
            this.Option = option;
            InitializeComponent();

            if (this.Option == "Index")
            {
                LoadIndexUC();
            }
        }

        private void LoadIndexUC()
        {
            this.grid.Children.Add(new Index());
        }

        private void LoadDetailsUC(int id)
        {
            this.grid.Children.Add(new Details(id));
        }

        private void LoadCreateUC()
        {
            this.grid.Children.Add(new Create());
        }

        private void LoadEditUC(int id)
        {
            this.grid.Children.Add(new Edit(id));
        }

        public void SwitchWindow(string option, int id)
        {
            grid.Children.Clear();
            this.Option = option;
            if (this.Option == "Index")
            {
                LoadIndexUC();
            }
            else if (this.Option == "Details")
            {
                LoadDetailsUC(id);
            }
            else if (this.Option == "Create")
            {
                LoadCreateUC();
            }
            else if (this.Option == "Edit")
            {
                LoadEditUC(id);
            }

        }
    }
}
