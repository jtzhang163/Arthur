using Arthur;
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
        private static AppContext appContext = new AppContext();
        public static AppContext AppContext
        {
            get
            {
                lock (Arthur.App.Application.DbLocker)
                {
                    return appContext;
                }
            }
        }

        public static DbSet<Storage> Storages
        {
            get
            {
                return AppContext.Storages;
            }
        }
        public static DbSet<Tray> Trays
        {
            get
            {
                return AppContext.Trays;
            }
        }
        public static DbSet<Battery> Batteries
        {
            get
            {
                return AppContext.Batteries;
            }
        }
        public static DbSet<ProcTray> ProcTrays
        {
            get
            {
                return AppContext.ProcTrays;
            }
        }
        public static DbSet<PLC> PLCs
        {
            get
            {
                return AppContext.PLCs;
            }
        }
        public static DbSet<MES> MESs
        {
            get
            {
                return AppContext.MESs;
            }
        }

        public static DbSet<BatteryScaner> BatteryScaners
        {
            get
            {
                return AppContext.BatteryScaners;
            }
        }

        public static DbSet<TrayScaner> TrayScaners
        {
            get
            {
                return AppContext.TrayScaners;
            }
        }

        public static CurrentTask CurrentTask
        {
            get
            {
                return AppContext.CurrentTasks.FirstOrDefault();
            }
        }

        public static DbSet<TaskLog> TaskLogs
        {
            get
            {
                return AppContext.TaskLogs;
            }
        }

    }
}
