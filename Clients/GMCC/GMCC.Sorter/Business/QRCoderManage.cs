using Arthur.Core;
using GMCC.Sorter.Model;
using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public class QRCoderManage
    {

        public static Result Create(int packId)
        {
            var pack = GetObject.GetById<Pack>(packId);
            if (string.IsNullOrEmpty(pack.Code))
            {
                return new Result("系统中不包含箱体，ID:" + packId);
            }
            var content = Current.Option.PackDetailWebSiteUrl.Replace("[pack_code]", pack.Code);

            var result = Arthur.Core.Coder.QRCoder.Create(content, 5, 4, AppDomain.CurrentDomain.BaseDirectory + "Images\\gmcc_logo.png", 30);
            if (result.IsFailed)
            {
                return result;
            }
            var bimg = (Bitmap)result.Data;

            var dirPath = string.Format("D:\\GMCC.Sorter\\QRCODE_FILES\\{0}\\{1}", pack.SortResult, DateTime.Now.ToString("yyyyMM"));

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            return Arthur.Core.Coder.QRCoder.SaveQRCode(bimg, dirPath + string.Format("\\{0}.png", pack.Code));
        }
    }
}
