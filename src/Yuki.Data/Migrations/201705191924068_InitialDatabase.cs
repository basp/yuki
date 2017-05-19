namespace Yuki.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkspaceId = c.Int(nullable: false),
                        Notes = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkspaceId = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkspaceId = c.Int(nullable: false),
                        ClientId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        IsPrivate = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkspaceId = c.Int(nullable: false),
                        TimeEntryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TimeEntries", t => t.TimeEntryId, cascadeDelete: true)
                .Index(t => t.TimeEntryId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        WorkspaceId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        WorkspaceId = c.Int(nullable: false),
                        ProjectId = c.Int(),
                        TaskId = c.Int(),
                        UserId = c.Int(nullable: false),
                        Start = c.DateTime(nullable: false),
                        Stop = c.DateTime(),
                        Duration = c.Int(nullable: false),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        FullName = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Workspaces",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LastUpdated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WorkspaceUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkspaceId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Workspaces", t => t.WorkspaceId, cascadeDelete: true)
                .Index(t => t.WorkspaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkspaceUsers", "WorkspaceId", "dbo.Workspaces");
            DropForeignKey("dbo.Tags", "TimeEntryId", "dbo.TimeEntries");
            DropIndex("dbo.WorkspaceUsers", new[] { "WorkspaceId" });
            DropIndex("dbo.Tags", new[] { "TimeEntryId" });
            DropTable("dbo.WorkspaceUsers");
            DropTable("dbo.Workspaces");
            DropTable("dbo.Users");
            DropTable("dbo.TimeEntries");
            DropTable("dbo.Tasks");
            DropTable("dbo.Tags");
            DropTable("dbo.Projects");
            DropTable("dbo.Groups");
            DropTable("dbo.Clients");
        }
    }
}
