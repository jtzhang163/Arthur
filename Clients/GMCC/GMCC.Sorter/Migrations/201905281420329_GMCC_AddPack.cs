namespace GMCC.Sorter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GMCC_AddPack : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_pack",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SortResult = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        Model = c.String(maxLength: 50),
                        Pos = c.Int(nullable: false),
                        ScanTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.t_battery", "PackId", c => c.Int(nullable: false));
            AddColumn("dbo.t_battery", "CAP", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.t_battery", "ESR", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.t_battery", "PackStatus", c => c.Int(nullable: false));
            AddColumn("dbo.t_battery", "IsUploaded", c => c.Boolean(nullable: false));
            AddColumn("dbo.t_battery", "Model", c => c.String(maxLength: 50));
            AddColumn("dbo.t_proc_tray", "Model", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_proc_tray", "Model");
            DropColumn("dbo.t_battery", "Model");
            DropColumn("dbo.t_battery", "IsUploaded");
            DropColumn("dbo.t_battery", "PackStatus");
            DropColumn("dbo.t_battery", "ESR");
            DropColumn("dbo.t_battery", "CAP");
            DropColumn("dbo.t_battery", "PackId");
            DropTable("dbo.t_pack");
        }
    }
}
