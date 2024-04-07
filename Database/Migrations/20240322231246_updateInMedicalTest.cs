using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class updateInMedicalTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropColumn(
                name: "AlcoholConsumption",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "BloodPresureAbove",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "BloodPresureDown",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Cholesterol",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Diabets",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Diet",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "ExcersiceHoursPerWeek",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "FamilyHistory",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "HeartRate",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "MedicalTests");

            migrationBuilder.RenameColumn(
                name: "Triglycerides",
                table: "MedicalTests",
                newName: "SystolicBloodPressure");

            migrationBuilder.RenameColumn(
                name: "StressLevel",
                table: "MedicalTests",
                newName: "Prevalenthypertension");

            migrationBuilder.RenameColumn(
                name: "SleepHoursPerDay",
                table: "MedicalTests",
                newName: "PrevalentStroke");

            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "MedicalTests",
                newName: "NumberOfCigarettes");

            migrationBuilder.RenameColumn(
                name: "SedentaryHoursPerDay",
                table: "MedicalTests",
                newName: "GlucoseLevel");

            migrationBuilder.RenameColumn(
                name: "PreviousHeartProblems",
                table: "MedicalTests",
                newName: "DiastolicBloodPressure");

            migrationBuilder.RenameColumn(
                name: "PhysicalActivityDaysPerWeek",
                table: "MedicalTests",
                newName: "Diabetes");

            migrationBuilder.RenameColumn(
                name: "Obesity",
                table: "MedicalTests",
                newName: "CholesterolLevel");

            migrationBuilder.RenameColumn(
                name: "MedicationUse",
                table: "MedicalTests",
                newName: "BloodPressureMedicine");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Prescriptions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "MedicalTests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "MedicalTests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MedicalTests_UserId",
                table: "MedicalTests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalTests_AspNetUsers_UserId",
                table: "MedicalTests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicalTests_AspNetUsers_UserId",
                table: "MedicalTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions");

            migrationBuilder.DropIndex(
                name: "IX_MedicalTests_UserId",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "MedicalTests");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MedicalTests");

            migrationBuilder.RenameColumn(
                name: "SystolicBloodPressure",
                table: "MedicalTests",
                newName: "Triglycerides");

            migrationBuilder.RenameColumn(
                name: "Prevalenthypertension",
                table: "MedicalTests",
                newName: "StressLevel");

            migrationBuilder.RenameColumn(
                name: "PrevalentStroke",
                table: "MedicalTests",
                newName: "SleepHoursPerDay");

            migrationBuilder.RenameColumn(
                name: "NumberOfCigarettes",
                table: "MedicalTests",
                newName: "Sex");

            migrationBuilder.RenameColumn(
                name: "GlucoseLevel",
                table: "MedicalTests",
                newName: "SedentaryHoursPerDay");

            migrationBuilder.RenameColumn(
                name: "DiastolicBloodPressure",
                table: "MedicalTests",
                newName: "PreviousHeartProblems");

            migrationBuilder.RenameColumn(
                name: "Diabetes",
                table: "MedicalTests",
                newName: "PhysicalActivityDaysPerWeek");

            migrationBuilder.RenameColumn(
                name: "CholesterolLevel",
                table: "MedicalTests",
                newName: "Obesity");

            migrationBuilder.RenameColumn(
                name: "BloodPressureMedicine",
                table: "MedicalTests",
                newName: "MedicationUse");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "Prescriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "AlcoholConsumption",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "BloodPresureAbove",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "BloodPresureDown",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Cholesterol",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Diabets",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Diet",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ExcersiceHoursPerWeek",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FamilyHistory",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HeartRate",
                table: "MedicalTests",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "Label",
                table: "MedicalTests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Prescriptions_Doctors_DoctorId",
                table: "Prescriptions",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
