using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace apbd_project.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Offer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "softwareProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_softwareProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "businessClients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KrsNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_businessClients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_businessClients_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "individualClients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pesel = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_individualClients", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_individualClients_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoftwareProductId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SignedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contracts_clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_contracts_softwareProducts_SoftwareProductId",
                        column: x => x.SoftwareProductId,
                        principalTable: "softwareProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "oneTimePurchase",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SupportEndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_oneTimePurchase", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_oneTimePurchase_contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "clients",
                columns: new[] { "Id", "Address", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "ul. Klonowa 2, 00-001 Warszawa", "client1@gmail.com", "123456789" },
                    { 2, "ul. Brzozowa 3, 00-002 Warszawa", "client2@gmail.com", "987654321" }
                });

            migrationBuilder.InsertData(
                table: "discounts",
                columns: new[] { "Id", "Amount", "EndDate", "Name", "Offer", "StartDate" },
                values: new object[] { 1, 15, new DateTime(2024, 7, 5, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(593), "Some discount", "Some offer", new DateTime(2024, 6, 28, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(534) });

            migrationBuilder.InsertData(
                table: "softwareProducts",
                columns: new[] { "Id", "Category", "Description", "Name", "Type", "Version" },
                values: new object[,]
                {
                    { 1, "Finance", "Some description", "Software product 1", "Subscription", "1.0" },
                    { 2, "Education", "Description 2", "Product 2", "One-time purchase", "2.0" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "HashedPassword", "RefreshToken", "RefreshTokenExpiryTime", "Role", "Username" },
                values: new object[,]
                {
                    { 1, "$2a$11$WxeIQbfKhbGh4GKO6dYwzeQUISZKv36mHEo1yXdRlNfBF7QyU8asy", null, null, "admin", "admin" },
                    { 2, "$2a$11$IoM9kEzrs53xTBAslNrM2ek3leKViw31r1x2ezK89MHfX838xzyde", null, null, "user", "user" }
                });

            migrationBuilder.InsertData(
                table: "businessClients",
                columns: new[] { "ClientId", "CompanyName", "KrsNumber" },
                values: new object[] { 2, "Firma XYZ", "9876543210" });

            migrationBuilder.InsertData(
                table: "contracts",
                columns: new[] { "Id", "ClientId", "Price", "SignedDate", "SoftwareProductId", "StartDate" },
                values: new object[] { 1, 1, 10000m, new DateTime(2024, 6, 28, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(1326), 1, new DateTime(2024, 6, 28, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(1313) });

            migrationBuilder.InsertData(
                table: "individualClients",
                columns: new[] { "ClientId", "IsActive", "Name", "Pesel", "Surname" },
                values: new object[] { 1, true, "Jan", "12345678901", "Kowalski" });

            migrationBuilder.InsertData(
                table: "oneTimePurchase",
                columns: new[] { "ContractId", "EndDate", "SupportEndDate", "Version" },
                values: new object[] { 1, new DateTime(2024, 7, 12, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(1380), new DateTime(2025, 6, 28, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(1391), "1.0" });

            migrationBuilder.InsertData(
                table: "payments",
                columns: new[] { "Id", "Amount", "ContractId", "PaymentDate" },
                values: new object[] { 1, 10000m, 1, new DateTime(2024, 6, 28, 8, 24, 8, 995, DateTimeKind.Local).AddTicks(1711) });

            migrationBuilder.CreateIndex(
                name: "IX_contracts_ClientId",
                table: "contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_contracts_SoftwareProductId",
                table: "contracts",
                column: "SoftwareProductId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_ContractId",
                table: "payments",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "businessClients");

            migrationBuilder.DropTable(
                name: "discounts");

            migrationBuilder.DropTable(
                name: "individualClients");

            migrationBuilder.DropTable(
                name: "oneTimePurchase");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "contracts");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "softwareProducts");
        }
    }
}
