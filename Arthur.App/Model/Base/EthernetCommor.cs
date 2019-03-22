using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    /// <summary>
    /// 以太网通信设备
    /// </summary>
    public abstract class EthernetCommor : Communicator
    {
        public string IP { get; set; }

        public int Port { get; set; }
    }
}
