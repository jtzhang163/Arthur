using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using GMCC.Sorter.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
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
    public sealed class MesViewModel : ServerCommorViewModel
    {

        public MesViewModel(Commor commor) : base(commor)
        {

        }

        public void Comm()
        {
            lock (this)
            {
                if (BatteryManage.GetFirstBatteryNotUpload(out Battery battery).IsOk)
                {
                    this.CommorInfo = "获取测试数据" + battery.Code;
                    var result = BatteryManage.GetAndSaveTestResult(battery);
                    if (result.IsFailed)
                    {
                        return;
                    }
                    this.CommorInfo = "开始上传：" + battery.Code;

                    result = BatteryManage.GetAndSaveTestResult(battery);
                    if (result.IsFailed)
                    {
                        return;
                    }
                    this.CommorInfo = "上传完成：" + battery.Code;
                }
            }
            this.IsAlive = true;
        }
    }
}
