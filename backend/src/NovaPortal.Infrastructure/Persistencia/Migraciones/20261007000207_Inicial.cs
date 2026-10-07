using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NovaPortal.Infrastructure.Persistencia.Migraciones
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "colaboradores",
                columns: table => new
                {
                    id_colaborador = table.Column<Guid>(type: "uuid", nullable: false),
                    correo_colaborador = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    nombre_colaborador = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    sal_colaborador = table.Column<byte[]>(type: "bytea", nullable: false),
                    hash_colaborador = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_colaboradores", x => x.id_colaborador);
                });

            migrationBuilder.InsertData(
                table: "colaboradores",
                columns: new[] { "id_colaborador", "correo_colaborador", "hash_colaborador", "nombre_colaborador", "sal_colaborador" },
                values: new object[,]
                {
                    { new Guid("0199b7a2-0000-7000-8000-000000000001"), "ana.torres@novatech.com.pe", new byte[] { 54, 120, 221, 207, 204, 245, 159, 167, 132, 254, 61, 13, 192, 64, 68, 158, 201, 234, 197, 141, 118, 143, 243, 191, 27, 228, 149, 204, 239, 234, 135, 115 }, "Ana Torres", new byte[] { 129, 228, 9, 21, 251, 172, 167, 190, 207, 206, 207, 203, 40, 67, 42, 9 } },
                    { new Guid("0199b7a2-0000-7000-8000-000000000002"), "carlos.rojas@novatech.com.pe", new byte[] { 179, 201, 63, 20, 197, 9, 93, 62, 66, 182, 94, 248, 168, 100, 176, 57, 57, 176, 217, 85, 200, 77, 173, 130, 74, 116, 255, 13, 249, 193, 197, 179 }, "Carlos Rojas", new byte[] { 9, 212, 39, 200, 68, 27, 165, 49, 168, 49, 97, 189, 135, 199, 244, 97 } }
                });

            migrationBuilder.CreateIndex(
                name: "ux_colaboradores_correo_colaborador",
                table: "colaboradores",
                column: "correo_colaborador",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "colaboradores");
        }
    }
}
