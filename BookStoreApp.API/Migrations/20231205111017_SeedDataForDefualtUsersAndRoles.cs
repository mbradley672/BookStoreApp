using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataForDefualtUsersAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6BA21BCB-528D-47ED-A7C8-3D95F618FE06", null, "User", "USER" },
                    { "C57394D0-94FF-463A-B323-FBF34E6527F9", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "97C482FC-EE47-44B1-B39D-B9E87036CC53", 0, "62b20d54-1863-4e30-9b5c-828dc6f6ddc3", "Demo@demo.com", false, "Demo", "User", false, null, "DEMO@DEMO.COM", "DEMO", "AQAAAAIAAYagAAAAEBAp5vT01CpNts9QOWJvTR0sBmeaSACQ13hghw0iDKwPVa8E03GGzA4DhbvhF0jhSg==", null, false, "07da078c-5e79-4e02-827d-2494b5d6b057", false, "Demo" },
                    { "9DCC2818-CE12-460E-9748-FB4AAA9F848D", 0, "4a2c9505-cc69-4b1f-9eed-b2ce02cad553", "admin@demo.com", false, "Demo", "Admin", false, null, "ADMIN@DEMO.COM", "ADMIN", "AQAAAAIAAYagAAAAEFg0i+xxCk6iXjIO0gnXoE46KbCBrg9ltnS4iN/nrDXLe+ZnvoPPvCCZTQvJLStdWA==", null, false, "98802086-c399-4ef4-97cb-6f9ceaf60ad6", false, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "6BA21BCB-528D-47ED-A7C8-3D95F618FE06", "97C482FC-EE47-44B1-B39D-B9E87036CC53" },
                    { "C57394D0-94FF-463A-B323-FBF34E6527F9", "9DCC2818-CE12-460E-9748-FB4AAA9F848D" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "6BA21BCB-528D-47ED-A7C8-3D95F618FE06", "97C482FC-EE47-44B1-B39D-B9E87036CC53" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "C57394D0-94FF-463A-B323-FBF34E6527F9", "9DCC2818-CE12-460E-9748-FB4AAA9F848D" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6BA21BCB-528D-47ED-A7C8-3D95F618FE06");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "C57394D0-94FF-463A-B323-FBF34E6527F9");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "97C482FC-EE47-44B1-B39D-B9E87036CC53");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9DCC2818-CE12-460E-9748-FB4AAA9F848D");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60);
        }
    }
}
