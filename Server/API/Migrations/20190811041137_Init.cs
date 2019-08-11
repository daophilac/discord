using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "VARCHAR(254)", nullable: false),
                    Phone = table.Column<string>(type: "VARCHAR(15)", nullable: true),
                    Password = table.Column<string>(type: "VARCHAR(60)", nullable: false),
                    UserName = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    ImageName = table.Column<string>(maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    ServerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ServerName = table.Column<string>(maxLength: 50, nullable: false),
                    ImageName = table.Column<string>(maxLength: 254, nullable: true),
                    DefaultRoleId = table.Column<int>(nullable: true),
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
                    ChannelName = table.Column<string>(maxLength: 50, nullable: false),
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
                name: "InstantInvite",
                columns: table => new
                {
                    Link = table.Column<string>(type: "VARCHAR(50)", nullable: false),
                    ServerId = table.Column<int>(nullable: false),
                    StillValid = table.Column<bool>(nullable: false),
                    NerverExpired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstantInvite", x => x.Link);
                    table.ForeignKey(
                        name: "FK_InstantInvite_Server_ServerId",
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
                    RoleLevel = table.Column<int>(nullable: false),
                    MainRole = table.Column<bool>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    Kick = table.Column<bool>(nullable: false),
                    ModifyChannel = table.Column<bool>(nullable: false),
                    ModifyRole = table.Column<bool>(nullable: false),
                    ChangeUserRole = table.Column<bool>(nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ChannelPermission",
                columns: table => new
                {
                    ChannelId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    ViewMessage = table.Column<bool>(nullable: false),
                    React = table.Column<bool>(nullable: false),
                    SendMessage = table.Column<bool>(nullable: false),
                    SendImage = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelPermission", x => new { x.ChannelId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_ChannelPermission_Channel_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channel",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelPermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServerUser",
                columns: table => new
                {
                    ServerId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerUser", x => new { x.ServerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ServerUser_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServerUser_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServerUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "UserId", "Email", "ImageName", "Password", "Phone", "UserName" },
                values: new object[,]
                {
                    { 1, "daophilac@gmail.com", "user_1.png", "123", null, "peanut" },
                    { 2, "adol@gmail.com", "user_2.png", "123", null, "adol" },
                    { 3, "lucknight@gmail.com", "user_3.png", "123", null, "lucknight" },
                    { 4, "eddie@gmail.com", "user_4.png", "123", null, "eddie" },
                    { 5, "test@gmail.com", null, "123", null, "test" }
                });

            migrationBuilder.InsertData(
                table: "Server",
                columns: new[] { "ServerId", "AdminId", "DefaultRoleId", "ImageName", "ServerName" },
                values: new object[,]
                {
                    { 1, 1, 2, "server_1.png", "Final Fantasy" },
                    { 2, 1, 4, "server_2.png", "Ys" },
                    { 3, 2, 6, "server_3.png", "Maiden" },
                    { 4, 2, 8, null, "TSFH" }
                });

            migrationBuilder.InsertData(
                table: "Channel",
                columns: new[] { "ChannelId", "ChannelName", "ServerId" },
                values: new object[,]
                {
                    { 1, "General", 1 },
                    { 6, "Ys8", 2 },
                    { 5, "Ys7", 2 },
                    { 4, "Origin", 2 },
                    { 8, "Secret", 3 },
                    { 9, "Sky World", 4 },
                    { 7, "General", 3 },
                    { 3, "Random Encounter", 1 },
                    { 2, "Boss", 1 }
                });

            migrationBuilder.InsertData(
                table: "InstantInvite",
                columns: new[] { "Link", "NerverExpired", "ServerId", "StillValid" },
                values: new object[,]
                {
                    { "e4912f82-d290-40e6-a23d-08d0ce1b50ab", true, 1, true },
                    { "648d7e41-75f9-48b8-8bb2-ccf1ffa74245", true, 2, true },
                    { "15f0529f-53b9-46b9-8bb6-8d8d863eb167", true, 3, true },
                    { "5766df3a-3e31-4c1c-b50c-bc887efd4626", true, 4, true }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "RoleId", "ChangeUserRole", "Kick", "MainRole", "ModifyChannel", "ModifyRole", "RoleLevel", "RoleName", "ServerId" },
                values: new object[,]
                {
                    { 18, false, false, false, false, false, 700, "Folk", 3 },
                    { 17, false, false, false, false, false, 800, "Artist", 3 },
                    { 7, true, true, true, true, true, 1000, "Admin", 4 },
                    { 6, false, false, true, false, false, 0, "Member", 3 },
                    { 5, true, true, true, true, true, 1000, "Admin", 3 },
                    { 16, true, false, false, true, true, 900, "New Admin", 3 },
                    { 4, false, false, true, false, false, 0, "Member", 2 },
                    { 14, false, false, false, false, false, 800, "Dogi", 2 },
                    { 13, true, true, false, true, true, 900, "Adol", 2 },
                    { 8, false, false, true, false, false, 0, "Member", 4 },
                    { 3, true, true, true, true, true, 1000, "Admin", 2 },
                    { 12, false, false, false, false, false, 699, "Black Wizard", 1 },
                    { 11, false, false, false, false, false, 700, "White Wizard", 1 },
                    { 10, false, false, false, false, false, 800, "Thief", 1 },
                    { 9, true, false, false, true, true, 900, "Knight", 1 },
                    { 2, false, false, true, false, false, 0, "Member", 1 },
                    { 1, true, true, true, true, true, 1000, "Admin", 1 },
                    { 15, false, false, false, false, false, 700, "Aisha", 2 },
                    { 19, true, false, false, true, true, 500, "Musician", 4 }
                });

            migrationBuilder.InsertData(
                table: "ChannelPermission",
                columns: new[] { "ChannelId", "RoleId", "React", "SendImage", "SendMessage", "ViewMessage" },
                values: new object[,]
                {
                    { 4, 4, true, true, false, true },
                    { 6, 3, true, true, true, true },
                    { 9, 8, true, true, true, true },
                    { 5, 4, false, false, false, false },
                    { 6, 4, false, false, false, false },
                    { 4, 13, true, true, false, true },
                    { 5, 13, true, true, true, true },
                    { 6, 13, true, true, true, true },
                    { 4, 14, true, true, false, true },
                    { 5, 14, true, true, true, true },
                    { 6, 14, true, true, true, true },
                    { 4, 15, true, true, false, true },
                    { 5, 3, true, true, true, true },
                    { 5, 15, true, true, true, true },
                    { 7, 5, true, true, true, true },
                    { 8, 5, true, true, true, true },
                    { 7, 6, true, true, true, true },
                    { 8, 6, false, false, false, false },
                    { 7, 16, true, true, true, true },
                    { 8, 16, true, true, true, true },
                    { 7, 17, true, true, true, true },
                    { 8, 17, false, false, false, false },
                    { 7, 18, true, true, true, true },
                    { 8, 18, false, false, false, false },
                    { 9, 7, true, true, true, true },
                    { 6, 15, true, true, true, true },
                    { 4, 3, true, true, true, true },
                    { 9, 19, true, true, true, true },
                    { 2, 12, false, false, false, false },
                    { 1, 1, true, true, true, true },
                    { 1, 2, true, true, true, true },
                    { 2, 2, false, false, false, false },
                    { 3, 12, true, true, true, true },
                    { 3, 2, true, true, true, true },
                    { 1, 9, true, true, true, true },
                    { 2, 9, true, true, true, true },
                    { 3, 9, true, true, true, true },
                    { 2, 1, true, true, true, true },
                    { 3, 1, true, true, true, true },
                    { 1, 10, true, true, true, true },
                    { 2, 10, false, false, false, false },
                    { 3, 10, true, true, true, true },
                    { 1, 11, true, true, true, true },
                    { 2, 11, false, false, false, false },
                    { 3, 11, true, true, true, true },
                    { 1, 12, true, true, true, true }
                });

            migrationBuilder.InsertData(
                table: "Message",
                columns: new[] { "MessageId", "ChannelId", "Content", "Time", "UserId" },
                values: new object[,]
                {
                    { 3, 1, "AAAAAAAAAA", new DateTime(2019, 1, 2, 0, 0, 2, 368, DateTimeKind.Unspecified), 3 },
                    { 6, 2, "Hi there", new DateTime(2019, 1, 2, 0, 0, 3, 543, DateTimeKind.Unspecified), 2 },
                    { 2, 1, "And this is the second message in final fantasy", new DateTime(2019, 1, 2, 0, 0, 1, 245, DateTimeKind.Unspecified), 2 },
                    { 4, 2, "Another channel in final fantasy", new DateTime(2019, 1, 2, 0, 0, 1, 123, DateTimeKind.Unspecified), 1 },
                    { 5, 2, "BBBBBBBBBBBBBB", new DateTime(2019, 1, 2, 0, 0, 2, 899, DateTimeKind.Unspecified), 1 },
                    { 1, 1, "This is the first message in final fantasy", new DateTime(2019, 1, 1, 0, 0, 0, 1, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "ServerUser",
                columns: new[] { "ServerId", "UserId", "RoleId" },
                values: new object[,]
                {
                    { 3, 2, 5 },
                    { 3, 1, 16 },
                    { 3, 4, 16 },
                    { 1, 2, 9 },
                    { 1, 3, 9 },
                    { 2, 3, 13 },
                    { 2, 2, 13 },
                    { 2, 1, 3 },
                    { 4, 2, 7 },
                    { 1, 1, 1 },
                    { 2, 4, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ServerId_ChannelName",
                table: "Channel",
                columns: new[] { "ServerId", "ChannelName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChannelPermission_RoleId",
                table: "ChannelPermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InstantInvite_ServerId",
                table: "InstantInvite",
                column: "ServerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ChannelId",
                table: "Message",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserId",
                table: "Message",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ServerId_RoleLevel",
                table: "Role",
                columns: new[] { "ServerId", "RoleLevel" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Server_AdminId",
                table: "Server",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUser_RoleId",
                table: "ServerUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUser_UserId",
                table: "ServerUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerUser_ServerId_UserId_RoleId",
                table: "ServerUser",
                columns: new[] { "ServerId", "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelPermission");

            migrationBuilder.DropTable(
                name: "InstantInvite");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "ServerUser");

            migrationBuilder.DropTable(
                name: "Channel");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Server");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
