using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Utils;
using GMCC.Sorter.ViewModel;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Query.Battery
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private BatteryViewModel Battery;

        public Edit(int id)
        {
            InitializeComponent();
            this.Battery = ContextToViewModel.Convert(Context.Batteries.Single(t => t.Id == id));
            this.DataContext = this.Battery;

            var trays = Context.Trays.ToList();
            this.proctray.Items.Add("——未知——");
            trays.ForEach(o => this.proctray.Items.Add(o.Code));
            var proctray_index = -1;
            if (this.Battery.ProcTrayId < 1)
            {
                proctray_index = 0;
            }
            else
            {
                var procTray = GetObject.GetById<Model.ProcTray>(this.Battery.ProcTrayId);
                proctray_index = trays.IndexOf(trays.FirstOrDefault(o => o.Id == procTray.TrayId)) + 1;
            }
            this.proctray.SelectedIndex = proctray_index;
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
            Helper.ExecuteParentUserControlMethod(this, "Battery", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {

            var trays = Context.Trays.ToList();

            var code = this.code.Text.Trim();
            var pos = this.pos.Text.Trim();
            var procTrayId = -1;
            if (this.proctray.SelectedIndex > 0)
            {
                var tray = trays[this.proctray.SelectedIndex - 1];
                var proc_tray = Context.ProcTrays.Where(o => o.TrayId == tray.Id).OrderByDescending(o => o.Id).FirstOrDefault() ?? new Model.ProcTray();
                procTrayId = proc_tray.Id;
            }

            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(pos))
            {
                 
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {
                    this.Battery.Code = code;
                    this.Battery.ProcTrayId = procTrayId;
                    this.Battery.Pos = Convert.ToInt32(pos);

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
    }
}
