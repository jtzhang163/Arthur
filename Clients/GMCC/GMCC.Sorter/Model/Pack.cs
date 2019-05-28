using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    /// <summary>
    /// 包装
    /// 分选后产品的包装
    /// </summary>
    public sealed class Pack : Product
    {
        /// <summary>
        /// 对应分选结果
        /// </summary>
        public SortResult SortResult { get; set; } = SortResult.Unknown;
    }
}
