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
    public class TrayManage : IManage<Tray>
    {

        public Result Create(Tray tray)
        {
            try
            {
                using (var db = new Data.AppContext())
                {
                    if (db.Trays.Count(r => r.Code == tray.Code) > 0)
                    {
                        return new Result(string.Format("系统中已存在条码为{0}的托盘！", tray.Code));
                    }
                    db.Trays.Add(new Tray() { Code = tray.Code, Company = tray.Company, CreateTime = DateTime.Now, IsEnabled = true });
                    db.SaveChanges();
                }
                Arthur.Business.Logging.AddOplog(string.Format("新增托盘[{0}]", tray.Code), Arthur.App.Model.OpType.创建);
                return Result.OK;
            }
            catch (Exception ex)
            {
                return new Result(ex.Message);
            }

        }
    }
}
