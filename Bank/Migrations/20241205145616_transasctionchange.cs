using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Migrations
{
    /// <inheritdoc />
    public partial class transasctionchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "c984adb7-1127-45ab-83ca-60632356ee43" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c984adb7-1127-45ab-83ca-60632356ee43");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "transactionType",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LoanRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Balance", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "e74149df-1a71-4e5b-900f-3016312eb45b", 0, 0m, "9d21bd1d-9f60-43df-ac45-58ffb5104909", "admin@gmail.com", true, "", "", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEF7vjPqO0LEofZlMvfVMvm9N9ke9OH5SV0zC1RomYF+kKFGna+5UhgImgambg4laew==", null, false, "ec253c5f-3e34-49b3-9ea4-6a28fb66427b", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "e74149df-1a71-4e5b-900f-3016312eb45b" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecipientId",
                table: "Transactions",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_AspNetUsers_RecipientId",
                table: "Transactions",
                column: "RecipientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_AspNetUsers_RecipientId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecipientId",
                table: "Transactions");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1", "e74149df-1a71-4e5b-900f-3016312eb45b" });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "e74149df-1a71-4e5b-900f-3016312eb45b");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "transactionType",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LoanRequests");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Balance", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "c984adb7-1127-45ab-83ca-60632356ee43", 0, 0m, "dfadd562-7967-4b3c-825a-9cf02380b769", "admin@gmail.com", true, "", "", false, null, "ADMIN@GMAIL.COM", "ADMIN", "AQAAAAIAAYagAAAAEOSSOUuYFEQ84UGovTpnmSWmSr8GBPNHe9W+dEvxxkRsPVUFhPYDmFCCP7RXbVoDuw==", null, false, "9bfa5ffb-535a-419d-919d-51785c1b2a05", false, "admin" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "1", "c984adb7-1127-45ab-83ca-60632356ee43" });
        }
    }
}
