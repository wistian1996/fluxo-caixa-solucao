using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoFluxo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDtCriacaoPadraoUtcPrecisao3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_criacao",
                table: "lancamentos",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "(UTC_TIMESTAMP(3))",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "UTC_TIMESTAMP()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "data_criacao",
                table: "lancamentos",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "UTC_TIMESTAMP()",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "(UTC_TIMESTAMP(3))");
        }
    }
}
