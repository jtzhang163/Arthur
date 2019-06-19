using GMCC.Sorter.Business;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Extensions
{
    public static class ModelExtension
    {
        public static List<Battery> GetBatteries(this ProcTray procTray)
        {
            using (var db = new Data.AppContext())
            {
                return db.Batteries.Where(o => o.ProcTrayId == procTray.Id).ToList();
            }
        }

        /// <summary>
        /// 获取通道序号
        /// </summary>
        /// <param name="battery"></param>
        /// <returns></returns>
        public static int GetChargeOrder(this Battery battery)
        {
            return OrderManage.GetChargeOrderByBindOrder(battery.Pos);
        }
    }
}
