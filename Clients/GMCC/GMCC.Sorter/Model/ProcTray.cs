using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    /// <summary>
    /// 流程托盘
    /// </summary>
    public class ProcTray : Product
    {
        /// <summary>
        /// 所在托盘
        /// </summary>
        public int TrayId { get; set; }

        /// <summary>
        /// 所在料仓
        /// </summary>
        public int StorageId { get; set; }

        /// <summary>
        /// 开始静置时间
        /// </summary>
        public DateTime StartStillTime { get; set; }

        /// <summary>
        /// 静置时长(min)
        /// </summary>
        public int StillTimeSpan { get; set; }
    }
}
