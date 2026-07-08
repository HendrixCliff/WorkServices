using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkServices.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEstimatedCostToServiceRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "JobAssignments");

            migrationBuilder.AddColumn<decimal>(
                name: "EstimatedCost",
                table: "ServiceRequests",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ServiceRequestId",
                table: "Reviews",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Payments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "JobAssignments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedCost",
                table: "ServiceRequests");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "JobAssignments");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "JobAssignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
