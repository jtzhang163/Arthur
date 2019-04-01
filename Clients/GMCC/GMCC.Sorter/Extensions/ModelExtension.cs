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
            return Context.Batteries.Where(o => o.ProcTrayId == procTray.Id).ToList();
        }
    }
}
