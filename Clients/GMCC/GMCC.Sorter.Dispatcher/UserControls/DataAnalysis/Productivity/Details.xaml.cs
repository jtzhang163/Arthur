using Arthur.View.Utils;
using System;
using System.IO;
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
using GMCC.Sorter.ViewModel;
using GMCC.Sorter.Model;
using System.Threading;

namespace GMCC.Sorter.Dispatcher.UserControls.DataAnalysis.Productivity
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {

        private string path_bak = AppDomain.CurrentDomain.BaseDirectory + "Htmls\\mix-line-bar.bak.html";
        private string path = AppDomain.CurrentDomain.BaseDirectory + "Htmls\\mix-line-bar.html";

        public Details(int id)
        {
            InitializeComponent();

            this.start_time.Value = DateTime.Now.AddDays(-9);
            this.end_time.Value = DateTime.Now;

            UpdateChart();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            UpdateChart();
        }

        private void UpdateChart()
        {

            var startDate = this.start_time.Value.Value;
            var endDate = this.end_time.Value.Value.AddDays(1);

            if (endDate < startDate)
            {
                MessageBox.Show("开始时间不能大于截止时间");
                return;
            }

            if (endDate > DateTime.Now.AddDays(1))
            {
                MessageBox.Show("截止时间不能大于当前时间");
                return;
            }

            if ((int)((endDate - startDate).TotalDays) > 10)
            {
                MessageBox.Show("时间范围过大，请控制在10天之内");
                return;
            }

            Thread t = new Thread(() =>
            {

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.tip.Text = "图表加载中，请稍后...";
                    this.tip.Visibility = Visibility.Visible;
                }));

                Thread.Sleep(3000);

                var productivities = new List<ProductivityViewModel>();

                var date = startDate;

                while (date < endDate)
                {
                    productivities.Add(new ProductivityViewModel()
                    {
                        Date = date.ToString("yyyy-MM-dd"),
                        ProductivityAll = 0,
                        ProductivityGood = 0
                    });
                    date = date.AddDays(1);
                }

                using (var db = new GMCC.Sorter.Data.AppContext())
                {
                    var sql1 = string.Format("SELECT CONVERT (VARCHAR(10), ScanTime, 120) AS [Date], COUNT (1) AS [ProductivityAll] FROM t_battery WHERE ScanTime BETWEEN '{0}' AND '{1}' GROUP BY CONVERT (VARCHAR(10), ScanTime, 120)", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                    var data1 = db.Database.SqlQuery<ProductivityViewModel>(sql1).ToList();
                    data1.ForEach(o => { productivities.First(p => p.Date == o.Date).ProductivityAll = o.ProductivityAll; });

                    var sql2 = string.Format("SELECT CONVERT (VARCHAR(10), ScanTime, 120) AS [Date], COUNT (1) AS [ProductivityGood] FROM t_battery WHERE SortResult < 6 AND  ScanTime BETWEEN '{0}' AND '{1}' GROUP BY CONVERT (VARCHAR(10), ScanTime, 120)", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
                    var data2 = db.Database.SqlQuery<ProductivityViewModel>(sql2).ToList();
                    data2.ForEach(o => { productivities.First(p => p.Date == o.Date).ProductivityGood = o.ProductivityGood; });

                }


                //获取html初始文件
                FileStream fs_bak = new FileStream(path_bak, FileMode.Open);
                StreamReader sr_bak = new StreamReader(fs_bak);
                var content = sr_bak.ReadToEnd();
                sr_bak.Close();
                fs_bak.Close();

                //x轴坐标值
                content = content.Replace("'{xAxis-data}'", string.Join(",", productivities.ConvertAll<string>(o => string.Format("'{0}'", o.Date))));
                //产量
                content = content.Replace("'{productivity-all}'", string.Join(",", productivities.ConvertAll<string>(o => o.ProductivityAll.ToString())));
                //合格
                content = content.Replace("'{productivity-good}'", string.Join(",", productivities.ConvertAll<string>(o => o.ProductivityGood.ToString())));
                //合格率
                content = content.Replace("'{productivity-ratio}'", string.Join(",", productivities.ConvertAll<string>(o => o.QualifiedRate.ToString())));

                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                sw.Write(content);
                sw.Close();
                sw.Close();

                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.browser.Navigate(path);
                    this.tip.Text = "";
                    this.tip.Visibility = Visibility.Collapsed;
                }));

            });
            t.Start();

        }


        private void Browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            SetWebBrowserSilent(sender as WebBrowser, true);
        }

        /// <summary>
        /// 设置浏览器静默，不弹错误提示框
        /// </summary>
        /// <param name="webBrowser">要设置的WebBrowser控件浏览器</param>
        /// <param name="silent">是否静默</param>
        private void SetWebBrowserSilent(WebBrowser webBrowser, bool silent)
        {
            FieldInfo fi = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fi != null)
            {
                object browser = fi.GetValue(webBrowser);
                if (browser != null)
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { silent });
            }
        }

    }
}
