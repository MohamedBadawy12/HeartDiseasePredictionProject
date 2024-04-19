using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddingMedicalTestAndLabAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTests_MedicalAnalysts_MedicalAnalystId",
                table: "MedicalTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_MedicalTests_MedicalId",
                table: "Predictions");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Predictions",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "MedicalId",
                table: "Predictions",
                newName: "MedicalTestId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_MedicalId",
                table: "Predictions",
                newName: "IX_Predictions_MedicalTestId");

            migrationBuilder.AlterColumn<int>(
                name: "MedicalAnalystId",
                table: "MedicalTests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "MedicalAnalystEmail",
                table: "MedicalTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientEmail",
                table: "MedicalTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "MedicalTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Prediction",
                table: "MedicalTests",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Probability",
                table: "MedicalTests",
                type: "real",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LabAppointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PateintName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    PatientID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientSSN = table.Column<long>(type: "bigint", nullable: false),
                    LabId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabAppointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabAppointments_AspNetUsers_PatientID",
                        column: x => x.PatientID,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LabAppointments_Labs_LabId",
                        column: x => x.LabId,
                        principalTable: "Labs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabAppointments_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointments_LabId",
                table: "LabAppointments",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointments_PatientID",
                table: "LabAppointments",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_LabAppointments_PatientSSN",
                table: "LabAppointments",
                column: "PatientSSN");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTests_MedicalAnalysts_MedicalAnalystId",
                table: "MedicalTests",
                column: "MedicalAnalystId",
                principalTable: "MedicalAnalysts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_MedicalTests_MedicalTestId",
                table: "Predictions",
                column: "MedicalTestId",
                principalTable: "MedicalTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTests_MedicalAnalysts_MedicalAnalystId",
                table: "MedicalTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Predictions_MedicalTests_MedicalTestId",
                table: "Predictions");

            migrationBuilder.DropTable(
                name: "LabAppointments");

            migrationBuilder.DropColumn(
                name: "MedicalAnalystEmail",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "PatientEmail",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Prediction",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Probability",
                table: "MedicalTests");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Predictions",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "MedicalTestId",
                table: "Predictions",
                newName: "MedicalId");

            migrationBuilder.RenameIndex(
                name: "IX_Predictions_MedicalTestId",
                table: "Predictions",
                newName: "IX_Predictions_MedicalId");

            migrationBuilder.AlterColumn<int>(
                name: "MedicalAnalystId",
                table: "MedicalTests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTests_MedicalAnalysts_MedicalAnalystId",
                table: "MedicalTests",
                column: "MedicalAnalystId",
                principalTable: "MedicalAnalysts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Predictions_MedicalTests_MedicalId",
                table: "Predictions",
                column: "MedicalId",
                principalTable: "MedicalTests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
