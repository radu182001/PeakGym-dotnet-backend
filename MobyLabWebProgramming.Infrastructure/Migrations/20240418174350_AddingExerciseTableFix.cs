using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    public partial class AddingExerciseTableFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainerId",
                table: "Exercise",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Exercise_TrainerId",
                table: "Exercise",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_User_TrainerId",
                table: "Exercise",
                column: "TrainerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_User_TrainerId",
                table: "Exercise");

            migrationBuilder.DropIndex(
                name: "IX_Exercise_TrainerId",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Exercise");
        }
    }
}
