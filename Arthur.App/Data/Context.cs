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
        public static AppContext AppContext = new AppContext();

        public static DbSet<User> Users = AppContext.Users;
        public static DbSet<Role> Roles = AppContext.Roles;

        public static DbSet<Option> Options = AppContext.Options;

        public static DbSet<EventLog> EventLogs = AppContext.EventLogs;
        public static DbSet<Oplog> Oplogs = AppContext.Oplogs;
    }
}
