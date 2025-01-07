using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class TT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "UserInfos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "UserInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UserInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendAt",
                table: "FriendRequests",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2025, 1, 7, 16, 44, 32, 555, DateTimeKind.Utc).AddTicks(2509),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(2024, 12, 26, 11, 5, 59, 301, DateTimeKind.Utc).AddTicks(9164));

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_FullName",
                table: "UserInfos",
                column: "FullName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInfos_FullName",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UserInfos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SendAt",
                table: "FriendRequests",
                type: "timestamp with time zone",
                nullable: true,
                defaultValue: new DateTime(2024, 12, 26, 11, 5, 59, 301, DateTimeKind.Utc).AddTicks(9164),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true,
                oldDefaultValue: new DateTime(2025, 1, 7, 16, 44, 32, 555, DateTimeKind.Utc).AddTicks(2509));
        }
    }
}
