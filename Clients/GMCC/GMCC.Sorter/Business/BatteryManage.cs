using Arthur;
using Arthur.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public class BatteryManage : IManage<Battery>
    {

        public Result Create(Battery battery)
        {
            if (Context.Trays.Count(r => r.Code == battery.Code) > 0)
            {
                return new Result(string.Format("系统中已存在条码为{0}的电池！", battery.Code));
            }
            try
            {
                Context.Batteries.Add(new Battery() { Code = battery.Code, ScanTime = DateTime.Now});
                Context.AppContext.SaveChanges();
                Arthur.Business.Logging.AddOplog(string.Format("新增电池[{0}]", battery.Code), Arthur.App.Model.OpType.创建);
                return Result.OK;
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }

        }
    }
}
