namespace GMCC.Sorter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GMCC_PackOptimize : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.t_battery", "TestTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.t_pack", "IsUploaded", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.t_pack", "IsUploaded");
            DropColumn("dbo.t_battery", "TestTime");
        }
    }
}
