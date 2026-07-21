using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkServices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId1",
                table: "Payments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ServiceRequestId1",
                table: "Payments",
                column: "ServiceRequestId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_ServiceRequests_ServiceRequestId1",
                table: "Payments",
                column: "ServiceRequestId1",
                principalTable: "ServiceRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_ServiceRequests_ServiceRequestId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ServiceRequestId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId1",
                table: "Payments");
        }
    }
}
