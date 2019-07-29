namespace GMCC.Sorter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GMCC_CommOptimize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_battery_scaner", "ScanCommand", c => c.String());
            AddColumn("dbo.t_battery_scaner", "CommType", c => c.String());
            AddColumn("dbo.t_battery_scaner", "ReadTimeout", c => c.Int(nullable: false));
            AddColumn("dbo.t_battery_scaner", "CommInterval", c => c.Int(nullable: false));
            AddColumn("dbo.t_mes", "CommType", c => c.String());
            AddColumn("dbo.t_mes", "ReadTimeout", c => c.Int(nullable: false));
            AddColumn("dbo.t_mes", "CommInterval", c => c.Int(nullable: false));
            AddColumn("dbo.t_plc", "CommType", c => c.String());
            AddColumn("dbo.t_plc", "ReadTimeout", c => c.Int(nullable: false));
            AddColumn("dbo.t_plc", "CommInterval", c => c.Int(nullable: false));
            AddColumn("dbo.t_tray_scaner", "ScanCommand", c => c.String());
            AddColumn("dbo.t_tray_scaner", "CommType", c => c.String());
            AddColumn("dbo.t_tray_scaner", "ReadTimeout", c => c.Int(nullable: false));
            AddColumn("dbo.t_tray_scaner", "CommInterval", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_tray_scaner", "CommInterval");
            DropColumn("dbo.t_tray_scaner", "ReadTimeout");
            DropColumn("dbo.t_tray_scaner", "CommType");
            DropColumn("dbo.t_tray_scaner", "ScanCommand");
            DropColumn("dbo.t_plc", "CommInterval");
            DropColumn("dbo.t_plc", "ReadTimeout");
            DropColumn("dbo.t_plc", "CommType");
            DropColumn("dbo.t_mes", "CommInterval");
            DropColumn("dbo.t_mes", "ReadTimeout");
            DropColumn("dbo.t_mes", "CommType");
            DropColumn("dbo.t_battery_scaner", "CommInterval");
            DropColumn("dbo.t_battery_scaner", "ReadTimeout");
            DropColumn("dbo.t_battery_scaner", "CommType");
            DropColumn("dbo.t_battery_scaner", "ScanCommand");
        }
    }
}
