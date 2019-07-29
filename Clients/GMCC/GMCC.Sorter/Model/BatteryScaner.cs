using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    /// <summary>
    /// 电池扫码枪
    /// </summary>
    public sealed class BatteryScaner : EthernetCommor, IScaner
    {
        public string ScanCommand { get; set; }
    }
}
