using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arthur.Utility;

namespace Arthur.App
{
    public class AppOption
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
                    // SetProperty(ref checkTesterInfoInterval, value);
                }
            }
        }
    }
}
