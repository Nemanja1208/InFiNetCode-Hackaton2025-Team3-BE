using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Layer.Migrations
{
    /// <inheritdoc />
    public partial class updateMvpPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoreFeatures",
                table: "MvpPlans");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "MvpPlans");

            migrationBuilder.DropColumn(
                name: "TimeEstimate",
                table: "MvpPlans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoreFeatures",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TimeEstimate",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
