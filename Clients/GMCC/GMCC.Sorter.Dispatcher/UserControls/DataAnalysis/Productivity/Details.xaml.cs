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

            //生成html文件
            FileStream fs_bak = new FileStream(path_bak, FileMode.Open);
            StreamReader sr_bak = new StreamReader(fs_bak);
            var content = sr_bak.ReadToEnd();
            sr_bak.Close();
            fs_bak.Close();

            FileStream fs = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(content);
            sw.Close();
            sw.Close();

            this.browser.Navigate(path);
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
