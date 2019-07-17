using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter
{
    public static class Common
    {

        private static string proj_no = string.Empty;

        /// <summary>
        /// 项目编号 
        /// 0079 / 0081
        /// 0079 为 60A
        /// 0081 为 120A
        /// </summary>
        public static string PROJ_NO
        {
            get
            {
                if (string.IsNullOrEmpty(proj_no))
                {
                    proj_no = ConfigurationManager.AppSettings["PROJ_NO"];
                }
                return proj_no;
            }
        }

        /// <summary>
        /// 料仓列数
        /// 0079: 18
        /// 0081: 30
        /// </summary>
        public static int STOR_COL_COUNT = PROJ_NO == "0079" ? 18 : 30;

        /// <summary>
        /// 料仓层数
        /// 0079: 5
        /// 0081: 3
        /// </summary>
        public static int STOR_FLOOR_COUNT = PROJ_NO == "0079" ? 5 : 3;

        /// <summary>
        /// 托盘满盘电池数
        /// </summary>
        public static int TRAY_BATTERY_COUNT = 32;
    }
}
