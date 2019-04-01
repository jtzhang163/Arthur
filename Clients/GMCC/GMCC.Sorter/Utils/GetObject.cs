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
                obj = Context.ProcTrays.SingleOrDefault(o => o.Id == id) ?? new ProcTray();
            }
            return (T)obj;
        }   
    }
}
