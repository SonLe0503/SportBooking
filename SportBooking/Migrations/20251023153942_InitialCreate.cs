using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportBooking.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "USER"),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__CB9A1CDF96F8506E", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    fieldID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fieldName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ownerID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Fields__F0AC27FE11A5C9C1", x => x.fieldID);
                    table.ForeignKey(
                        name: "FK__Fields__ownerID__3C69FB99",
                        column: x => x.ownerID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    bookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: true),
                    fieldID = table.Column<int>(type: "int", nullable: true),
                    bookingDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    startTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    endTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    totalPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Bookings__C6D03BED0688CCEF", x => x.bookingID);
                    table.ForeignKey(
                        name: "FK__Bookings__fieldI__412EB0B6",
                        column: x => x.fieldID,
                        principalTable: "Fields",
                        principalColumn: "fieldID");
                    table.ForeignKey(
                        name: "FK__Bookings__userID__403A8C7D",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    feedbackID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userID = table.Column<int>(type: "int", nullable: true),
                    fieldID = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__2613FDC4EC80AAA4", x => x.feedbackID);
                    table.ForeignKey(
                        name: "FK__Feedbacks__field__46E78A0C",
                        column: x => x.fieldID,
                        principalTable: "Fields",
                        principalColumn: "fieldID");
                    table.ForeignKey(
                        name: "FK__Feedbacks__userI__45F365D3",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageField",
                columns: table => new
                {
                    imageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    fieldID = table.Column<int>(type: "int", nullable: false),
                    imageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ImageFie__336E9B757CB1FE15", x => x.imageID);
                    table.ForeignKey(
                        name: "FK__ImageFiel__field__59FA5E80",
                        column: x => x.fieldID,
                        principalTable: "Fields",
                        principalColumn: "fieldID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    paymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookingID = table.Column<int>(type: "int", nullable: true),
                    paymentDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    paymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "Completed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__A0D9EFA69735E67D", x => x.paymentID);
                    table.ForeignKey(
                        name: "FK__Payments__bookin__4BAC3F29",
                        column: x => x.bookingID,
                        principalTable: "Bookings",
                        principalColumn: "bookingID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_fieldID",
                table: "Bookings",
                column: "fieldID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_userID",
                table: "Bookings",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_fieldID",
                table: "Feedbacks",
                column: "fieldID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_userID",
                table: "Feedbacks",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_Fields_ownerID",
                table: "Fields",
                column: "ownerID");

            migrationBuilder.CreateIndex(
                name: "IX_ImageField_fieldID",
                table: "ImageField",
                column: "fieldID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_bookingID",
                table: "Payments",
                column: "bookingID");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__F3DBC572E4679EEF",
                table: "Users",
                column: "username",
                unique: true,
                filter: "[username] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "ImageField");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
