using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MigrationHandler.Migrations
{
    /// <inheritdoc />
    public partial class CarsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cars",
                columns: new string[] { "Name"},
                values: new string[] { "3", "Octavia", "Fabia", "Astra", "Mondeo" }               
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
