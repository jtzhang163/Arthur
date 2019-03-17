using System;
using System.Data.Entity;
using System.Collections.Generic;
using Arthur.Security;
using Arthur.App.Model;

namespace Arthur.App
{

    /// <summary>
    /// 系统日志上下文
    /// </summary>
    public class LoggingContext : DbContext
    {
        public LoggingContext() : base(Application.ConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventLog>().ToTable("t_event_log");
            modelBuilder.Entity<Oplog>().ToTable("t_oplog");
        }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Oplog> Oplogs { get; set; }
    }
}
