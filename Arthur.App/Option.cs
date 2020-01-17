using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.Core;
using Arthur.Core.Security;
using Arthur.Core.Transfer;

namespace Arthur.App
{
    public sealed class AppOption : BindableObject
    {
        private int rememberUserId = -1;

        public int RememberUserId
        {
            get
            {
                if (rememberUserId < 0)
                {
                    rememberUserId = _Convert.To(Business.Setting.GetOption("RememberUserId"), -1);
                    if (rememberUserId < -1)
                    {
                        rememberUserId = -1;
                        Business.Setting.SetOption("RememberUserId", rememberUserId.ToString(), "记住的用户ID");
                    }
                }
                return rememberUserId;
            }
            set
            {
                if (rememberUserId != value)
                {
                    Business.Setting.SetOption("RememberUserId", value.ToString());
                    rememberUserId = value;
                }
            }
        }

        private string appName = null;

        public string AppName
        {
            get
            {
                if (appName == null)
                {
                    appName = Business.Setting.GetOption("AppName");
                    if (appName == null)
                    {
                        appName = string.Empty;
                        Business.Setting.SetOption("AppName", appName, "应用程序名称");
                    }
                }
                return appName;
            }
            set
            {
                if (appName != value)
                {
                    Business.Setting.SetOption("AppName", value);
                    appName = value;
                    // SetProperty(ref checkTesterInfoInterval, value);
                }
            }
        }



        private string companyName = null;

        public string CompanyName
        {
            get
            {
                if (companyName == null)
                {
                    companyName = Business.Setting.GetOption("CompanyName");
                    if (companyName == null)
                    {
                        companyName = string.Empty;
                        Business.Setting.SetOption("CompanyName", companyName, "公司名");
                    }
                }
                return companyName;
            }
            set
            {
                if (companyName != value)
                {
                    Business.Setting.SetOption("CompanyName", value);
                    companyName = value;
                }
            }
        }


        private int dataGridPageSize = -1;

        public int DataGridPageSize
        {
            get
            {
                if (dataGridPageSize < 0)
                {
                    dataGridPageSize = _Convert.To(Business.Setting.GetOption("DataGridPageSize"), -1);
                    if (dataGridPageSize < 0)
                    {
                        dataGridPageSize = 100;
                        Business.Setting.SetOption("DataGridPageSize", dataGridPageSize.ToString(), "数据表每页的数据个数");
                    }
                }
                return dataGridPageSize;
            }
            set
            {
                if (dataGridPageSize != value)
                {
                    Business.Setting.SetOption("DataGridPageSize", value.ToString());
                    dataGridPageSize = value;
                }
            }
        }


        private string remainingMinutesStr = null;

        public int RemainingMinutes
        {
            get
            {
                if (remainingMinutesStr == null)
                {
                    remainingMinutesStr = Business.Setting.GetOption("26A610E04F74BC81");
                    if (remainingMinutesStr == null)
                    {
                        var tmpStr = EncryptBase64Helper.EncodeBase64(EncryptBase64Helper.EncodeBase64("2160")) + "QXJ0aHVy";//2160 Arthur
                        Business.Setting.SetOption("26A610E04F74BC81", tmpStr, "");
                        remainingMinutesStr = tmpStr;
                    }
                }
                var remainingMinutes = _Convert.To<int>(EncryptBase64Helper.DecodeBase64(EncryptBase64Helper.DecodeBase64(remainingMinutesStr.Replace("QXJ0aHVy", ""))), 0);
                return remainingMinutes;
            }
            set
            {
                var tmpStr = EncryptBase64Helper.EncodeBase64(EncryptBase64Helper.EncodeBase64(value.ToString())) + "QXJ0aHVy";
                if (remainingMinutesStr != tmpStr)
                {
                    Business.Setting.SetOption("26A610E04F74BC81", tmpStr);
                    remainingMinutesStr = tmpStr;
                }
            }
        }
    }
}
