using Microsoft.EntityFrameworkCore.Migrations;
using System;


#nullable disable

namespace Database.Migrations
{
	/// <inheritdoc />
	public partial class seedRoles : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
			   table: "AspNetRoles",
			   columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
			   values: new object[] { Guid.NewGuid().ToString(), "User", "User".ToUpper(), Guid.NewGuid().ToString() }
				);

			migrationBuilder.InsertData(
			  table: "AspNetRoles",
			  columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
			  values: new object[] { Guid.NewGuid().ToString(), "Doctor", "Doctor".ToUpper(), Guid.NewGuid().ToString() }
			   );

			migrationBuilder.InsertData(
			  table: "AspNetRoles",
			  columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
			  values: new object[] { Guid.NewGuid().ToString(), "MedicalAnalyst", "MedicalAnalyst".ToUpper(), Guid.NewGuid().ToString() }
			   );

			migrationBuilder.InsertData(
			  table: "AspNetRoles",
			  columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
			  values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }
			   );
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.Sql("DELETE FROM [AspNetRoles]");
		}
	}
}
