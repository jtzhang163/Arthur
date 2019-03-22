﻿using Arthur.App;
using Arthur.App.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    /// <summary>
    /// 主设备
    /// </summary>
    public class BatteryScanerViewModel : CommorViewModel
    {
        public BatteryScanerViewModel(Commor commor) : base(commor)
        {

        }

        public override void Comm()
        {
            if (this.Commor.Connected)
            {
                if (this.Commor.Comm("").IsOk)
                {

                }
                this.IsAlive = true;
            }
            else
            {
                this.IsAlive = false;
            }
        }
    }
}
