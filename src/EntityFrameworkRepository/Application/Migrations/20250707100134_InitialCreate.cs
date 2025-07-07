using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JMS.Plugins.EntityFramework.Application.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "jms");

            migrationBuilder.CreateTable(
                name: "discount_code",
                schema: "jms",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    used = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_discount_code", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_discount_code_code",
                schema: "jms",
                table: "discount_code",
                column: "code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "discount_code",
                schema: "jms");
        }
    }
}
