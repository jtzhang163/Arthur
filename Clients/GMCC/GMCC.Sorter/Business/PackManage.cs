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
using GMCC.Sorter.ViewModel;

namespace GMCC.Sorter.Business
{
    public sealed class PackManage : IManage<Pack>
    {
        public Result Create(Pack pack)
        {      
            try
            {
                var id = -1;
                using (var db = new Data.AppContext())
                {
                    var _pack = db.Packs.FirstOrDefault(o => o.Code == pack.Code);
                    if (_pack == null)
                    {
                        _pack = db.Packs.Add(new Pack()
                        {
                            Code = pack.Code,
                            Model = Current.Option.ProductModel,
                            ScanTime = DateTime.Now,
                            SortResult = pack.SortResult
                        });
                        db.SaveChanges();
                    }
                    id = _pack.Id;
                }

                Arthur.App.Business.Logging.AddOplog(string.Format("新增箱体[{0}]", pack.Code), Arthur.App.Model.OpType.创建);
    
                return Result.OkHasData(id);
            }
            catch (Exception ex)
            {
                return new Result(ex);
            }
        }
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

        public static Result Finish(SortPackViewModel sortPack)
        {
            var result = BatteryManage.SetPackFinish(sortPack.PackId);
            if (result.IsFailed)
            {
                return result;
            }

            //生成二维码
            result = QRCoderManage.Create(sortPack.PackId);
            if (result.IsSucceed)
            {
                sortPack.PackId = 0;
            }
            return result;
        }
    }
}
