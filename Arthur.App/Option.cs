using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.Utility;

namespace Arthur.App
{
    public class AppOption : BindableObject
    {
        private int rememberUserId = -1;

        public int RememberUserId
        {
            get
            {
                if (rememberUserId < 0)
                {
                    rememberUserId = _Convert.StrToInt(Business.Application.GetOption("RememberUserId"), -1);
                    if (rememberUserId < -1)
                    {
                        rememberUserId = -1;
                        Business.Application.SetOption("RememberUserId", rememberUserId.ToString(), "记住的用户ID");
                    }
                }
                return rememberUserId;
            }
            set
            {
                if (rememberUserId != value)
                {
                    Business.Application.SetOption("RememberUserId", value.ToString());
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
                    appName = Business.Application.GetOption("AppName");
                    if (appName == null)
                    {
                        appName = string.Empty;
                        Business.Application.SetOption("AppName", appName, "应用程序名称");
                    }
                }
                return appName;
            }
            set
            {
                if (appName != value)
                {
                    Business.Application.SetOption("AppName", value);
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
                    companyName = Business.Application.GetOption("CompanyName");
                    if (companyName == null)
                    {
                        companyName = string.Empty;
                        Business.Application.SetOption("CompanyName", companyName, "公司名");
                    }
                }
                return companyName;
            }
            set
            {
                if (companyName != value)
                {
                    Business.Application.SetOption("CompanyName", value);
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
                    dataGridPageSize = _Convert.StrToInt(Business.Application.GetOption("DataGridPageSize"), -1);
                    if (dataGridPageSize < 0)
                    {
                        dataGridPageSize = 15;
                        Business.Application.SetOption("DataGridPageSize", dataGridPageSize.ToString(), "数据表每页的数据个数");
                    }
                }
                return dataGridPageSize;
            }
            set
            {
                if (dataGridPageSize != value)
                {
                    Business.Application.SetOption("DataGridPageSize", value.ToString());
                    dataGridPageSize = value;
                }
            }
        }
    }
}
