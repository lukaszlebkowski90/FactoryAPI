using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryAPI.Migrations
{
    /// <inheritdoc />
    public partial class workerFullName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "FullName",
            table: "Workers",
            type: "nvarchar(max)",
            nullable: true);

            migrationBuilder.Sql(@"
                UPDATE Workers
                SET FullName = FirstName + ' ' + LastName
            ");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Workers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "LastName",
               table: "Workers",
               type: "nvarchar(max)",
               nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Workers",
                type: "nvarchar(max)",
                nullable: true);

            // update new columns

            migrationBuilder.DropColumn(
               name: "FullName",
               table: "Workers");
        }
    }
}
