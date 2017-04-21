namespace Yuki.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zero : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Entries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        WorkspaceId = c.Int(nullable: false),
                        ProjectId = c.Int(),
                        Description = c.String(),
                        Duration = c.Time(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId)
                .Index(t => t.UserId)
                .Index(t => t.ProjectId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkspaceId = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workspaces", t => t.WorkspaceId, cascadeDelete: true)
                .Index(t => t.WorkspaceId);
            
            CreateTable(
                "dbo.Timers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkspaceId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Description = c.String(),
                        Started = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workspaces", t => t.WorkspaceId, cascadeDelete: true)
                .Index(t => t.WorkspaceId);
            
            CreateTable(
                "dbo.Workspaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Timers", "WorkspaceId", "dbo.Workspaces");
            DropForeignKey("dbo.Projects", "WorkspaceId", "dbo.Workspaces");
            DropForeignKey("dbo.Entries", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Entries", "UserId", "dbo.Users");
            DropIndex("dbo.Timers", new[] { "WorkspaceId" });
            DropIndex("dbo.Projects", new[] { "WorkspaceId" });
            DropIndex("dbo.Entries", new[] { "ProjectId" });
            DropIndex("dbo.Entries", new[] { "UserId" });
            DropTable("dbo.Workspaces");
            DropTable("dbo.Timers");
            DropTable("dbo.Projects");
            DropTable("dbo.Users");
            DropTable("dbo.Entries");
        }
    }
}