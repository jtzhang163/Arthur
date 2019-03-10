using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    /// <summary>
    /// 基类
    /// </summary>
    public abstract class Service
    {
        [ReadOnly(true), DisplayName("ID"), Category("基本信息")]
        public int Id { get; set; }
    }
}
