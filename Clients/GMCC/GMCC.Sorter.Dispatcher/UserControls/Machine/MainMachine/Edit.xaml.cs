using Arthur.Utility;
using Arthur.View.Utils;
using GMCC.Sorter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Machine.MainMachine
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        public Edit(int id)
        {
            InitializeComponent();
            this.DataContext = Current.MainMachine;
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Collapsed;
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "MainMachine", "SwitchWindow", "Details", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var company = this.company.Text.Trim();
            var modelNumber = this.model_number.Text.Trim();
            var ip = this.ip.Text.Trim();
            var port = _Convert.StrToInt(this.port.Text.Trim(), -1);
            var comm_interval = this.comm_interval.Text.Trim();
            var bind_traycode = this.bind_traycode.Text.Trim();
            var jaw_traycode = this.jaw_traycode.Text.Trim();
            var unbind_traycode = this.unbind_traycode.Text.Trim();
            var still_timespan = this.still_timespan.Text.Trim();

            if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(this.port.Text))
            {
                 
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {

                    Current.MainMachine.Company = company;
                    Current.MainMachine.ModelNumber = modelNumber;
                    Current.MainMachine.IP = ip;
                    Current.MainMachine.Port = port;
                    Current.MainMachine.CommInterval = Convert.ToInt32(comm_interval);
                    Current.MainMachine.BindProcTrayId = GetProcTrayId(bind_traycode);
                    Current.MainMachine.JawProcTrayId = GetProcTrayId(jaw_traycode);
                    Current.MainMachine.UnbindProcTrayId = GetProcTrayId(unbind_traycode);
                    Current.MainMachine.StillTimeSpan = Convert.ToInt32(still_timespan);

                    Context.AppContext.SaveChanges();
                    tip.Background = new SolidColorBrush(Colors.Green);
                    tip.Text = "修改信息成功！";
                }
                catch (Exception ex)
                {
                     
                    tip.Text = "修改信息失败：" + ex.Message;
                }
            }
            tip.Visibility = Visibility.Visible;
        }

        private int GetProcTrayId(string code)
        {
            var id = -1;
            if (string.IsNullOrEmpty(code))
            {
                id = 0;
            }
            else if (Context.ProcTrays.Count(o => o.Code == code) == 0)
            {
                throw new Exception(string.Format("系统中不存在条码为[{0}]的托盘", code));
            }
            else
            {
                id = Context.ProcTrays.OrderByDescending(o => o.Id).FirstOrDefault(o => o.Code == code).Id;
            }
            return id;
        }
    }
}
