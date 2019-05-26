using Arthur.Core;
using Arthur.App.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Extensions;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using GMCC.Sorter.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public sealed class BatteryManage : IManage<Battery>
    {
        public Result Create(Battery battery)
        {
            return Create(battery, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="battery"></param>
        /// <param name="isScan">扫码获得，不是手动添加的</param>
        /// <returns></returns>
        public Result Create(Battery battery, bool isScan)
        {
            try
            {
                //if (Context.Batteries.Count(r => r.Code == battery.Code) > 0)
                //{
                //    return new Result(string.Format("系统中已存在条码为{0}的电池！", battery.Code));
                //}
                using (var db = new Data.AppContext())
                {
                    db.Batteries.Add(new Battery()
                    {
                        Code = battery.Code,
                        ScanTime = DateTime.Now,
                        ProcTrayId = Current.Option.Tray11_Id,
                        Pos = ProcTrayManage.GetBatteryCount(Current.Option.Tray11_Id) + 1,
                        SortResult = SortResult.Unknown
                    });
                    db.SaveChanges();
                }
                
                if (isScan)
                {
                    LogHelper.WriteInfo("电池扫码：" + battery.Code);
                }
                else
                {
                    Arthur.App.Business.Logging.AddOplog(string.Format("新增电池[{0}]", battery.Code), Arthur.App.Model.OpType.创建);
                }
                return Result.OK;
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}
