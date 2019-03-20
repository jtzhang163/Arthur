using GMCC.Sorter.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMCC.Sorter.Data
{
    /// <summary>
    /// 设备上下文
    /// </summary>
    public class AppContext : DbContext
    {
        public AppContext() : base(Arthur.App.Application.ConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tray>().ToTable("t_tray");
            modelBuilder.Entity<Storage>().ToTable("t_storage");

            modelBuilder.Entity<Battery>().ToTable("t_battery");
            modelBuilder.Entity<ProcTray>().ToTable("t_proc_tray");
        }

        public DbSet<Tray> Trays { get; set; }
        public DbSet<Storage> Storages { get; set; }

        public DbSet<Battery> Batteries { get; set; }
        public DbSet<ProcTray> ProcTrays { get; set; }
    }

    public class AppInitializer : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
            var trays = new List<Tray>();
            for (var i = 0; i < 100; i++)
            {
                var code = string.Format("TRAY{0:D4}", i + 1);
                trays.Add(new Tray()
                {
                    Code = code,
                    Name = "",
                    Company = "SZYitong",
                    CreateTime = DateTime.Now,
                    Location = "",
                    ModelNumber = "",
                    SerialNumber = "",
                    IsEnabled = true
                });
            }
            trays.ForEach(o => context.Trays.Add(o));

            var storages = new List<Storage>();
            for (var i = 0; i < Common.STOR_COL_COUNT; i++)
            {
                for (int j = 0; j < Common.STOR_FLOOR_COUNT; j++)
                {
                    var name = string.Format("料仓{0:D2}-{1}", i + 1, j + 1);
                    storages.Add(new Storage()
                    {
                        Name = name,
                        Company = "SZYitong",
                        CreateTime = DateTime.Now,
                        Location = "",
                        ModelNumber = "",
                        SerialNumber = "",
                        IsEnabled = true,
                        Column = i + 1,
                        Floor = j + 1
                    });
                }
            }
            storages.ForEach(o => context.Storages.Add(o));
        }
    }
}
