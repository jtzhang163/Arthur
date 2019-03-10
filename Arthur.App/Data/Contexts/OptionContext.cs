using System;
using System.Data.Entity;
using System.Collections.Generic;
using Arthur.App.Model;

namespace Arthur.App
{
    /// <summary>
    /// 用户上下文
    /// </summary>
    public class OptionContext : DbContext
    {
        public OptionContext() : base(Application.ConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Option>().ToTable("t_option");
        }
        public DbSet<Option> Options { get; set; }
    }

    public class OptionInitializer : DropCreateDatabaseIfModelChanges<OptionContext>
    {
        protected override void Seed(OptionContext context)
        {
            var options = new List<Option>()
            {
                new Option("AppName","XXXXXX系统","应用程序名称"),
                new Option("RememberUserId","-1","记住的用户Id"),
            };
            options.ForEach(o => context.Options.Add(o));
        }
    }
}
