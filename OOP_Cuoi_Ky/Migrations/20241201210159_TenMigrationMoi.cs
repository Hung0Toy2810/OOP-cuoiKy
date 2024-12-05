using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OOP_Cuoi_Ky.Migrations
{
    /// <inheritdoc />
    public partial class TenMigrationMoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    MAKH = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiemTichLuy = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.MAKH);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    MANV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChucVu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.MANV);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    MASP = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TinhTrang = table.Column<int>(type: "int", nullable: false),
                    HinhAnh = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.MASP);
                });

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    MAKM = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGianHL = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DieuKien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MucGiamGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MAKH = table.Column<int>(type: "int", nullable: false),
                    DaDung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.MAKM);
                    table.ForeignKey(
                        name: "FK_Promotions_Customers_MAKH",
                        column: x => x.MAKH,
                        principalTable: "Customers",
                        principalColumn: "MAKH",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    MAHD = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MAKH = table.Column<int>(type: "int", nullable: false),
                    MANV = table.Column<int>(type: "int", nullable: false),
                    ThoiGianTT = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HinhThucTT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoTienNhan = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.MAHD);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_MAKH",
                        column: x => x.MAKH,
                        principalTable: "Customers",
                        principalColumn: "MAKH",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_Employees_MANV",
                        column: x => x.MANV,
                        principalTable: "Employees",
                        principalColumn: "MANV",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    MAHD = table.Column<int>(type: "int", nullable: false),
                    MASP = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => new { x.MAHD, x.MASP });
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_MAHD",
                        column: x => x.MAHD,
                        principalTable: "Invoices",
                        principalColumn: "MAHD",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Products_MASP",
                        column: x => x.MASP,
                        principalTable: "Products",
                        principalColumn: "MASP",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_MASP",
                table: "InvoiceDetails",
                column: "MASP");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_MAKH",
                table: "Invoices",
                column: "MAKH");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_MANV",
                table: "Invoices",
                column: "MANV");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_MAKH",
                table: "Promotions",
                column: "MAKH");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
