using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.Helper.ComServer
{
    public class ExistCom
    {
        public List<string> Items { get; set; } = SerialPort.GetPortNames().ToList();
    }
}
