using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusSurveyManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyOrganizationMembership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "OrganizationMemberships",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "OrganizationMemberships");
        }
    }
}
