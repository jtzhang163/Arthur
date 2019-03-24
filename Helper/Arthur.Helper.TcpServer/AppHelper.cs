using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.Helper.TcpServer
{
    public class AppHelper
    {
        public static void SetConfig(string key, string value)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings[key].Value = value;
            cfa.Save();
        }
    }
}
