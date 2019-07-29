using Arthur.Core.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    public static class Application
    {
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultDatabase"].ToString();

        /// <summary>
        /// 软件是否已运行，防止重复打开软件
        /// </summary>
        public static bool ThisAppIsAlreadyRunning()
        {
            string proName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name);
            Process[] pro = Process.GetProcesses();
            int n = pro.Where(p => p.ProcessName.Equals(proName)).Count();
            return n > 1 ? true : false;
        }


        public static object DbLocker = new object();
    }
}
