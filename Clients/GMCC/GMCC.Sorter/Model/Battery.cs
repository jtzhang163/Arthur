using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    public sealed class Battery : Product
    {
        /// <summary>
        /// 所在流程托盘
        /// </summary>
        public int ProcTrayId { get; set; }

        /// <summary>
        /// 所在包装
        /// </summary>
        public int PackId { get; set; }

        public decimal CAP { get; set; }

        public decimal ESR { get; set; }

        /// <summary>
        /// 分选结果
        /// </summary>
        public SortResult SortResult { get; set; } = SortResult.Unknown;

        /// <summary>
        /// 打包状态
        /// </summary>
        public PackStatus PackStatus { get; set; } = PackStatus.未知;

        /// <summary>
        /// 是否已上传
        /// </summary>
        public bool IsUploaded { get; set; }

        /// <summary>
        /// 测试时间
        /// </summary>
        public DateTime TestTime { get; set; }
    }

    /// <summary>
    /// 分选结果
    /// </summary>
    public enum SortResult
    {
        [Description("未知")]
        Unknown = 0,
        [Description("1档OK")]
        OK01 = 1,
        [Description("2档OK")]
        OK02 = 2,
        [Description("3档OK")]
        OK03 = 3,
        [Description("4档OK")]
        OK04 = 4,
        [Description("5档OK")]
        OK05 = 5,
        [Description("电压不良")]
        VNG = 6,
        [Description("电阻不良")]
        RNG = 7,
        [Description("容量不良")]
        CNG = 8,
        [Description("其他不良")]
        UNG = 9,
    }

    /// <summary>
    /// 打包状态
    /// </summary>
    public enum PackStatus
    {
        未知 = 0,
        未打包 = 1,
        打包中 = 2,
        打包完 = 3,
        不良品 = 4
    }
}
