using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class AddZoneAndTimesForLab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Labs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientImage",
                table: "LabAppointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndTime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartTime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientImage",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientImage",
                table: "AcceptAndCancelLabAppointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatientImage",
                table: "AcceptAndCancelAppointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Labs");

            migrationBuilder.DropColumn(
                name: "PatientImage",
                table: "LabAppointments");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Zone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PatientImage",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientImage",
                table: "AcceptAndCancelLabAppointments");

            migrationBuilder.DropColumn(
                name: "PatientImage",
                table: "AcceptAndCancelAppointments");
        }
    }
}
