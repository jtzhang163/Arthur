using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Utils
{
    /// <summary>
    /// 获取对象，如果不存在返回一个默认对象
    /// </summary>
    public static class GetObject
    {
        public static T GetById<T>(int id)
        {
            var obj = new object();
            if (typeof(T).Name == "Battery")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.Batteries.SingleOrDefault(o => o.Id == id) ?? new Battery();
                }
            }
            else if (typeof(T).Name == "ProcTray")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.ProcTrays.SingleOrDefault(o => o.Id == id) ?? new ProcTray();
                }
            }
            else if (typeof(T).Name == "Pack")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.Packs.SingleOrDefault(o => o.Id == id) ?? new Pack();
                }
            }
            else if (typeof(T).Name == "StorageViewModel")
            {
                obj = Current.Storages.SingleOrDefault(o => o.Id == id);
            }
            return (T)obj;
        }


        public static T GetByCode<T>(string code)
        {
            var obj = new object();
            if (typeof(T).Name == "Battery")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.Batteries.Where(o => o.Code == code).OrderByDescending(o => o.Id).FirstOrDefault() ?? new Battery();
                }
            }
            else if (typeof(T).Name == "ProcTray")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.ProcTrays.Where(o => o.Code == code).OrderByDescending(o => o.Id).FirstOrDefault() ?? new ProcTray();
                }
            }
            else if (typeof(T).Name == "Pack")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.Packs.Where(o => o.Code == code).OrderByDescending(o => o.Id).FirstOrDefault() ?? new Pack();
                }
            }
            return (T)obj;
        }
    }
}
