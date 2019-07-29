using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    /// <summary>
    /// 通信器
    /// </summary>
    public abstract class Communicator : Machine
    {
        /// <summary>
        /// 通信方式
        /// </summary>
        public string CommType { get; set; }

        /// <summary>
        /// 读数据超时时间（单位：ms，0 = 无超时）
        /// </summary>
        public int ReadTimeout { get; set; }

        /// <summary>
        /// 与上位机通讯时间间隔（单位：ms）
        /// </summary>
        public int CommInterval { get; set; }
    }
}
