using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vebtech.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$y2Vi6iUMXnccEfIbNA6F0eR4fG0pAHbkS.1FTwThJeafnMxHo7YVS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Admins",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "$2a$11$0KS93akMpFsNS8GdNEOOv.VGwKR.mXlRI4D1WdAF4rxlFxf6idNfC");
        }
    }
}
