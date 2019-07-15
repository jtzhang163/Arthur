using Microsoft.Win32;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace Arthur.App.View.Tool.QRCoder
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : UserControl
    {

        private Bitmap bimg = null; //保存生成的二维码，方便后面保存
        private string logoImagePath = string.Empty; //存储Logo的路径

        public Details(string content, string logoPath)
        {
            InitializeComponent();
            this.content.Text = content;
            if (!string.IsNullOrEmpty(logoPath))
            {
                SetLogoImage(logoPath);
            }
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

        private void btnSelectLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "图片文件|*.jpg;*.png;*.gif|All files(*.*)|*.*";
            if (openDialog.ShowDialog() == true)
            {
                SetLogoImage(openDialog.FileName);
            }
        }

        private void SetLogoImage(string logoPath)
        {
            logoImagePath = logoPath;
            Bitmap bImg = new Bitmap(logoPath);
            imgLogo.Source = new BitmapImage(new Uri(logoPath));
            ResetImageStrethch(imgLogo, bImg);
        }

        /// <summary>
        /// 重置Image的Strethch属性
        /// 当图片小于size时显示原始大小
        /// 当图片大于size时，缩小图片比例，让图片全部显示出来
        /// </summary>
        /// <param name="img"></param>
        /// <param name="size"></param>
        private void ResetImageStrethch(System.Windows.Controls.Image img, Bitmap bImg)
        {
            if (bImg.Width <= img.Width)
            {
                img.Stretch = Stretch.None;
            }
            else
            {
                img.Stretch = Stretch.Fill;
            }
        }

        private void btnCreateQRCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowQRCode();
            }
            catch (Exception ex)
            {
                tip.Background = new SolidColorBrush(Colors.Red);
                tip.Text = "生成二维码出错：" + ex.Message;
                tip.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 显示生成的二维码
        /// </summary>
        public void ShowQRCode()
        {

            if (content.Text.Trim().Length <= 0)
            {
                tip.Background = new SolidColorBrush(Colors.Red);
                tip.Text = "二维码内容不能为空，请输入内容！";
                tip.Visibility = Visibility.Visible;
                return;
            }

            var ret = Arthur.Core.Coder.QRCoder.Create(content.Text, Convert.ToInt32(scale.Text), Convert.ToInt32(version.Text), logoImagePath, Convert.ToInt32(logo_scale.Text));
            if (ret.IsSucceed)
            {
                bimg = (Bitmap)ret.Data;
                imgQRcode.Source = BitmapToBitmapImage(bimg);
                ResetImageStrethch(imgQRcode, bimg);
            }
            else
            {
                tip.Background = new SolidColorBrush(Colors.Red);
                tip.Text = ret.Msg;
                tip.Visibility = Visibility.Visible;
                return;
            }

        }

        /// <summary>
        /// 将Bitmap转换成BitmapImage,使其能够在Image控件中显示
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            BitmapImage bImage = new BitmapImage();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            bImage.BeginInit();
            bImage.StreamSource = new MemoryStream(ms.ToArray());
            bImage.EndInit();
            return bImage;
        }

        private void btnSaveQRCode_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Png文件(*.Png)|*.png|All files(*.*)|*.*";
            saveDialog.FileName = "QRCODE" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    Arthur.Core.Coder.QRCoder.SaveQRCode(bimg, saveDialog.FileName);
                    tip.Background = new SolidColorBrush(Colors.Green);
                    tip.Text = "保存成功";
                    tip.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    tip.Background = new SolidColorBrush(Colors.Red);
                    tip.Text = "保存二维码出错！";
                    tip.Visibility = Visibility.Visible;
                    return;
                }
            }
        }
    }
}
