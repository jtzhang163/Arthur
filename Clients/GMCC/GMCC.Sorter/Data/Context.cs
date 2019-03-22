using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Data
{
    public class Context
    {
        public static AppContext AppContext = new AppContext();

        public static DbSet<Storage> Storages = AppContext.Storages;
        public static DbSet<Tray> Trays = AppContext.Trays;

        public static DbSet<Battery> Batteries = AppContext.Batteries;
        public static DbSet<ProcTray> ProcTrays = AppContext.ProcTrays;

        public static DbSet<PLC> PLCs = AppContext.PLCs;
        public static DbSet<MES> MESs = AppContext.MESs;
    }
}
