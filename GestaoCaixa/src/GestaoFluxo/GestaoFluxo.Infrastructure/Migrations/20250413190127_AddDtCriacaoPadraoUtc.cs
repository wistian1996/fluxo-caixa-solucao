using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoFluxo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDtCriacaoPadraoUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_criacao",
                table: "lancamentos",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "(UTC_TIMESTAMP())",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_criacao",
                table: "lancamentos",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()");
        }
    }
}
