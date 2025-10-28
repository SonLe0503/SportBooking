using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportBooking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedbackAndPriceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFixedPrice",
                table: "Fields");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Feedbacks",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Feedbacks",
                newName: "Rating");

            migrationBuilder.RenameColumn(
                name: "fieldID",
                table: "Feedbacks",
                newName: "FieldId");

            migrationBuilder.RenameColumn(
                name: "createdAt",
                table: "Feedbacks",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "comment",
                table: "Feedbacks",
                newName: "Comment");

            migrationBuilder.RenameColumn(
                name: "feedbackID",
                table: "Feedbacks",
                newName: "FeedbackId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_userID",
                table: "Feedbacks",
                newName: "IX_Feedbacks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_fieldID",
                table: "Feedbacks",
                newName: "IX_Feedbacks_FieldId");

            migrationBuilder.AddColumn<decimal>(
                name: "fixedPrice",
                table: "Fields",
                type: "decimal(10,2)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<int>(
                name: "ParentFeedbackId",
                table: "Feedbacks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_ParentFeedbackId",
                table: "Feedbacks",
                column: "ParentFeedbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Feedbacks_ParentFeedbackId",
                table: "Feedbacks",
                column: "ParentFeedbackId",
                principalTable: "Feedbacks",
                principalColumn: "FeedbackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Feedbacks_ParentFeedbackId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_ParentFeedbackId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "fixedPrice",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "ParentFeedbackId",
                table: "Feedbacks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Feedbacks",
                newName: "userID");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Feedbacks",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "FieldId",
                table: "Feedbacks",
                newName: "fieldID");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Feedbacks",
                newName: "createdAt");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "Feedbacks",
                newName: "comment");

            migrationBuilder.RenameColumn(
                name: "FeedbackId",
                table: "Feedbacks",
                newName: "feedbackID");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_UserId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_userID");

            migrationBuilder.RenameIndex(
                name: "IX_Feedbacks_FieldId",
                table: "Feedbacks",
                newName: "IX_Feedbacks_fieldID");

            migrationBuilder.AddColumn<bool>(
                name: "isFixedPrice",
                table: "Fields",
                type: "bit",
                nullable: true,
                defaultValue: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdAt",
                table: "Feedbacks",
                type: "datetime",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
