using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Other
{
    /// <summary>
    /// 绑盘序号和通道序号关系
    /// </summary>
    public class BindChargeOrder
    {
        /// <summary>
        /// 绑盘顺序
        /// </summary>
        public int BindOrder { get; set; }

        /// <summary>
        /// 充放电顺序
        /// </summary>
        public int ChargeOrder { get; set; }
    }
}
