namespace API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class APIContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Channel",
                c => new
                    {
                        ChannelID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ServerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChannelID)
                .ForeignKey("dbo.Server", t => t.ServerID, cascadeDelete: true)
                .Index(t => t.ServerID);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        MessageID = c.Int(nullable: false, identity: true),
                        ChannelID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.MessageID)
                .ForeignKey("dbo.Channel", t => t.ChannelID, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ChannelID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        Password = c.String(),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Gender = c.Int(),
                        Image = c.String(),
                        Server_ServerID = c.Int(),
                        Role_RoleID = c.Int(),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Server", t => t.Server_ServerID)
                .ForeignKey("dbo.Role", t => t.Role_RoleID)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Server_ServerID)
                .Index(t => t.Role_RoleID);
            
            CreateTable(
                "dbo.ChannelRolePermission",
                c => new
                    {
                        ChannelID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                        PermissionID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ChannelID, t.RoleID, t.PermissionID })
                .ForeignKey("dbo.Channel", t => t.ChannelID, cascadeDelete: true)
                .ForeignKey("dbo.Permission", t => t.PermissionID, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.ChannelID)
                .Index(t => t.RoleID)
                .Index(t => t.PermissionID);
            
            CreateTable(
                "dbo.Permission",
                c => new
                    {
                        PermissionID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PermissionID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ServerID = c.Int(nullable: false),
                        Server_ServerID = c.Int(),
                    })
                .PrimaryKey(t => t.RoleID)
                .ForeignKey("dbo.Server", t => t.Server_ServerID)
                .ForeignKey("dbo.Server", t => t.ServerID)
                .Index(t => t.ServerID)
                .Index(t => t.Server_ServerID);
            
            CreateTable(
                "dbo.Server",
                c => new
                    {
                        ServerID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Image = c.String(),
                        AdminID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ServerID)
                .ForeignKey("dbo.User", t => t.AdminID)
                .Index(t => t.AdminID);
            
            CreateTable(
                "dbo.ServerUser",
                c => new
                    {
                        ServerID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ServerID, t.UserID })
                .ForeignKey("dbo.Server", t => t.ServerID)
                .ForeignKey("dbo.User", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ServerID)
                .Index(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServerUser", "UserID", "dbo.User");
            DropForeignKey("dbo.ServerUser", "ServerID", "dbo.Server");
            DropForeignKey("dbo.ChannelRolePermission", "RoleID", "dbo.Role");
            DropForeignKey("dbo.User", "Role_RoleID", "dbo.Role");
            DropForeignKey("dbo.Role", "ServerID", "dbo.Server");
            DropForeignKey("dbo.User", "Server_ServerID", "dbo.Server");
            DropForeignKey("dbo.Server", "AdminID", "dbo.User");
            DropForeignKey("dbo.Role", "Server_ServerID", "dbo.Server");
            DropForeignKey("dbo.Channel", "ServerID", "dbo.Server");
            DropForeignKey("dbo.ChannelRolePermission", "PermissionID", "dbo.Permission");
            DropForeignKey("dbo.ChannelRolePermission", "ChannelID", "dbo.Channel");
            DropForeignKey("dbo.Message", "UserID", "dbo.User");
            DropForeignKey("dbo.Message", "ChannelID", "dbo.Channel");
            DropIndex("dbo.ServerUser", new[] { "UserID" });
            DropIndex("dbo.ServerUser", new[] { "ServerID" });
            DropIndex("dbo.Server", new[] { "AdminID" });
            DropIndex("dbo.Role", new[] { "Server_ServerID" });
            DropIndex("dbo.Role", new[] { "ServerID" });
            DropIndex("dbo.ChannelRolePermission", new[] { "PermissionID" });
            DropIndex("dbo.ChannelRolePermission", new[] { "RoleID" });
            DropIndex("dbo.ChannelRolePermission", new[] { "ChannelID" });
            DropIndex("dbo.User", new[] { "Role_RoleID" });
            DropIndex("dbo.User", new[] { "Server_ServerID" });
            DropIndex("dbo.User", new[] { "Email" });
            DropIndex("dbo.Message", new[] { "UserID" });
            DropIndex("dbo.Message", new[] { "ChannelID" });
            DropIndex("dbo.Channel", new[] { "ServerID" });
            DropTable("dbo.ServerUser");
            DropTable("dbo.Server");
            DropTable("dbo.Role");
            DropTable("dbo.Permission");
            DropTable("dbo.ChannelRolePermission");
            DropTable("dbo.User");
            DropTable("dbo.Message");
            DropTable("dbo.Channel");
        }
    }
}
