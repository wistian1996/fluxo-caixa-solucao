using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoFluxo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOutboxUsingJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_outbox_messages_is_processado_data_criacao",
                table: "outbox_messages");

            migrationBuilder.DropColumn(
                name: "is_processado",
                table: "outbox_messages");

            migrationBuilder.AlterColumn<string>(
                name: "payload",
                table: "outbox_messages",
                type: "JSON",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5000)",
                oldMaxLength: 5000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_outbox_messages_data_processamento_data_criacao",
                table: "outbox_messages",
                columns: new[] { "data_processamento", "data_criacao" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_outbox_messages_data_processamento_data_criacao",
                table: "outbox_messages");

            migrationBuilder.AlterColumn<string>(
                name: "payload",
                table: "outbox_messages",
                type: "varchar(5000)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "JSON")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "is_processado",
                table: "outbox_messages",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_outbox_messages_is_processado_data_criacao",
                table: "outbox_messages",
                columns: new[] { "is_processado", "data_criacao" });
        }
    }
}
