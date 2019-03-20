using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    /// <summary>
    /// 设备
    /// </summary>
    public abstract class Machine : Service
    {
        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        [MaxLength(50)]
        public string Company { get; set; }

        /// <summary>
        /// 所在位置
        /// </summary>
        [MaxLength(50)]
        public string Location { get; set; }

        /// <summary>
        /// 型号
        /// </summary>
        [MaxLength(50)]
        public string ModelNumber { get; set; }

        /// <summary>
        /// 序列号
        /// </summary>
        [MaxLength(50)]
        public string SerialNumber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
