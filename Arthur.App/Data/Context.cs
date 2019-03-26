using Arthur.App.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Data
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

        public static DbSet<User> Users
        {
            get
            {
                return AppContext.Users;
            }
        }

        // = AppContext.Users;
        public static DbSet<Role> Roles
        {
            get
            {
                return AppContext.Roles;
            }
        }
        public static DbSet<Option> Options
        {
            get
            {
                return AppContext.Options;
            }
        }
        public static DbSet<EventLog> EventLogs
        {
            get
            {
                return AppContext.EventLogs;
            }
        }
        public static DbSet<Oplog> Oplogs
        {
            get
            {
                return AppContext.Oplogs;
            }
        }
    }
}
