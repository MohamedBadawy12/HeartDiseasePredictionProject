using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAcceptAndCancelLabApp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "AcceptAndCancelLabAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PateintName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LabEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientSSN = table.Column<long>(type: "bigint", nullable: false),
                    LabId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcceptAndCancelLabAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcceptAndCancelLabAppointments_AspNetUsers_PatientID",
                        column: x => x.PatientID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AcceptAndCancelLabAppointments_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcceptAndCancelLabAppointments_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcceptAndCancelLabAppointments_LabId",
                table: "AcceptAndCancelLabAppointments",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptAndCancelLabAppointments_PatientID",
                table: "AcceptAndCancelLabAppointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_AcceptAndCancelLabAppointments_PatientSSN",
                table: "AcceptAndCancelLabAppointments",
                column: "PatientSSN");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcceptAndCancelLabAppointments");

            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "Labs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
