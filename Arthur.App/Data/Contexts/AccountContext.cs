using System;
using System.Data.Entity;
using System.Collections.Generic;
using Arthur.Security;

namespace Arthur.App.Data
{

    /// <summary>
    /// 用户上下文
    /// </summary>
    public class AccountContext : DbContext
    {
        public AccountContext() : base(Application.ConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("t_user");
            modelBuilder.Entity<Role>().ToTable("t_role");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }

    public class AcconutInitializer : DropCreateDatabaseIfModelChanges<AccountContext>
    {
        protected override void Seed(AccountContext context)
        {
            var Roles = new List<Role>
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
                        }
                    }
                },
            };
            Roles.ForEach(g => context.Roles.Add(g));
        }
    }
}
