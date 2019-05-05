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
    public class GetObject
    {
        public static T GetById<T>(int id)
        {
            var obj = new object();
            if (typeof(T).Name == "Battery")
            {
                obj = Context.Batteries.SingleOrDefault(o => o.Id == id) ?? new Battery();
            }
            else if (typeof(T).Name == "ProcTray")
            {
                if (AppCache.ProcTrays.Count(o => o.Id == id) > 0)
                {
                    obj = AppCache.ProcTrays.Single(o => o.Id == id);
                }
                else
                {
                    obj = Context.ProcTrays.SingleOrDefault(o => o.Id == id);
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
                obj = Context.Batteries.SingleOrDefault(o => o.Code == code) ?? new Battery();
            }
            else if (typeof(T).Name == "ProcTray")
            {
                if (AppCache.ProcTrays.Count(o => o.Code == code) > 0)
                {
                    obj = AppCache.ProcTrays.Where(o => o.Code == code).OrderByDescending(o => o.ScanTime).FirstOrDefault();
                }
                else
                {
                    obj = Context.ProcTrays.Where(o => o.Code == code).OrderByDescending(o => o.ScanTime).FirstOrDefault();
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
            return (T)obj;
        }

    }
}
