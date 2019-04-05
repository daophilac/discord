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
                    PermissionId = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(maxLength: 256, nullable: false),
                    Password = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.UniqueConstraint("UK_Email", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    ServerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    AdminId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_Server_User_AdminId",
                        column: x => x.AdminId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Channel",
                columns: table => new
                {
                    ChannelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ServerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channel", x => x.ChannelId);
                    table.ForeignKey(
                        name: "FK_Channel_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ServerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Role_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServerUser",
                columns: table => new
                {
                    ServerId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerUser", x => new { x.ServerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ServerUser_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChannelId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Time = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ChannelRolePermission",
                columns: table => new
                {
                    ChannelId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelRolePermission", x => new { x.ChannelId, x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_ChannelRolePermission_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelRolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.NoAction);
                });
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "PermissionId", "Description", "Name" },
                values: new object[,]
                {
                    { "full", "Will allow users to do anything", "full" },
                    { "no_react", "Won't allow users to give reactions", "no react" },
                    { "no_chat", "Won't allow users to chat", "no chat" },
                    { "no_view", "Won't allow users to see anything", "no view" }
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Email", "FirstName", "Gender", "Image", "LastName", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, "daophilac@gmail.com", "Đào Phi", 0, "user_1.png", "Lạc", "123", "peanut" },
                    { 2, "daophilac1@gmail.com", "Đào Phi", 0, "user_2.png", "Lạc", "123", "peanut" },
                    { 3, "lucknight@gmail.com", "luck", 0, "user_3.png", "night", "123", "lucknight" },
                    { 4, "eddie@gmail.com", "ed", 0, "user_4.png", "die", "123", "eddie" }
                });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerId", "AdminId", "Image", "Name" },
                values: new object[] { 1, 1, "server_1.png", "Final Fantasy" });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerId", "AdminId", "Image", "Name" },
                values: new object[] { 2, 1, "server_2.png", "Ys" });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerId", "AdminId", "Image", "Name" },
                values: new object[] { 3, 2, "server_3.png", "Hentai Maiden" });

            migrationBuilder.InsertData(
                table: "Channel",
                columns: new[] { "ChannelId", "Name", "ServerId" },
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
                columns: new[] { "RoleId", "Name", "ServerId" },
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
                columns: new[] { "ServerId", "UserId" },
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
                columns: new[] { "ChannelId", "RoleId", "PermissionId" },
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
                columns: new[] { "MessageId", "ChannelId", "Content", "Time", "UserId" },
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
                name: "IX_Channel_ServerId",
                table: "Channel",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRolePermission_PermissionId",
                table: "ChannelRolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRolePermission_RoleId",
                table: "ChannelRolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChannelId",
                table: "Message",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ServerId",
                table: "Role",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_Server_AdminId",
                table: "Server",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUser_UserId",
                table: "ServerUser",
                column: "UserId");
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
