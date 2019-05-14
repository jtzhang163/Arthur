using GMCC.Sorter.Cache;
using GMCC.Sorter.Data;
using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Utils
{
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
                if (AppCache.ProcTrays.Count(o => o.Id == id) > 0)
                {
                    obj = AppCache.ProcTrays.Single(o => o.Id == id);
                }
                else
                {
                    using (var db = new Data.AppContext())
                    {
                        obj = db.ProcTrays.SingleOrDefault(o => o.Id == id);
                    }
                    if (obj == null)
                    {
                        obj = new ProcTray();
                    }
                    else
                    {
                        AppCache.ProcTrays.Add(obj as ProcTray);
                    }
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
                    obj = db.Batteries.Where(o => o.Code == code).OrderByDescending(o => o.ScanTime).FirstOrDefault() ?? new Battery();
                }
            }
            else if (typeof(T).Name == "ProcTray")
            {
                using (var db = new Data.AppContext())
                {
                    obj = db.ProcTrays.Where(o => o.Code == code).OrderByDescending(o => o.ScanTime).FirstOrDefault() ?? new ProcTray();
                }
            }
            return (T)obj;
        }
    }
}
