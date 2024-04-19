using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatesInLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "date",
                table: "MedicalTests",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "MedicalAnalystEmail",
                table: "MedicalTests",
                newName: "MedicalAnalystName");

            migrationBuilder.AddColumn<string>(
                name: "LabEmail",
                table: "MedicalTests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LabId",
                table: "MedicalTests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Labs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LabEmail",
                table: "LabAppointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTests_LabId",
                table: "MedicalTests",
                column: "LabId");

            migrationBuilder.CreateIndex(
                name: "IX_Labs_UserId",
                table: "Labs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labs_AspNetUsers_UserId",
                table: "Labs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTests_Labs_LabId",
                table: "MedicalTests",
                column: "LabId",
                principalTable: "Labs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labs_AspNetUsers_UserId",
                table: "Labs");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTests_Labs_LabId",
                table: "MedicalTests");

            migrationBuilder.DropIndex(
                name: "IX_MedicalTests_LabId",
                table: "MedicalTests");

            migrationBuilder.DropIndex(
                name: "IX_Labs_UserId",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "LabEmail",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "LabId",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "LabEmail",
                table: "LabAppointments");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "MedicalTests",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "MedicalAnalystName",
                table: "MedicalTests",
                newName: "MedicalAnalystEmail");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
