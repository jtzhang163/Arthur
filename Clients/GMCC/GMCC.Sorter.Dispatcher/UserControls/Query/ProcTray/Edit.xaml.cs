using Arthur.View.Utils;
using GMCC.Sorter.Data;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Query.ProcTray
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private ProcTrayViewModel ProcTray;

        public Edit(int id)
        {
            InitializeComponent();
            this.ProcTray = ContextToViewModel.Convert(Context.ProcTrays.Single(t => t.Id == id));
            this.DataContext = this.ProcTray;

            this.storage.Items.Add("——未知——");
            Current.Storages.ForEach(o => this.storage.Items.Add(o.Name));
            var storage_index = -1;
            if (this.ProcTray.StorageId < 1)
            {
                storage_index = 0;
            }
            else
            {
                storage_index = Current.Storages.IndexOf(Current.Storages.FirstOrDefault(o => o.Id == this.ProcTray.StorageId)) + 1;
            }
            this.storage.SelectedIndex = storage_index;

            this.startstill_time.Value = this.ProcTray.StartStillTime;
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
            Helper.ExecuteParentUserControlMethod(this, "ProcTray", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var code = this.code.Text.Trim();
            var storageId = -1;
            if(this.storage.SelectedIndex > 0)
            {
                storageId = Current.Storages[this.storage.SelectedIndex - 1].Id;
            }
            var startstill_time = this.startstill_time.Value.Value;

            if (string.IsNullOrWhiteSpace(code))
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
                tip.Text = "请填写数据！";
            }
            else
            {
                try
                {
                    this.ProcTray.Code = code;
                    this.ProcTray.StorageId = storageId;
                    this.ProcTray.StartStillTime = startstill_time;

                    Context.AppContext.SaveChanges();
                    tip.Foreground = new SolidColorBrush(Colors.Green);
                    tip.Text = "修改信息成功！";

                }
                catch (Exception ex)
                {
                    tip.Foreground = new SolidColorBrush(Colors.Red);
                    tip.Text = "修改信息失败：" + ex.Message;
                }
            }
            tip.Visibility = Visibility.Visible;
        }
    }
}
