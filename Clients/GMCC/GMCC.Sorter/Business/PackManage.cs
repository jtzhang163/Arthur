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
    public sealed class PackManage
    {
        public static Result GetCurrentPackId(SortResult sortResult)
        {
            try
            {
                Pack pack;
                using (var db = new Data.AppContext())
                {
                    pack = db.Packs.Where(o => o.SortResult == sortResult).OrderByDescending(o => o.Id).FirstOrDefault();
                }

                if (pack != null && pack.Id > 0)
                {
                    return Result.OkHasData(pack.Id);
                }
                return Result.OkHasData(0);
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }

        public static int GetPackCount(int packId)
        {
            int count = 0;
            try
            {
                using (var db = new Data.AppContext())
                {
                    count = db.Batteries.Count(o => o.PackId == packId && o.PackStatus == PackStatus.打包中);
                }
            }
            catch (Exception ex)
            {
                Running.StopRunAndShowMsg(ex);
            }

            return count;
        }
    }
}
