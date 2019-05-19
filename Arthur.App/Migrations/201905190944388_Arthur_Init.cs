namespace Arthur.App.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Arthur_Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.t_event_log",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 255),
                        EventType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_oplog",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 255),
                        OpType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_option",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 50),
                        Value = c.String(maxLength: 200),
                        Remark = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.t_user",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Gender = c.Int(nullable: false),
                        Nickname = c.String(maxLength: 50),
                        ProfilePicture = c.String(maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 255),
                        Number = c.String(maxLength: 50),
                        PhoneNumber = c.String(maxLength: 30),
                        Email = c.String(maxLength: 30),
                        RegisterTime = c.DateTime(),
                        LastLoginTime = c.DateTime(),
                        LoginTimes = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.t_role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.t_user", "RoleId", "dbo.t_role");
            DropIndex("dbo.t_user", new[] { "RoleId" });
            DropTable("dbo.t_user");
            DropTable("dbo.t_role");
            DropTable("dbo.t_option");
            DropTable("dbo.t_oplog");
            DropTable("dbo.t_event_log");
        }
    }
}
