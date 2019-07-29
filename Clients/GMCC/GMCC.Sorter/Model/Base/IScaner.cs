using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    /// <summary>
    /// 扫码枪接口
    /// </summary>
    interface IScaner
    {
        /// <summary>
        /// 扫码指令
        /// </summary>
        string ScanCommand { get; set; }
    }
}
