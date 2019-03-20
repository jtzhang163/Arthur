using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }

    /// <summary>
    /// 托盘
    /// </summary>
    public class Tray : Machine
    {
        /// <summary>
        /// 条码
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }
    }
}
