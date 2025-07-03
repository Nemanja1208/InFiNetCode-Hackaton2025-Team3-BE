using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure_Layer.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdeaSessionMvpPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetAudience",
                table: "MvpPlans",
                newName: "TimeEstimate");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ExperienceLevel",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "IdeaSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TargetAudience",
                table: "IdeaSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "MvpPlans");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "MvpPlans");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "IdeaSessions");

            migrationBuilder.DropColumn(
                name: "TargetAudience",
                table: "IdeaSessions");

            migrationBuilder.RenameColumn(
                name: "TimeEstimate",
                table: "MvpPlans",
                newName: "TargetAudience");

            migrationBuilder.AlterColumn<string>(
                name: "Summary",
                table: "MvpPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
