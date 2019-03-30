using Arthur;
using Arthur.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using GMCC.Sorter.Run;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Business
{
    public class ProcTrayManage : IManage<ProcTray>
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
                lock (Arthur.App.Application.DbLocker)
                {
                    var tray = Context.Trays.FirstOrDefault(t => t.Code == procTray.Code);
                    if (tray == null)
                    {
                        tray = Context.Trays.Add(new Tray()
                        {
                            Code = procTray.Code,
                            CreateTime = DateTime.Now,
                            Company = "SZYitong",
                            IsEnabled = true,
                            Name = procTray.Code,
                        });
                        Context.AppContext.SaveChanges();
                    }

                    var proctray = Context.ProcTrays.Add(new ProcTray()
                    {
                        TrayId = tray.Id,
                        StorageId = -1,
                        Code = procTray.Code,
                        ScanTime = DateTime.Now,
                        StartStillTime = Arthur.Default.DateTime
                    });

                    id = proctray.Id;
                    Context.AppContext.SaveChanges();
                }

                if (isScan)
                {
                    LogHelper.WriteInfo("绑盘托盘扫码：" + procTray.Code);
                }
                else
                {
                    Arthur.Business.Logging.AddOplog(string.Format("新增流程托盘[{0}]", procTray.Code), Arthur.App.Model.OpType.创建);
                }
                return Result.OkHasData(id);
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
    }
}
