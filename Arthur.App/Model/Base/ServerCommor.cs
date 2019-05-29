using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    /// <summary>
    /// 服务器
    /// </summary>
    public abstract class ServerCommor : Communicator
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public string Host { get; set; }

    }
}
