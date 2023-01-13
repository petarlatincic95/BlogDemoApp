using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogWebApp.Migrations.AuthDb
{
    public partial class AddingNormalizedUserName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a5f85b91-e8e6-43dc-9886-5a3191c73855",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6aa49de0-2ae3-439a-b40a-44a3b8dbb099", "SUPERADMIN@BLOGWEB.COM", "SUPERADMIN@BLOGWEB.COM", "AQAAAAEAACcQAAAAELOo4F3vi5NoSFUxd98sZ0OLjw5IqS+VWPqIryvInJA/kD1QLL46S+QxYAmjgoV3iQ==", "dd71d00d-4e31-47b5-9dab-823b9c8cc422" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a5f85b91-e8e6-43dc-9886-5a3191c73855",
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp" },
                values: new object[] { "42dffc50-fdea-4674-917d-1a5ca30aac7a", null, null, "AQAAAAEAACcQAAAAELS5lZEOGpW1iOcTrNhCnBx7M7Ag+4ZoHCkE/3+7WXwd5do//bjPu2DTxeEd0vs8aw==", "60d44aae-28ae-42b3-a74c-ed7f0c2a25a9" });
        }
    }
}
