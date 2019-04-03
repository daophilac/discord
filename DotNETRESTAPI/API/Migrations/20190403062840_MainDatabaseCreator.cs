using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class MainDatabaseCreator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    PermissionID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    Password = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    ServerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    AdminID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.ServerID);
                    table.ForeignKey(
                        name: "FK_Server_User_AdminID",
                        column: x => x.AdminID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    ChannelID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ServerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.ChannelID);
                    table.ForeignKey(
                        name: "FK_Channel_Server_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ServerID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleID);
                    table.ForeignKey(
                        name: "FK_Role_Server_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerUser",
                columns: table => new
                {
                    ServerID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerUser", x => new { x.ServerID, x.UserID });
                    table.ForeignKey(
                        name: "FK_ServerUser_Server_ServerID",
                        column: x => x.ServerID,
                        principalTable: "Server",
                        principalColumn: "ServerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerUser_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelID = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Message_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "ChannelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChannelRolePermission",
                columns: table => new
                {
                    ChannelID = table.Column<int>(nullable: false),
                    RoleID = table.Column<int>(nullable: false),
                    PermissionID = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelRolePermission", x => new { x.ChannelID, x.RoleID, x.PermissionID });
                    table.ForeignKey(
                        name: "FK_ChannelRolePermission_Channel_ChannelID",
                        column: x => x.ChannelID,
                        principalTable: "Channel",
                        principalColumn: "ChannelID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRolePermission_Permission_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permission",
                        principalColumn: "PermissionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRolePermission_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionID", "Description", "Name" },
                values: new object[,]
                {
                    { "full", "Will allow users to do anything", "full" },
                    { "no_react", "Won't allow users to give reactions", "no react" },
                    { "no_chat", "Won't allow users to chat", "no chat" },
                    { "no_view", "Won't allow users to see anything", "no view" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserID", "Email", "FirstName", "Gender", "Image", "LastName", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, "daophilac@gmail.com", "Đào Phi", 0, "user_1.png", "Lạc", "123", "peanut" },
                    { 2, "daophilac1@gmail.com", "Đào Phi", 0, "user_2.png", "Lạc", "123", "peanut" },
                    { 3, "lucknight@gmail.com", "luck", 0, "user_3.png", "night", "123", "lucknight" },
                    { 4, "eddie@gmail.com", "ed", 0, "user_4.png", "die", "123", "eddie" }
                });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerID", "AdminID", "Image", "Name" },
                values: new object[] { 1, 1, "server_1.png", "Final Fantasy" });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerID", "AdminID", "Image", "Name" },
                values: new object[] { 2, 1, "server_2.png", "Ys" });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerID", "AdminID", "Image", "Name" },
                values: new object[] { 3, 2, "server_3.png", "Hentai Maiden" });

            migrationBuilder.InsertData(
                table: "Channel",
                columns: new[] { "ChannelID", "Name", "ServerID" },
                values: new object[,]
                {
                    { 1, "General", 1 },
                    { 2, "Boss", 1 },
                    { 3, "Random Encounter", 1 },
                    { 8, "Secret", 3 },
                    { 7, "General", 3 },
                    { 4, "Origin", 2 },
                    { 5, "Ys7", 2 },
                    { 6, "Ys8", 2 }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleID", "Name", "ServerID" },
                values: new object[,]
                {
                    { 10, "Folk", 3 },
                    { 9, "Artist", 3 },
                    { 8, "Admin", 3 },
                    { 7, "Aisha", 2 },
                    { 6, "Dogi", 2 },
                    { 5, "Adol", 2 },
                    { 4, "Black Wizard", 1 },
                    { 3, "White Wizard", 1 },
                    { 2, "Thief", 1 },
                    { 1, "Knight", 1 }
                });

            migrationBuilder.InsertData(
                table: "ServerUser",
                columns: new[] { "ServerID", "UserID" },
                values: new object[,]
                {
                    { 1, 3 },
                    { 1, 2 },
                    { 2, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 2, 4 },
                    { 1, 1 },
                    { 3, 2 },
                    { 3, 1 },
                    { 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "ChannelRolePermission",
                columns: new[] { "ChannelID", "RoleID", "PermissionID" },
                values: new object[,]
                {
                    { 2, 4, "full" },
                    { 8, 1, "full" },
                    { 7, 3, "full" },
                    { 7, 2, "full" },
                    { 7, 1, "full" },
                    { 6, 3, "no_view" },
                    { 6, 2, "full" },
                    { 6, 1, "full" },
                    { 5, 3, "full" },
                    { 5, 2, "full" },
                    { 5, 1, "full" },
                    { 4, 3, "no_view" },
                    { 4, 2, "no_view" },
                    { 4, 1, "full" },
                    { 3, 4, "no_chat" },
                    { 8, 2, "full" },
                    { 8, 3, "no_chat" },
                    { 3, 3, "no_chat" },
                    { 2, 3, "full" },
                    { 1, 3, "full" },
                    { 3, 2, "full" },
                    { 2, 2, "full" },
                    { 1, 2, "full" },
                    { 3, 1, "full" },
                    { 2, 1, "full" },
                    { 1, 1, "full" },
                    { 1, 4, "full" }
                });

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "MessageID", "ChannelID", "Content", "Time", "UserID" },
                values: new object[,]
                {
                    { 6, 2, "Hi there", new DateTime(2019, 1, 2, 0, 0, 3, 543, DateTimeKind.Unspecified), 2 },
                    { 5, 2, "BBBBBBBBBBBBBB", new DateTime(2019, 1, 2, 0, 0, 2, 899, DateTimeKind.Unspecified), 1 },
                    { 4, 2, "Another channel in final fantasy", new DateTime(2019, 1, 2, 0, 0, 1, 123, DateTimeKind.Unspecified), 1 },
                    { 3, 1, "AAAAAAAAAA", new DateTime(2019, 1, 2, 0, 0, 2, 368, DateTimeKind.Unspecified), 3 },
                    { 2, 1, "And this is the second message in final fantasy", new DateTime(2019, 1, 2, 0, 0, 1, 245, DateTimeKind.Unspecified), 2 },
                    { 1, 1, "This is the first message in final fantasy", new DateTime(2019, 1, 1, 0, 0, 0, 1, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ServerID",
                table: "Channel",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRolePermission_PermissionID",
                table: "ChannelRolePermission",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRolePermission_RoleID",
                table: "ChannelRolePermission",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChannelID",
                table: "Message",
                column: "ChannelID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserID",
                table: "Message",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ServerID",
                table: "Role",
                column: "ServerID");

            migrationBuilder.CreateIndex(
                name: "IX_Server_AdminID",
                table: "Server",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUser_UserID",
                table: "ServerUser",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelRolePermission");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "ServerUser");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "Server");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
