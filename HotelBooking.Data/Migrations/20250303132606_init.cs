using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelBooking.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hotel",
                columns: table => new
                {
                    HotelCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HotelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumRooms = table.Column<int>(type: "int", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StarRating = table.Column<int>(type: "int", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hotel", x => x.HotelCode);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "room_types",
                columns: table => new
                {
                    RoomType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultRoomPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_types", x => x.RoomType);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuestTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DOB = table.Column<DateOnly>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PassportNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    RoomNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HotelCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoomTypeNavigationRoomType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room", x => x.RoomNo);
                    table.ForeignKey(
                        name: "FK_room_hotel_HotelCode",
                        column: x => x.HotelCode,
                        principalTable: "hotel",
                        principalColumn: "HotelCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_room_room_types_RoomTypeNavigationRoomType",
                        column: x => x.RoomTypeNavigationRoomType,
                        principalTable: "room_types",
                        principalColumn: "RoomType",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jwts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    token_hash_value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expired_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    fcm_token = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jwts", x => x.id);
                    table.ForeignKey(
                        name: "FK_jwts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    notifi_content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    is_accept = table.Column<bool>(type: "bit", nullable: false),
                    from_user_notifi_id = table.Column<int>(type: "int", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_notifications_users_from_user_notifi_id",
                        column: x => x.from_user_notifi_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_actived = table.Column<bool>(type: "bit", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    job_id = table.Column<int>(type: "int", nullable: true),
                    file_id = table.Column<int>(type: "int", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_posts_files_file_id",
                        column: x => x.file_id,
                        principalTable: "files",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_posts_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    BookingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HotelCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GuestID = table.Column<int>(type: "int", nullable: false),
                    RoomNo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstArrivalTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstDepartureTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumAdults = table.Column<int>(type: "int", nullable: false),
                    NumChildren = table.Column<int>(type: "int", nullable: false),
                    SpecialReq = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookingStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HotelModelHotelCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserModelId = table.Column<int>(type: "int", nullable: true),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_booking", x => x.BookingID);
                    table.ForeignKey(
                        name: "FK_booking_hotel_HotelCode",
                        column: x => x.HotelCode,
                        principalTable: "hotel",
                        principalColumn: "HotelCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_booking_hotel_HotelModelHotelCode",
                        column: x => x.HotelModelHotelCode,
                        principalTable: "hotel",
                        principalColumn: "HotelCode");
                    table.ForeignKey(
                        name: "FK_booking_room_RoomNo",
                        column: x => x.RoomNo,
                        principalTable: "room",
                        principalColumn: "RoomNo");
                    table.ForeignKey(
                        name: "FK_booking_users_GuestID",
                        column: x => x.GuestID,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_booking_users_UserModelId",
                        column: x => x.UserModelId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    InvoiceNo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingID = table.Column<int>(type: "int", nullable: false),
                    GuestID = table.Column<int>(type: "int", nullable: false),
                    RoomCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomService = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RestaurantCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BarCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MiscCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IfLateCheckout = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    changed_by = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bill", x => x.InvoiceNo);
                    table.ForeignKey(
                        name: "FK_bill_booking_BookingID",
                        column: x => x.BookingID,
                        principalTable: "booking",
                        principalColumn: "BookingID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bill_users_GuestID",
                        column: x => x.GuestID,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bill_BookingID",
                table: "bill",
                column: "BookingID");

            migrationBuilder.CreateIndex(
                name: "IX_bill_GuestID",
                table: "bill",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_booking_GuestID",
                table: "booking",
                column: "GuestID");

            migrationBuilder.CreateIndex(
                name: "IX_booking_HotelCode",
                table: "booking",
                column: "HotelCode");

            migrationBuilder.CreateIndex(
                name: "IX_booking_HotelModelHotelCode",
                table: "booking",
                column: "HotelModelHotelCode");

            migrationBuilder.CreateIndex(
                name: "IX_booking_RoomNo",
                table: "booking",
                column: "RoomNo");

            migrationBuilder.CreateIndex(
                name: "IX_booking_UserModelId",
                table: "booking",
                column: "UserModelId");

            migrationBuilder.CreateIndex(
                name: "IX_jwts_user_id",
                table: "jwts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_from_user_notifi_id",
                table: "notifications",
                column: "from_user_notifi_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_file_id",
                table: "posts",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "IX_posts_user_id",
                table: "posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_room_HotelCode",
                table: "room",
                column: "HotelCode");

            migrationBuilder.CreateIndex(
                name: "IX_room_RoomTypeNavigationRoomType",
                table: "room",
                column: "RoomTypeNavigationRoomType");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_user_id",
                table: "user_roles",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_Email",
                table: "users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bill");

            migrationBuilder.DropTable(
                name: "jwts");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "hotel");

            migrationBuilder.DropTable(
                name: "room_types");
        }
    }
}
