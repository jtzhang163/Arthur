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
using GMCC.Sorter.Other;

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

        /// <summary>
        /// 获取腔体电容电阻参数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Result GetTestResult(string code)
        {
            using (var db = new GMCCContext())
            {
                var sql = string.Format("SELECT TOP 1 [ID],[UpdateTime],[Barcode],[CAP],[ESR] FROM MonomerTestResult WHERE [Barcode] = '{0}' ORDER BY [ID] DESC;", code);
                try
                {
                    var testResults = db.Database.SqlQuery<MonomerTestResult>(sql).ToList();
                    if (testResults.Count > 0)
                    {
                        return Result.OkHasData(testResults[0]);
                    }
                    else
                    {
                        return new Result("远程数据库中不包含腔体测试结果，code：" + code);
                    }
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }
    }
}
