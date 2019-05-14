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
    /// 托盘
    /// </summary>
    public sealed class Tray : Machine
    {
        /// <summary>
        /// 条码
        /// </summary>
        [MaxLength(50)]
        public string Code { get; set; }
    }
}
