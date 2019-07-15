using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.Core
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// DataTime转时间戳（秒）
        /// https://tool.lu/timestamp/
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dateTime)
        {
            return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}
