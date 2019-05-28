using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Other
{
    /// <summary>
    /// 单体测试结果
    /// 数据由BTS客户端生成
    /// </summary>
    public sealed class MonomerTestResult
    {
        public int ID { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Barcode { get; set; }
        
        /// <summary>
        /// 容量
        /// </summary>
        public decimal CAP { get; set; }

        /// <summary>
        /// 内阻
        /// </summary>
        public decimal ESR { get; set; }
    }

    /// <summary>
    /// 自动分选并打包结果，数据由调度系统生成
    /// </summary>
    public sealed class AutoSorting
    {
        public int ID { get; set; }

        /// <summary>
        /// 箱体号
        /// </summary>
        public string CaseNumber { get; set; }

        public string BarCode { get; set; }

        /// <summary>
        /// 容量
        /// </summary>
        public decimal CAP { get; set; }

        /// <summary>
        /// 内阻
        /// </summary>
        public decimal ESR { get; set; }
        public DateTime TestDate { get; set; }

        /// <summary>
        /// （预留，用于记录操作者，最好能设定成操作者）
        /// </summary>
        public int UserID { get; set; }

        public string UserName { get; set; }
    }

}
