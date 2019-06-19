using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Other
{
    /// <summary>
    /// 托盘各种顺序
    /// </summary>
    public class TrayOrder
    {

        ///////////////////////////
        ///    绑盘顺序：
        ///     4    3    2    1
        ///     8    7    6    5
        ///    12   11   10    9
        ///    16   15   14   13
        ///    20   19   18   17
        ///    24   23   22   21
        ///    28   27   26   25
        ///    32   31   30   29
        ////////////////////////////
        ///    充放电通道顺序：
        ///    32   24   16    8
        ///    31   23   15    7
        ///    30   22   14    6
        ///    29   21   13    5
        ///    28   20   12    4
        ///    27   19   11    3
        ///    26   18   10    2
        ///    25   17    9    1
        ////////////////////////////
        ///    PLC分选结果顺序：
        ///     1    2    3    4
        ///     5    6    7    8
        ///     9   10   11   12
        ///    13   14   15   16
        ///    17   18   19   20
        ///    21   22   23   24
        ///    25   26   27   28
        ///    29   30   31   32
        ////////////////////////////
        ///    打包顺序：
        ///     1    3    2    4
        ///     5    7    6    8
        ///     9   11   10   12
        ///    13   15   14   16
        ///    17   19   18   20
        ///    21   23   22   24
        ///    25   27   26   28
        ///    29   31   30   32
        ////////////////////////////

        /// <summary>
        /// 绑盘顺序
        /// </summary>
        public int BindOrder { get; set; }

        /// <summary>
        /// 充放电顺序
        /// </summary>
        public int ChargeOrder { get; set; }

        /// <summary>
        /// 分选顺序
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 打包顺序
        /// </summary>
        public int PackOrder { get; set; }
    }
}
