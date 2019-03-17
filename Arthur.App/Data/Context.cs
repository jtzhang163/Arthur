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
        public static AccountContext AccountContext = new AccountContext();
        public static OptionContext OptionContext = new OptionContext();
        public static LoggingContext LoggingContext = new LoggingContext();

        public static DbSet<User> Users = AccountContext.Users;
        public static DbSet<Role> Roles = AccountContext.Roles;

        public static DbSet<Option> Options = OptionContext.Options;

        public static DbSet<EventLog> EventLogs = LoggingContext.EventLogs;
        public static DbSet<Oplog> Oplogs = LoggingContext.Oplogs;
    }
}
