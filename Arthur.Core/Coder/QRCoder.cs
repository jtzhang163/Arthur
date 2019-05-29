using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;

namespace Arthur.Core.Coder
{
    public static class QRCoder
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="scale">二维码尺寸(1-10，默认5)</param>
        /// <param name="version">二维码版本(1-30，默认4)</param>
        /// <param name="logoImagepath">logo图片位置</param>
        /// <param name="logoSize">logo尺寸(默认50)</param>
        /// <returns>结果为Result.OK时，Result.Data中保存着Bitmap类型的二维码图片</returns>
        public static Result Create(string content, int scale = 5, int version = 4, string logoImagepath = "", int logoSize = 50)
        {
            QRCodeEncoder qrEncoder = new QRCodeEncoder();
            try
            {
                qrEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrEncoder.QRCodeScale = scale;
                qrEncoder.QRCodeVersion = version;
                qrEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                Bitmap qrcode = qrEncoder.Encode(content, Encoding.UTF8);
                if (!logoImagepath.Equals(string.Empty))
                {
                    Graphics g = Graphics.FromImage(qrcode);
                    Bitmap bitmapLogo = new Bitmap(logoImagepath);
                    bitmapLogo = new Bitmap(bitmapLogo, new System.Drawing.Size(logoSize, logoSize));
                    PointF point = new PointF(qrcode.Width / 2 - logoSize / 2, qrcode.Height / 2 - logoSize / 2);
                    g.DrawImage(bitmapLogo, point);
                }
                return Result.OkHasData(qrcode);
            }
            catch (IndexOutOfRangeException ex)
            {
                return new Result(-1, "超出当前二维码版本的容量上限，请选择更高的二维码版本！");
            }
            catch (Exception ex)
            {
                return new Result("生成二维码出错", ex);
            }
        }

        /// <summary>
        /// 保存二维码，并为二维码添加白色背景。
        /// </summary>
        /// <param name="path"></param>
        public static Result SaveQRCode(Bitmap bimg, string path)
        {
            try
            {
                if (bimg != null)
                {
                    Bitmap bitmap = new Bitmap(bimg.Width + 30, bimg.Height + 30);
                    Graphics g = Graphics.FromImage(bitmap);
                    g.FillRectangle(System.Drawing.Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                    g.DrawImage(bimg, new PointF(15, 15));
                    bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
                    bitmap.Dispose();

                    return Result.Success;
                }
                return new Result("生成二维码出错：传入图像为空");
            }
            catch (Exception ex)
            {
                return new Result("生成二维码出错", ex);
            }
        }
    }
}
