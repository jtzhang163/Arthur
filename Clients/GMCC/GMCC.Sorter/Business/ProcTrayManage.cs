using Arthur.Core;
using Arthur.App.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public sealed class ProcTrayManage : IManage<ProcTray>
    {
        public Result Create(ProcTray procTray)
        {
            return Create(procTray, false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procTray"></param>
        /// <param name="isScan">扫码获得，不是手动添加的</param>
        /// <returns></returns>
        public Result Create(ProcTray procTray, bool isScan)
        {

            try
            {
                var id = -1;

                using (var db = new Data.AppContext())
                {
                    var tray = db.Trays.FirstOrDefault(t => t.Code == procTray.Code);
                    if (tray == null)
                    {
                        tray = db.Trays.Add(new Tray()
                        {
                            Code = procTray.Code,
                            CreateTime = DateTime.Now,
                            Company = "SZYitong",
                            IsEnabled = true,
                            Name = procTray.Code,
                        });
                    }

                    var proctray = db.ProcTrays.Add(new ProcTray()
                    {
                        TrayId = tray.Id,
                        StorageId = -1,
                        Code = procTray.Code,
                        ScanTime = DateTime.Now,
                        StartStillTime = DateTime.Now,//Arthur.Default.DateTime
                        Model = Current.Option.ProductModel
                    });

                    db.SaveChanges();

                    id = proctray.Id;
                }

                if (isScan)
                {
                    LogHelper.WriteInfo("绑盘托盘扫码：" + procTray.Code);
                }
                else
                {
                    Arthur.App.Business.Logging.AddOplog(string.Format("新增流程托盘[{0}]", procTray.Code), Arthur.App.Model.OpType.创建);
                }
                return Result.SuccessHasData(id);
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        //public List<Battery> GetBatteries(int procTrayId)
        //{
        //    return Context.Batteries.Where(o => o.ProcTrayId == procTrayId).AsNoTracking().ToList();
        //}

        public static int GetBatteryCount(int procTrayId)
        {
            using (var db = new Data.AppContext())
            {
                return db.Batteries.Count(o => o.ProcTrayId == procTrayId);
            }
        }

        //public static ProcTray CreateById(int id)
        //{
        //    return Context.ProcTrays.SingleOrDefault(o => o.Id == id) ?? new ProcTray();
        //}

        /// <summary>
        /// 清除所有该流程托盘的信息
        /// </summary>
        public static void ClearProcTray(int id)
        {
            if (Current.Option.Tray11_Id == id) Current.Option.Tray11_Id = 0;
            if (Current.Option.Tray12_Id == id) Current.Option.Tray12_Id = 0;
            if (Current.Option.Tray13_Id == id) Current.Option.Tray13_Id = 0;
            if (Current.Option.Tray21_Id == id) Current.Option.Tray21_Id = 0;
            if (Current.Option.Tray22_Id == id) Current.Option.Tray22_Id = 0;
            if (Current.Option.Tray23_Id == id) Current.Option.Tray23_Id = 0;
            if (Current.Option.Tray11_PreId == id) Current.Option.Tray11_PreId = 0;
            if (Current.Option.Tray12_PreId == id) Current.Option.Tray12_PreId = 0;
            if (Current.Option.Tray21_PreId == id) Current.Option.Tray21_PreId = 0;
            if (Current.Option.Tray22_PreId == id) Current.Option.Tray22_PreId = 0;
            if (Current.Option.Tray23_PreId == id) Current.Option.Tray23_PreId = 0;
        }
    }
}
