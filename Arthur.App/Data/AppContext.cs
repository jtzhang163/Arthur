using System;
using System.Data.Entity;
using System.Collections.Generic;
using Arthur.Security;
using Arthur.App.Model;

namespace Arthur.App
{
    /// <summary>
    /// 上下文
    /// </summary>
    public class AppContext : DbContext
    {
        public AppContext() : base(Application.ConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("t_user");
            modelBuilder.Entity<Role>().ToTable("t_role");

            modelBuilder.Entity<EventLog>().ToTable("t_event_log");
            modelBuilder.Entity<Oplog>().ToTable("t_oplog");

            modelBuilder.Entity<Option>().ToTable("t_option");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Oplog> Oplogs { get; set; }

        public DbSet<Option> Options { get; set; }
    }

    public class AppInitializer : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            var roles = new List<Role>
            {
                new Role
                {
                    Name = "系统管理员",
                    Level = 4,
                    Users = new List<User>
                    {
                        new User
                        {
                            Name = "Administrator",
                            Password = EncryptHelper.EncodeBase64("Administrator"),
                            RegisterTime = DateTime.Now,
                            Email = string.Empty,
                            Number = string.Empty,
                            PhoneNumber = string.Empty,
                            Nickname = string.Empty,
                            Gender = Gender.Unknown,
                            IsEnabled = true,
                        }
                    }
                },

                new Role
                {
                    Name = "管理员",
                    Level = 3,
                    Users = new List<User>
                    {
                        new User
                        {
                            Name = "Admin",
                            Password =  EncryptHelper.EncodeBase64("Admin"),
                            RegisterTime = DateTime.Now,
                            Email = string.Empty,
                            Number = string.Empty,
                            PhoneNumber = string.Empty,
                            Nickname = string.Empty,
                            Gender = Gender.Unknown,
                            IsEnabled = true,
                        }
                    }
                },

                new Role
                {
                    Name = "维护员",
                    Level = 2,
                },

                new Role
                {
                    Name = "操作员",
                    Level = 1,
                },
            };
            roles.ForEach(g => context.Roles.Add(g));

            var options = new List<Option>()
            {
                new Option("AppName","XXXXXX系统","应用程序名称"),
                new Option("RememberUserId","-1","记住的用户Id"),
            };
            options.ForEach(o => context.Options.Add(o));
        }
    }
}
