using Arthur.Utils;
using Arthur.View.Utils;
using GMCC.Sorter.Data;
using GMCC.Sorter.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
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

namespace GMCC.Sorter.Dispatcher.UserControls.Machine.TrayScaner
{
    /// <summary>
    /// Edit.xaml 的交互逻辑
    /// </summary>
    public partial class Edit : UserControl
    {
        private TrayScanerViewModel TrayScaner = null;

        public Edit(int id)
        {
            InitializeComponent();
            this.TrayScaner = Current.TrayScaners.FirstOrDefault(o => o.Id == id);
            this.DataContext = this.TrayScaner;

            //  var list = EnumberHelper.EnumToList<Arthur.App.Model.OpType>();
            var ports = SerialPort.GetPortNames().ToList();
            ports.ForEach(o => { this.portname.Items.Add(o); });
            this.portname.SelectedIndex = ports.IndexOf(this.TrayScaner.PortName);

            var baudrates = new List<int>() { 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200 };
            baudrates.ForEach(o => { this.baudrate.Items.Add(o); });
            this.baudrate.SelectedIndex = baudrates.IndexOf(this.TrayScaner.BaudRate);

            var databits = new List<int>() { 6, 7, 8 };
            databits.ForEach(o => { this.databits.Items.Add(o); });
            this.databits.SelectedIndex = databits.IndexOf(this.TrayScaner.DataBits);

            var paritys = EnumberHelper.EnumToList<Parity>();
            paritys.ForEach(o => { this.parity.Items.Add(o.EnumName); });
            this.parity.SelectedIndex = this.parity.Items.IndexOf(this.TrayScaner.Parity.ToString());

            var stopbits = EnumberHelper.EnumToList<StopBits>();
            stopbits.ForEach(o => { this.stopbits.Items.Add(o.EnumName); });
            this.stopbits.SelectedIndex = this.stopbits.Items.IndexOf(this.TrayScaner.StopBits.ToString());
        }

        private void textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            tip.Visibility = Visibility.Hidden;
        }

        private void level_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Helper.ExecuteParentUserControlMethod(this, "TrayScaner", "SwitchWindow", "Index", 0);
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            var company = this.company.Text.Trim();
            var modelNumber = this.model_number.Text.Trim();
            var portname = Convert.ToString(this.portname.SelectedItem);
            var baudrate = Convert.ToInt32(this.baudrate.SelectedItem);
            var databits = Convert.ToInt32(this.databits.SelectedItem);
            var parity = (Parity)Enum.Parse(typeof(Parity), this.parity.SelectedItem.ToString());
            var stopbits = (StopBits)Enum.Parse(typeof(StopBits), this.stopbits.SelectedItem.ToString());

            try
            {
                this.TrayScaner.Company = company;
                this.TrayScaner.ModelNumber = modelNumber;
                this.TrayScaner.PortName = portname;
                this.TrayScaner.BaudRate = baudrate;
                this.TrayScaner.DataBits = databits;
                this.TrayScaner.Parity = parity;
                this.TrayScaner.StopBits = stopbits;

                Context.AppContext.SaveChanges();
                tip.Foreground = new SolidColorBrush(Colors.Green);
                tip.Text = "修改信息成功！";
            }
            catch (Exception ex)
            {
                tip.Foreground = new SolidColorBrush(Colors.Red);
                tip.Text = "修改信息失败：" + ex.Message;
            }

            tip.Visibility = Visibility.Visible;
        }
    }
}
