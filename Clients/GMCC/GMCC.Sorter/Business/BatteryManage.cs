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
using Arthur.App.Model;

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
                        SortResult = SortResult.Unknown,
                        Model = Current.Option.ProductModel,
                        IsUploaded = false,
                        PackId = -1,
                        PackStatus = PackStatus.未打包,
                        CAP = 0,
                        ESR = 0,
                        TestTime = DateTime.Now
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
                return Result.Success;
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public static Result Upload(Battery battery)
        {
            Result result;
            using (var db = new GMCCContext())
            {
                var sql = string.Format("INSERT INTO AutoSorting (CaseNumber, BarCode, CAP, ESR, TestDate, UserID, UserName) VALUES ('{0}', '{1}', {2}, {3}, '{4}', {5}, '{6}')",
                   GetObject.GetById<Pack>(battery.PackId).Code, battery.Code, battery.CAP, battery.ESR, battery.TestTime, Current.User.Id, Current.User.Name);
                try
                {
                    if (db.Database.ExecuteSqlCommand(sql) > 0)
                    {
                        result = Result.Success;
                    }
                    else
                    {
                        return new Result("");
                    }
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }

            using (var db = new Data.AppContext())
            {
                try
                {
                    battery = db.Batteries.Where(o => o.Id == battery.Id).FirstOrDefault();
                    battery.IsUploaded = true;
                    db.SaveChanges();
                    return Result.Success;
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }

        /// <summary>
        /// 不良品电池从包中移除
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Result NgBatteryOutFromPack(string code)
        {
            using (var db = new Data.AppContext())
            {
                try
                {
                    var battery = db.Batteries.Where(o => o.Code == code).OrderByDescending(o => o.Id).FirstOrDefault();
                    if (battery == null)
                    {
                        return new Result("系统中不存在电池：" + code);
                    }
                    if (battery.PackStatus == PackStatus.不良品)
                    {
                        return new Result("该电池已经被移除");
                    }
                    battery.PackStatus = PackStatus.不良品;
                    battery.PackId = 0;
                    if (db.SaveChanges() > 0)
                    {
                        Arthur.App.Business.Logging.AddOplog("不良品电池从包中移除" + code,  OpType.编辑);
                    }
                    return Result.Success;
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }

        public static Result GetBattery(int procTrayId, int pos)
        {
            using (var db = new Data.AppContext())
            {
                try
                {
                    var battery = db.Batteries.FirstOrDefault(o => o.ProcTrayId == procTrayId && o.Pos == pos);
                    if (battery == null)
                    {
                        return new Result("系统中不存在电池");
                    }
                    return Result.SuccessHasData(battery);
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }

        public static Result GetFillBatteryCount(int packId)
        {
            using (var db = new Data.AppContext())
            {
                try
                {
                    var count = db.Batteries.Count(o => o.PackId == packId);
                    return Result.SuccessHasData(count);
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }

        public static Result SetPacking(int id, int packId)
        {
            using (var db = new Data.AppContext())
            {
                try
                {
                    var battery = db.Batteries.FirstOrDefault(o => o.Id == id);
                    battery.PackId = packId;
                    battery.PackStatus = PackStatus.打包中;
                    db.SaveChanges();
                    return Result.Success;
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }

        public static Result SetPackFinish(int packId)
        {
            using (var db = new Data.AppContext())
            {
                try
                {
                    db.Batteries.Where(o => o.PackId == packId).ToList().ForEach(o => o.PackStatus = PackStatus.打包完);
                    db.SaveChanges();
                    return Result.Success;
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }

        public static Result GetFirstBatteryNotUpload(out Battery battery)
        {
            battery = new Battery();
            using (var db = new Data.AppContext())
            {
                try
                {
                    battery = db.Batteries.Where(o => !o.IsUploaded && o.PackStatus == PackStatus.打包完 && o.PackId > 0).FirstOrDefault();
                    if (battery == null)
                    {
                        return new Result(1, "全部上传完");
                    }
                    return Result.SuccessHasData(battery);
                }
                catch (Exception ex)
                {
                    return new Result(ex);
                }
            }
        }
    }
}
