using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur
{
    public static class Default
    {
        /// <summary>
        /// 默认时间（2000-01-01 00:00:00）
        /// </summary>
        public static DateTime DateTime { get; set; } = DateTime.Parse("2000-01-01 00:00:00");
    }
}
