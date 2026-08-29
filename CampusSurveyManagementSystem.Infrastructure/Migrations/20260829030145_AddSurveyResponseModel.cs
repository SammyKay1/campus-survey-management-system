using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusSurveyManagementSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSurveyResponseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_SessionIdentifier",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "SessionIdentifier",
                table: "SurveyResponses");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "SurveyResponses",
                newName: "RespondentUserId");

            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "SurveyResponses",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResponses_UserId",
                table: "SurveyResponses",
                newName: "IX_SurveyResponses_RespondentUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "SurveyResponses",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "SurveyResponses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ResponseAnswerOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResponseAnswerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionOptionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseAnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponseAnswerOptions_QuestionOptions_QuestionOptionId",
                        column: x => x.QuestionOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResponseAnswerOptions_ResponseAnswers_ResponseAnswerId",
                        column: x => x.ResponseAnswerId,
                        principalTable: "ResponseAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_Status",
                table: "SurveyResponses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseAnswerOptions_QuestionOptionId",
                table: "ResponseAnswerOptions",
                column: "QuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponseAnswerOptions_ResponseAnswerId_QuestionOptionId",
                table: "ResponseAnswerOptions",
                columns: new[] { "ResponseAnswerId", "QuestionOptionId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResponseAnswerOptions");

            migrationBuilder.DropIndex(
                name: "IX_SurveyResponses_Status",
                table: "SurveyResponses");

            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "SurveyResponses");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "SurveyResponses",
                newName: "IsComplete");

            migrationBuilder.RenameColumn(
                name: "RespondentUserId",
                table: "SurveyResponses",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SurveyResponses_RespondentUserId",
                table: "SurveyResponses",
                newName: "IX_SurveyResponses_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SubmittedAt",
                table: "SurveyResponses",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SessionIdentifier",
                table: "SurveyResponses",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponses_SessionIdentifier",
                table: "SurveyResponses",
                column: "SessionIdentifier");
        }
    }
}
