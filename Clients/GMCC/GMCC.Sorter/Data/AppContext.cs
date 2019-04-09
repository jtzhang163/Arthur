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
    /// <summary>
    /// 应用程序上下文
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

            modelBuilder.Entity<BatteryScaner>().ToTable("t_battery_scaner");
            modelBuilder.Entity<TrayScaner>().ToTable("t_tray_scaner");

            modelBuilder.Entity<MES>().ToTable("t_mes");
            modelBuilder.Entity<PLC>().ToTable("t_plc");

            modelBuilder.Entity<CurrentTask>().ToTable("t_task");
            modelBuilder.Entity<TaskLog>().ToTable("t_task_log");
        }

        public DbSet<Tray> Trays { get; set; }
        public DbSet<Storage> Storages { get; set; }

        public DbSet<Battery> Batteries { get; set; }
        public DbSet<ProcTray> ProcTrays { get; set; }

        public DbSet<MES> MESs { get; set; }
        public DbSet<PLC> PLCs { get; set; }

        public DbSet<BatteryScaner> BatteryScaners { get; set; }
        public DbSet<TrayScaner> TrayScaners { get; set; }

        public DbSet<CurrentTask> CurrentTasks { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }

    }

    public class AppInitializer : DropCreateDatabaseIfModelChanges<AppContext>
    {
        protected override void Seed(AppContext context)
        {
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

            var mes = new MES()
            {
                Name = "MES",
                Host = "127.0.0.1",
                Company = "GMCC",
                CreateTime = DateTime.Now,
                IsEnabled = true,
                Location = "",
                SerialNumber = "",
                ModelNumber = "",
            };
            context.MESs.Add(mes);

            var plc = new PLC()
            {
                Name = "PLC",
                IP = "192.168.1.239",
                Port = 9600,
                Company = "OMRON",
                ModelNumber = "CJ2M-CP33",
                CreateTime = DateTime.Now,
                IsEnabled = true,
                Location = "",
                SerialNumber = "",
            };
            context.PLCs.Add(plc);

            var batteryScaner = new BatteryScaner()
            {
                Name = "电池扫码枪",
                IP = "192.168.1.1",
                Port = 9999,
                Company = "Datalogic",
                ModelNumber = "MATRIX 300N 482-011",
                CreateTime = DateTime.Now,
                IsEnabled = true,
                Location = "",
                SerialNumber = "",
            };
            context.BatteryScaners.Add(batteryScaner);

            var trayScaners = new List<TrayScaner>()
            {
                new TrayScaner()
                {
                    Name = "绑盘托盘扫码枪",
                    PortName = "COM1",
                    BaudRate = 9600,
                    Parity = System.IO.Ports.Parity.None,
                    DataBits = 8,
                    StopBits = System.IO.Ports.StopBits.None,
                    Company = "Honeywell",
                    ModelNumber = "3320G-2-INT",
                    CreateTime = DateTime.Now,
                    IsEnabled = true,
                    Location = "",
                    SerialNumber = "",
                },
                new TrayScaner()
                {
                    Name = "解盘托盘扫码枪",
                    PortName = "COM2",
                    BaudRate = 9600,
                    Parity = System.IO.Ports.Parity.None,
                    DataBits = 8,
                    StopBits = System.IO.Ports.StopBits.None,
                    Company = "Honeywell",
                    ModelNumber = "3320G-2-INT",
                    CreateTime = DateTime.Now,
                    IsEnabled = true,
                    Location = "",
                    SerialNumber = "",
                }
            };
            trayScaners.ForEach(o => context.TrayScaners.Add(o));

            var task = new CurrentTask()
            {
                Type = TaskType.未知,
                PreType = TaskType.未知,
                ProcTrayId = -1,
                StorageId = -1,
                StartTime = Default.DateTime,
            };
            context.CurrentTasks.Add(task);
        }
    }
}
