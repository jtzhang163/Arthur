namespace GMCC.Sorter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GMCC_Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_battery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProcTrayId = c.Int(nullable: false),
                        SortResult = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        Pos = c.Int(nullable: false),
                        ScanTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_battery_scaner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IP = c.String(),
                        Port = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        ModelNumber = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_task",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ProcTrayId = c.Int(nullable: false),
                        StorageId = c.Int(nullable: false),
                        PreType = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_mes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Host = c.String(),
                        Name = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        ModelNumber = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_plc",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IP = c.String(),
                        Port = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        ModelNumber = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_proc_tray",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrayId = c.Int(nullable: false),
                        StorageId = c.Int(nullable: false),
                        StartStillTime = c.DateTime(nullable: false),
                        StillTimeSpan = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        Pos = c.Int(nullable: false),
                        ScanTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_storage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Column = c.Int(nullable: false),
                        Floor = c.Int(nullable: false),
                        ProcTrayId = c.Int(nullable: false),
                        StillTimeSpan = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        ModelNumber = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_task_log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskType = c.Int(nullable: false),
                        ProcTrayId = c.Int(nullable: false),
                        StorageId = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_tray",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        ModelNumber = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_tray_scaner",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PortName = c.String(),
                        BaudRate = c.Int(nullable: false),
                        DataBits = c.Int(nullable: false),
                        Parity = c.Int(nullable: false),
                        StopBits = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        Location = c.String(maxLength: 50),
                        ModelNumber = c.String(maxLength: 50),
                        SerialNumber = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.t_tray_scaner");
            DropTable("dbo.t_tray");
            DropTable("dbo.t_task_log");
            DropTable("dbo.t_storage");
            DropTable("dbo.t_proc_tray");
            DropTable("dbo.t_plc");
            DropTable("dbo.t_mes");
            DropTable("dbo.t_task");
            DropTable("dbo.t_battery_scaner");
            DropTable("dbo.t_battery");
        }
    }
}
