﻿using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Model
{
    public sealed class Storage : Machine
    {
        /// <summary>
        /// 所在列
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// 所在层
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 流程托盘Id
        /// </summary>
        public int ProcTrayId { get; set; }
       
        /// <summary>
        /// 静置时长(min)
        /// </summary>
        public int StillTimeSpan { get; set; }
    }
}
