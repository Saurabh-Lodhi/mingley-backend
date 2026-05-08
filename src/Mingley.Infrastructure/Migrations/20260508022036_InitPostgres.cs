using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mingley.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gifts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    CoinCost = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gifts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    DurationDays = table.Column<int>(type: "integer", nullable: false),
                    Features = table.Column<string>(type: "text", nullable: true),
                    IsPopular = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsPremium = table.Column<bool>(type: "boolean", nullable: false),
                    CoinBalance = table.Column<int>(type: "integer", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorSecret = table.Column<string>(type: "text", nullable: true),
                    LastActiveAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsOnline = table.Column<bool>(type: "boolean", nullable: false),
                    OtpCode = table.Column<string>(type: "text", nullable: true),
                    OtpExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OtpPurpose = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BlockerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlockedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks_Users_BlockedUserId",
                        column: x => x.BlockedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blocks_Users_BlockerId",
                        column: x => x.BlockerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CallSessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CallerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uuid", nullable: false),
                    CallType = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    AnsweredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DurationSeconds = table.Column<int>(type: "integer", nullable: true),
                    CoinsDeducted = table.Column<int>(type: "integer", nullable: true),
                    EndReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallSessions_Users_CallerId",
                        column: x => x.CallerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CallSessions_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoinTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Coins = table.Column<int>(type: "integer", nullable: false),
                    Direction = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TransactionType = table.Column<string>(type: "text", nullable: true),
                    ReferenceId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoinTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoinTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UtrId = table.Column<string>(type: "text", nullable: true),
                    ScreenshotUrl = table.Column<string>(type: "text", nullable: true),
                    RequestedCoins = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    User2Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_User1Id",
                        column: x => x.User1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Users_User2Id",
                        column: x => x.User2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    ReferenceId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivacyAgreements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Accepted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyAgreements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivacyAgreements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReporterId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Users_ReportedUserId",
                        column: x => x.ReportedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Users_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Swipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SwiperId = table.Column<Guid>(type: "uuid", nullable: false),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Swipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Swipes_Users_SwiperId",
                        column: x => x.SwiperId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Swipes_Users_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserImages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserInterests",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterests", x => new { x.UserId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_UserInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInterests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Lat = table.Column<double>(type: "double precision", nullable: true),
                    Lng = table.Column<double>(type: "double precision", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLocations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InterestedIn = table.Column<string>(type: "text", nullable: true),
                    MinAge = table.Column<int>(type: "integer", nullable: true),
                    MaxAge = table.Column<int>(type: "integer", nullable: true),
                    MaxDistance = table.Column<int>(type: "integer", nullable: true),
                    RelationshipType = table.Column<string>(type: "text", nullable: true),
                    NearbyOnly = table.Column<bool>(type: "boolean", nullable: true),
                    OnlineOnly = table.Column<bool>(type: "boolean", nullable: true),
                    VerifiedOnly = table.Column<bool>(type: "boolean", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlanId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    AutoRenew = table.Column<bool>(type: "boolean", nullable: false),
                    CancelReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_SubscriptionPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Coins = table.Column<int>(type: "integer", nullable: false),
                    BankOrUpi = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WithdrawalRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chats_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    GiftName = table.Column<string>(type: "text", nullable: true),
                    GiftCost = table.Column<int>(type: "integer", nullable: true),
                    CoinAmount = table.Column<int>(type: "integer", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CoinsDeducted = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Gifts",
                columns: new[] { "Id", "CoinCost", "CreatedAt", "DeletedAt", "Icon", "IsActive", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c0000001-0000-0000-0000-000000000001"), 10, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5303), null, "heart-outline", true, false, "Heart", null },
                    { new Guid("c0000001-0000-0000-0000-000000000002"), 20, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5309), null, "rose-outline", true, false, "Rose", null },
                    { new Guid("c0000001-0000-0000-0000-000000000003"), 50, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5313), null, "gift-outline", true, false, "Gift", null },
                    { new Guid("c0000001-0000-0000-0000-000000000004"), 200, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5317), null, "cafe-outline", true, false, "Coffee Date", null }
                });

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Icon", "IsDeleted", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-000000000001"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4500), null, "musical-notes-outline", false, "Music", null },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4522), null, "airplane-outline", false, "Travel", null },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4527), null, "barbell-outline", false, "Gym", null },
                    { new Guid("a0000001-0000-0000-0000-000000000004"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4531), null, "film-outline", false, "Movies", null },
                    { new Guid("a0000001-0000-0000-0000-000000000005"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4534), null, "book-outline", false, "Reading", null },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4538), null, "restaurant-outline", false, "Cooking", null },
                    { new Guid("a0000001-0000-0000-0000-000000000007"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4570), null, "color-palette-outline", false, "Art", null },
                    { new Guid("a0000001-0000-0000-0000-000000000008"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4575), null, "body-outline", false, "Dancing", null },
                    { new Guid("a0000001-0000-0000-0000-000000000009"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4579), null, "camera-outline", false, "Photography", null },
                    { new Guid("a0000001-0000-0000-0000-000000000010"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4583), null, "body-outline", false, "Yoga", null },
                    { new Guid("a0000001-0000-0000-0000-000000000011"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4587), null, "map-outline", false, "Travelling", null },
                    { new Guid("a0000001-0000-0000-0000-000000000012"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4590), null, "bag-handle-outline", false, "Shopping", null },
                    { new Guid("a0000001-0000-0000-0000-000000000013"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4593), null, "game-controller-outline", false, "Video games", null },
                    { new Guid("a0000001-0000-0000-0000-000000000014"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(4596), null, "walk-outline", false, "Run", null }
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "DurationDays", "Features", "IsActive", "IsDeleted", "IsPopular", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b0000001-0000-0000-0000-000000000001"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5214), null, 30, "[\"Unlimited likes\",\"No ads\",\"See who liked you\",\"Basic filters\"]", true, false, false, "Silver", 299m, null },
                    { new Guid("b0000001-0000-0000-0000-000000000002"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5229), null, 30, "[\"Unlimited likes\",\"No ads\",\"Verified filter\",\"Profile boost\",\"5 coins per message\"]", true, false, true, "Gold", 599m, null },
                    { new Guid("b0000001-0000-0000-0000-000000000003"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5235), null, 30, "[\"All Gold features\",\"Top picks daily\",\"Priority support\",\"Read receipts\",\"Free video calls\"]", true, false, false, "Platinum", 999m, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Avatar", "Bio", "CoinBalance", "CreatedAt", "DateOfBirth", "DeletedAt", "Email", "FullName", "Gender", "IsActive", "IsDeleted", "IsOnline", "IsPremium", "IsVerified", "LastActiveAt", "OtpCode", "OtpExpiry", "OtpPurpose", "PasswordHash", "Phone", "Role", "TwoFactorEnabled", "TwoFactorSecret", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d0000001-0000-0000-0000-000000000001"), "https://randomuser.me/api/portraits/men/1.jpg", null, 0, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5425), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@mingley.app", "Super Admin", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "admin", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000002"), "https://randomuser.me/api/portraits/women/44.jpg", "Love dancing, yoga and cooking 🌺 | Delhi girl", 1500, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5448), new DateTime(1998, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "priya@demo.com", "Priya Sharma", "female", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000003"), "https://randomuser.me/api/portraits/men/32.jpg", "Music lover 🎵 | Traveller | Software Engineer", 2000, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5453), new DateTime(1995, 7, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, "rahul@demo.com", "Rahul Mehta", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000004"), "https://randomuser.me/api/portraits/men/45.jpg", "Fitness enthusiast 💪 | Photographer | Noida", 5000, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5457), new DateTime(1993, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "arjun@demo.com", "Arjun Singh", "male", true, false, false, true, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000005"), "https://randomuser.me/api/portraits/women/68.jpg", "Singer and travel lover 🎵✈️ | Mumbai", 800, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5462), new DateTime(1999, 7, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "neha@demo.com", "Neha Kapoor", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000006"), "https://randomuser.me/api/portraits/men/75.jpg", "Entrepreneur | Coffee addict ☕ | Delhi", 600, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5468), new DateTime(1996, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "vikram@demo.com", "Vikram Nair", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000007"), "https://randomuser.me/api/portraits/women/90.jpg", "Foodie and photographer 📸🍕 | Pune", 400, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5474), new DateTime(2000, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "ankita@demo.com", "Ankita Singh", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000008"), "https://randomuser.me/api/portraits/men/88.jpg", "Gym rat 🏋️ | Cricket fan | Noida", 100, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5479), new DateTime(1997, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, "deepak@demo.com", "Deepak Verma", "male", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000009"), "https://randomuser.me/api/portraits/women/55.jpg", "Fashion lover 👗 | Artist | Hyderabad", 1200, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5499), new DateTime(1999, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "aisha@demo.com", "Aisha Khan", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000010"), "https://randomuser.me/api/portraits/men/60.jpg", "Chef 🍳 | Food blogger | Bengaluru", 300, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5508), new DateTime(1994, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "rohit@demo.com", "Rohit Sharma", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000011"), "https://randomuser.me/api/portraits/women/5.jpg", "Books and coffee", 1811, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6625), new DateTime(1999, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "divya@demo.com", "Divya Menon", "female", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000012"), "https://randomuser.me/api/portraits/women/6.jpg", "Fitness enthusiast | Yoga instructor", 1301, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6648), new DateTime(1998, 10, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "pooja@demo.com", "Pooja Reddy", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000013"), "https://randomuser.me/api/portraits/women/7.jpg", "Chef in the making | Food blogger", 783, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6660), new DateTime(2000, 4, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "shruti@demo.com", "Shruti Verma", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000014"), "https://randomuser.me/api/portraits/women/8.jpg", "Software engineer by day, painter by night", 559, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6665), new DateTime(1999, 3, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "kavya@demo.com", "Kavya Nair", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000015"), "https://randomuser.me/api/portraits/women/9.jpg", "Travel addict | 23 countries visited", 1367, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6673), new DateTime(2002, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, "meera@demo.com", "Meera Joshi", "female", true, false, true, true, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000016"), "https://randomuser.me/api/portraits/women/10.jpg", "Music is my therapy", 295, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6682), new DateTime(1999, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "riya@demo.com", "Riya Gupta", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000017"), "https://randomuser.me/api/portraits/women/11.jpg", "Dog mom | Nature lover", 236, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6690), new DateTime(1997, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, "simran@demo.com", "Simran Kaur", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000018"), "https://randomuser.me/api/portraits/women/12.jpg", "Entrepreneur | Dream chaser", 1163, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6697), new DateTime(1999, 11, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "nandini@demo.com", "Nandini Rao", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000019"), "https://randomuser.me/api/portraits/women/13.jpg", "Dancer | Theatre artist", 1394, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6703), new DateTime(2002, 8, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "trisha@demo.com", "Trisha Das", "female", true, false, true, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000020"), "https://randomuser.me/api/portraits/women/14.jpg", "Voracious reader | Tea enthusiast", 454, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6709), new DateTime(2000, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "sonali@demo.com", "Sonali Patil", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000021"), "https://randomuser.me/api/portraits/women/15.jpg", "Adventure seeker | Hiker", 1036, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6716), new DateTime(2002, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "kritika@demo.com", "Kritika Sharma", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000022"), "https://randomuser.me/api/portraits/women/16.jpg", "Makeup artist | Beauty blogger", 796, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6723), new DateTime(1995, 9, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "pallavi@demo.com", "Pallavi Iyer", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000023"), "https://randomuser.me/api/portraits/women/17.jpg", "Data scientist by profession, artist at heart", 263, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6733), new DateTime(1995, 11, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "ananya@demo.com", "Ananya Bose", "female", true, false, true, true, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000024"), "https://randomuser.me/api/portraits/women/18.jpg", "Meditation and mindfulness", 857, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6743), new DateTime(1997, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "swati@demo.com", "Swati Mishra", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000025"), "https://randomuser.me/api/portraits/women/19.jpg", "Loves cooking for people she cares about", 949, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6750), new DateTime(2000, 8, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "deepika@demo.com", "Deepika Roy", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000026"), "https://randomuser.me/api/portraits/women/20.jpg", "Gym addict | Health coach", 1132, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6838), new DateTime(2002, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "aditi@demo.com", "Aditi Pandey", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000027"), "https://randomuser.me/api/portraits/women/21.jpg", "Love dancing yoga and cooking", 441, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(6849), new DateTime(2002, 6, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "sakshi@demo.com", "Sakshi Yadav", "female", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000028"), "https://randomuser.me/api/portraits/women/22.jpg", "Singer and travel lover", 667, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7287), new DateTime(1998, 5, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "ishita@demo.com", "Ishita Malik", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000029"), "https://randomuser.me/api/portraits/women/23.jpg", "Foodie and photographer", 1300, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7307), new DateTime(1999, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "preeti@demo.com", "Preeti Arora", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000030"), "https://randomuser.me/api/portraits/women/24.jpg", "Fashion lover | Artist", 1043, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7314), new DateTime(1999, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "varsha@demo.com", "Varsha Kumar", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000031"), "https://randomuser.me/api/portraits/women/25.jpg", "Books and coffee", 1005, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7322), new DateTime(2002, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "tanvi@demo.com", "Tanvi Jain", "female", true, false, true, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000032"), "https://randomuser.me/api/portraits/women/26.jpg", "Fitness enthusiast | Yoga instructor", 446, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7328), new DateTime(2002, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "rashmi@demo.com", "Rashmi Pillai", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000033"), "https://randomuser.me/api/portraits/women/27.jpg", "Chef in the making | Food blogger", 851, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7345), new DateTime(1997, 2, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "komal@demo.com", "Komal Shah", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000034"), "https://randomuser.me/api/portraits/women/28.jpg", "Software engineer by day painter at night", 1159, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7352), new DateTime(2002, 1, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, "preethi@demo.com", "Preethi Nair", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000035"), "https://randomuser.me/api/portraits/women/29.jpg", "Travel addict | 23 countries visited", 1312, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7359), new DateTime(2000, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, "lavanya@demo.com", "Lavanya Reddy", "female", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000036"), "https://randomuser.me/api/portraits/women/30.jpg", "Music is my therapy", 1152, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7365), new DateTime(1999, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "gauri@demo.com", "Gauri Desai", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000037"), "https://randomuser.me/api/portraits/women/31.jpg", "Dog mom | Nature lover", 615, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7373), new DateTime(1998, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "megha@demo.com", "Megha Tiwari", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000038"), "https://randomuser.me/api/portraits/women/32.jpg", "Entrepreneur | Dream chaser", 708, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7379), new DateTime(1998, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "sonal@demo.com", "Sonal Mehta", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000039"), "https://randomuser.me/api/portraits/women/33.jpg", "Dancer | Theatre artist", 1428, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7386), new DateTime(1997, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "roshni@demo.com", "Roshni Choudhary", "female", true, false, true, true, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000040"), "https://randomuser.me/api/portraits/women/34.jpg", "Voracious reader | Tea enthusiast", 935, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7391), new DateTime(1996, 10, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "bhavna@demo.com", "Bhavna Saxena", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000041"), "https://randomuser.me/api/portraits/women/35.jpg", "Adventure seeker | Hiker", 1838, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7402), new DateTime(1995, 7, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "nisha@demo.com", "Nisha Bajaj", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000042"), "https://randomuser.me/api/portraits/women/36.jpg", "Makeup artist | Beauty blogger", 981, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7409), new DateTime(1999, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "payal@demo.com", "Payal Ghosh", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000043"), "https://randomuser.me/api/portraits/women/37.jpg", "Data scientist by profession artist at heart", 494, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7419), new DateTime(1998, 9, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "monika@demo.com", "Monika Srivastava", "female", true, false, true, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000044"), "https://randomuser.me/api/portraits/women/38.jpg", "Meditation and mindfulness", 1586, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7426), new DateTime(1995, 9, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "deeksha@demo.com", "Deeksha Singh", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000045"), "https://randomuser.me/api/portraits/women/39.jpg", "Loves cooking for people she cares about", 858, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7432), new DateTime(1998, 3, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, "harpreet@demo.com", "Harpreet Bhatia", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000046"), "https://randomuser.me/api/portraits/women/40.jpg", "Gym addict | Health coach", 689, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7438), new DateTime(1999, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, "amrita@demo.com", "Amrita Chatterjee", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000047"), "https://randomuser.me/api/portraits/women/41.jpg", "Love dancing yoga and cooking", 1330, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7444), new DateTime(1995, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, "sunita@demo.com", "Sunita Pillai", "female", true, false, true, true, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000048"), "https://randomuser.me/api/portraits/women/42.jpg", "Singer and travel lover", 754, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7451), new DateTime(2000, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "poornima@demo.com", "Poornima Rao", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000049"), "https://randomuser.me/api/portraits/women/43.jpg", "Foodie and photographer", 1076, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7462), new DateTime(1997, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "jayashree@demo.com", "Jayashree Nair", "female", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000050"), "https://randomuser.me/api/portraits/women/44.jpg", "Fashion lover | Artist", 206, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7469), new DateTime(2000, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "saranya@demo.com", "Saranya Kumar", "female", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000061"), "https://randomuser.me/api/portraits/men/6.jpg", "Biker | Mountain lover", 3185, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7830), new DateTime(1990, 3, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "amit@demo.com", "Amit Patel", "male", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000062"), "https://randomuser.me/api/portraits/men/7.jpg", "Startup founder | Tech geek", 1626, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7850), new DateTime(1999, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, "karan@demo.com", "Karan Malhotra", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000063"), "https://randomuser.me/api/portraits/men/8.jpg", "Wildlife photographer", 735, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7857), new DateTime(1990, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, "raj@demo.com", "Raj Kapoor", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000064"), "https://randomuser.me/api/portraits/men/9.jpg", "Musician | Weekend hiker", 3931, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7863), new DateTime(1996, 7, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "nikhil@demo.com", "Nikhil Joshi", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000065"), "https://randomuser.me/api/portraits/men/10.jpg", "Doctor | Runner", 3883, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7874), new DateTime(1999, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, "sanjay@demo.com", "Sanjay Gupta", "male", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000066"), "https://randomuser.me/api/portraits/men/11.jpg", "Architect | Design enthusiast", 3183, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7879), new DateTime(1999, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "aditya@demo.com", "Aditya Kumar", "male", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000067"), "https://randomuser.me/api/portraits/men/12.jpg", "Teacher | Book lover", 3925, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7884), new DateTime(1999, 7, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "manish@demo.com", "Manish Tiwari", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000068"), "https://randomuser.me/api/portraits/men/13.jpg", "Pilot | Adventure seeker", 1515, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7889), new DateTime(1995, 2, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "gaurav@demo.com", "Gaurav Reddy", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000069"), "https://randomuser.me/api/portraits/men/14.jpg", "Writer | Coffee connoisseur", 1932, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7895), new DateTime(1997, 9, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "sumit@demo.com", "Sumit Yadav", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000070"), "https://randomuser.me/api/portraits/men/15.jpg", "Cricket player | Fitness freak", 1689, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7902), new DateTime(2000, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "vishal@demo.com", "Vishal Chauhan", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000071"), "https://randomuser.me/api/portraits/men/16.jpg", "Coder by day gamer by night", 4294, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7909), new DateTime(1991, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "ankit@demo.com", "Ankit Sharma", "male", true, false, true, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000072"), "https://randomuser.me/api/portraits/men/17.jpg", "Chef in the making | Food critic", 1767, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7913), new DateTime(1995, 8, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "ravi@demo.com", "Ravi Menon", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000073"), "https://randomuser.me/api/portraits/men/18.jpg", "Yoga and meditation | Philosophy lover", 3209, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7921), new DateTime(1993, 2, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "sandeep@demo.com", "Sandeep Iyer", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000074"), "https://randomuser.me/api/portraits/men/19.jpg", "Marine biologist | Diver", 4732, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7927), new DateTime(1997, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "akash@demo.com", "Akash Singh", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000075"), "https://randomuser.me/api/portraits/men/20.jpg", "Stand-up comedian | Movie buff", 3607, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7932), new DateTime(1992, 11, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "pankaj@demo.com", "Pankaj Bose", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000076"), "https://randomuser.me/api/portraits/men/21.jpg", "Music lover | Traveller", 2389, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7939), new DateTime(1993, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "tarun@demo.com", "Tarun Saxena", "male", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000077"), "https://randomuser.me/api/portraits/men/22.jpg", "Fitness enthusiast | Photographer", 1440, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7949), new DateTime(1997, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "harish@demo.com", "Harish Pillai", "male", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000078"), "https://randomuser.me/api/portraits/men/23.jpg", "Entrepreneur | Coffee addict", 2628, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7955), new DateTime(1996, 10, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, "vivek@demo.com", "Vivek Srivastava", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000079"), "https://randomuser.me/api/portraits/men/24.jpg", "Gym rat | Cricket fan", 414, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7961), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "mohit@demo.com", "Mohit Choudhary", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000080"), "https://randomuser.me/api/portraits/men/25.jpg", "Chef | Food blogger", 3432, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7968), new DateTime(1995, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "ashish@demo.com", "Ashish Bajaj", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000081"), "https://randomuser.me/api/portraits/men/26.jpg", "Biker | Mountain lover", 3362, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7976), new DateTime(1991, 6, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, "praveen@demo.com", "Praveen Ghosh", "male", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000082"), "https://randomuser.me/api/portraits/men/27.jpg", "Startup founder | Tech geek", 3101, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7988), new DateTime(1990, 7, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "suresh@demo.com", "Suresh Nair", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000083"), "https://randomuser.me/api/portraits/men/28.jpg", "Wildlife photographer", 1972, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(7995), new DateTime(2000, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, "dinesh@demo.com", "Dinesh Patil", "male", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000084"), "https://randomuser.me/api/portraits/men/29.jpg", "Musician | Weekend hiker", 4773, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8003), new DateTime(2000, 8, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "vinod@demo.com", "Vinod Kulkarni", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000085"), "https://randomuser.me/api/portraits/men/30.jpg", "Doctor | Runner", 1117, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8010), new DateTime(1999, 7, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "ramesh@demo.com", "Ramesh Rao", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000086"), "https://randomuser.me/api/portraits/men/31.jpg", "Architect | Design enthusiast", 4010, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8121), new DateTime(1998, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, "ajay@demo.com", "Ajay Desai", "male", true, false, true, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000087"), "https://randomuser.me/api/portraits/men/32.jpg", "Teacher | Book lover", 4672, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8130), new DateTime(1995, 9, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "vijay@demo.com", "Vijay Krishnan", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000088"), "https://randomuser.me/api/portraits/men/33.jpg", "Pilot | Adventure seeker", 3133, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8137), new DateTime(1992, 9, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "manoj@demo.com", "Manoj Shah", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000089"), "https://randomuser.me/api/portraits/men/34.jpg", "Writer | Coffee connoisseur", 3506, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8147), new DateTime(1998, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "naresh@demo.com", "Naresh Bansal", "male", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000090"), "https://randomuser.me/api/portraits/men/35.jpg", "Cricket player | Fitness freak", 4031, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8154), new DateTime(1994, 5, 21, 0, 0, 0, 0, DateTimeKind.Utc), null, "girish@demo.com", "Girish Suresh", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000091"), "https://randomuser.me/api/portraits/men/36.jpg", "Coder by day gamer by night", 846, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8163), new DateTime(1996, 8, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "kartik@demo.com", "Kartik Arora", "male", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000092"), "https://randomuser.me/api/portraits/men/37.jpg", "Chef in the making | Food critic", 2916, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8170), new DateTime(1993, 10, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "nitin@demo.com", "Nitin Mishra", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000093"), "https://randomuser.me/api/portraits/men/38.jpg", "Yoga and meditation | Philosophy lover", 4750, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8176), new DateTime(1990, 6, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "satish@demo.com", "Satish Roy", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000094"), "https://randomuser.me/api/portraits/men/39.jpg", "Marine biologist | Diver", 1086, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8182), new DateTime(1992, 10, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "rakesh@demo.com", "Rakesh Pandey", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000095"), "https://randomuser.me/api/portraits/men/40.jpg", "Stand-up comedian | Movie buff", 404, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8190), new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "pradeep@demo.com", "Pradeep Malik", "male", true, false, false, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000096"), "https://randomuser.me/api/portraits/men/41.jpg", "Music lover | Traveller", 501, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8197), new DateTime(2000, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "sachin@demo.com", "Sachin Bhatia", "male", true, false, true, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000097"), "https://randomuser.me/api/portraits/men/42.jpg", "Fitness enthusiast | Photographer", 4331, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8207), new DateTime(2000, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "devendra@demo.com", "Devendra Chatterjee", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000098"), "https://randomuser.me/api/portraits/men/43.jpg", "Entrepreneur | Coffee addict", 2496, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8214), new DateTime(2000, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "srinivas@demo.com", "Srinivas Rajan", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000099"), "https://randomuser.me/api/portraits/men/44.jpg", "Gym rat | Cricket fan", 1887, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8219), new DateTime(2000, 2, 22, 0, 0, 0, 0, DateTimeKind.Utc), null, "krishnan@demo.com", "Krishnan Pillai", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000100"), "https://randomuser.me/api/portraits/men/45.jpg", "Chef | Food blogger", 3735, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8231), new DateTime(1994, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "venkat@demo.com", "Venkat Rao", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000101"), "https://randomuser.me/api/portraits/men/46.jpg", "Biker | Mountain lover", 656, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8236), new DateTime(1998, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "sunil@demo.com", "Sunil Jain", "male", true, false, true, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000102"), "https://randomuser.me/api/portraits/men/47.jpg", "Startup founder | Tech geek", 3040, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8242), new DateTime(1995, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), null, "rajesh@demo.com", "Rajesh Kumar", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000103"), "https://randomuser.me/api/portraits/men/48.jpg", "Wildlife photographer", 4416, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8249), new DateTime(1994, 10, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "hemant@demo.com", "Hemant Singh", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000104"), "https://randomuser.me/api/portraits/men/49.jpg", "Musician | Weekend hiker", 721, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8257), new DateTime(1994, 7, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, "bharat@demo.com", "Bharat Mehta", "male", true, false, false, true, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000105"), "https://randomuser.me/api/portraits/men/50.jpg", "Doctor | Runner", 1274, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8266), new DateTime(1997, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "alok@demo.com", "Alok Verma", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000106"), "https://randomuser.me/api/portraits/men/51.jpg", "Architect | Design lover", 2100, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8273), new DateTime(1993, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "deepesh@demo.com", "Deepesh Nair", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000107"), "https://randomuser.me/api/portraits/men/52.jpg", "Coder by day gamer by night", 980, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8278), new DateTime(1998, 8, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "prashant@demo.com", "Prashant Singh", "male", true, false, true, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000108"), "https://randomuser.me/api/portraits/men/53.jpg", "Chef in the making | Foodie", 1560, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8288), new DateTime(1996, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "shyam@demo.com", "Shyam Verma", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000109"), "https://randomuser.me/api/portraits/men/54.jpg", "Yoga and meditation | Philosophy lover", 2900, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8297), new DateTime(1994, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "kapil@demo.com", "Kapil Gupta", "male", true, false, false, false, true, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null },
                    { new Guid("d0000001-0000-0000-0000-000000000110"), "https://randomuser.me/api/portraits/men/55.jpg", "Marine biologist | Diver", 3200, new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(8305), new DateTime(1997, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, "sanket@demo.com", "Sanket Kulkarni", "male", true, false, true, false, false, null, null, null, null, "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG", null, "user", false, null, null }
                });

            migrationBuilder.InsertData(
                table: "DepositRequests",
                columns: new[] { "Id", "AdminNote", "CreatedAt", "DeletedAt", "IsDeleted", "RequestedCoins", "ScreenshotUrl", "Status", "UpdatedAt", "UserId", "UtrId" },
                values: new object[,]
                {
                    { new Guid("e1000001-0000-0000-0000-000000000001"), null, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 1000, null, "pending", null, new Guid("d0000001-0000-0000-0000-000000000003"), "UTR123456789" },
                    { new Guid("e1000001-0000-0000-0000-000000000003"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, 2000, null, "approved", null, new Guid("d0000001-0000-0000-0000-000000000004"), "UTR987654321" }
                });

            migrationBuilder.InsertData(
                table: "Matches",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsActive", "IsDeleted", "UpdatedAt", "User1Id", "User2Id" },
                values: new object[] { new Guid("a1000001-0000-0000-0000-000000000001"), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, true, false, null, new Guid("d0000001-0000-0000-0000-000000000003"), new Guid("d0000001-0000-0000-0000-000000000002") });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "Body", "CreatedAt", "DeletedAt", "IsDeleted", "IsRead", "ReferenceId", "Title", "Type", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("f1000001-0000-0000-0000-000000000001"), "You matched with Priya Sharma!", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, false, null, "New Match! 🎉", "match", null, new Guid("d0000001-0000-0000-0000-000000000003") },
                    { new Guid("f1000001-0000-0000-0000-000000000002"), "You matched with Rahul Mehta!", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, true, null, "New Match! 🎉", "match", null, new Guid("d0000001-0000-0000-0000-000000000002") },
                    { new Guid("f1000001-0000-0000-0000-000000000003"), "Priya sent you a message", new DateTime(2024, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc), null, false, false, null, "New Message 💬", "message", null, new Guid("d0000001-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "Swipes",
                columns: new[] { "Id", "Action", "CreatedAt", "DeletedAt", "IsDeleted", "SwiperId", "TargetId", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b1000001-0000-0000-0000-000000000001"), "like", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("d0000001-0000-0000-0000-000000000003"), new Guid("d0000001-0000-0000-0000-000000000002"), null },
                    { new Guid("b1000001-0000-0000-0000-000000000002"), "like", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("d0000001-0000-0000-0000-000000000002"), new Guid("d0000001-0000-0000-0000-000000000003"), null }
                });

            migrationBuilder.InsertData(
                table: "UserInterests",
                columns: new[] { "InterestId", "UserId" },
                values: new object[,]
                {
                    { new Guid("a0000001-0000-0000-0000-000000000001"), new Guid("d0000001-0000-0000-0000-000000000002") },
                    { new Guid("a0000001-0000-0000-0000-000000000008"), new Guid("d0000001-0000-0000-0000-000000000002") },
                    { new Guid("a0000001-0000-0000-0000-000000000010"), new Guid("d0000001-0000-0000-0000-000000000002") },
                    { new Guid("a0000001-0000-0000-0000-000000000001"), new Guid("d0000001-0000-0000-0000-000000000003") },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), new Guid("d0000001-0000-0000-0000-000000000003") },
                    { new Guid("a0000001-0000-0000-0000-000000000009"), new Guid("d0000001-0000-0000-0000-000000000003") },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), new Guid("d0000001-0000-0000-0000-000000000004") },
                    { new Guid("a0000001-0000-0000-0000-000000000009"), new Guid("d0000001-0000-0000-0000-000000000004") },
                    { new Guid("a0000001-0000-0000-0000-000000000001"), new Guid("d0000001-0000-0000-0000-000000000005") },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), new Guid("d0000001-0000-0000-0000-000000000005") },
                    { new Guid("a0000001-0000-0000-0000-000000000002"), new Guid("d0000001-0000-0000-0000-000000000006") },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), new Guid("d0000001-0000-0000-0000-000000000007") },
                    { new Guid("a0000001-0000-0000-0000-000000000009"), new Guid("d0000001-0000-0000-0000-000000000007") },
                    { new Guid("a0000001-0000-0000-0000-000000000003"), new Guid("d0000001-0000-0000-0000-000000000008") },
                    { new Guid("a0000001-0000-0000-0000-000000000007"), new Guid("d0000001-0000-0000-0000-000000000009") },
                    { new Guid("a0000001-0000-0000-0000-000000000006"), new Guid("d0000001-0000-0000-0000-000000000010") }
                });

            migrationBuilder.InsertData(
                table: "UserLocations",
                columns: new[] { "Id", "City", "Country", "CreatedAt", "DeletedAt", "IsDeleted", "Lat", "Lng", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("b2000001-0000-0000-0000-000000000001"), "Delhi", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5834), null, false, 28.614000000000001, 77.209000000000003, null, new Guid("d0000001-0000-0000-0000-000000000002") },
                    { new Guid("b2000001-0000-0000-0000-000000000002"), "Noida", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5845), null, false, 28.535, 77.391000000000005, null, new Guid("d0000001-0000-0000-0000-000000000003") },
                    { new Guid("b2000001-0000-0000-0000-000000000003"), "Gurgaon", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5849), null, false, 28.459, 77.025999999999996, null, new Guid("d0000001-0000-0000-0000-000000000004") },
                    { new Guid("b2000001-0000-0000-0000-000000000004"), "Mumbai", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5853), null, false, 19.076000000000001, 72.876999999999995, null, new Guid("d0000001-0000-0000-0000-000000000005") },
                    { new Guid("b2000001-0000-0000-0000-000000000005"), "Pune", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5858), null, false, 18.52, 73.855999999999995, null, new Guid("d0000001-0000-0000-0000-000000000007") },
                    { new Guid("b2000001-0000-0000-0000-000000000006"), "Delhi", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5862), null, false, 28.699999999999999, 77.099999999999994, null, new Guid("d0000001-0000-0000-0000-000000000006") },
                    { new Guid("b2000001-0000-0000-0000-000000000007"), "Noida", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5869), null, false, 28.539999999999999, 77.400000000000006, null, new Guid("d0000001-0000-0000-0000-000000000008") },
                    { new Guid("b2000001-0000-0000-0000-000000000008"), "Hyderabad", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5872), null, false, 17.385000000000002, 78.486000000000004, null, new Guid("d0000001-0000-0000-0000-000000000009") },
                    { new Guid("b2000001-0000-0000-0000-000000000009"), "Bengaluru", "India", new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5876), null, false, 12.972, 77.593999999999994, null, new Guid("d0000001-0000-0000-0000-000000000010") }
                });

            migrationBuilder.InsertData(
                table: "UserPreferences",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "InterestedIn", "IsDeleted", "Location", "MaxAge", "MaxDistance", "MinAge", "NearbyOnly", "OnlineOnly", "RelationshipType", "UpdatedAt", "UserId", "VerifiedOnly" },
                values: new object[,]
                {
                    { new Guid("a2000001-0000-0000-0000-000000000001"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5708), null, "boys", false, null, 35, 100, 22, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000002"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000002"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5722), null, "girls", false, null, 30, 100, 20, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000003"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000003"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5726), null, "girls", false, null, 32, 100, 21, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000004"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000004"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5730), null, "boys", false, null, 33, 100, 23, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000005"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000005"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5734), null, "boys", false, null, 34, 100, 24, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000007"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000006"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5739), null, "girls", false, null, 30, 100, 21, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000006"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000007"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5742), null, "girls", false, null, 28, 100, 20, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000008"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000008"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5751), null, "boys", false, null, 32, 100, 22, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000009"), false },
                    { new Guid("a2000001-0000-0000-0000-000000000009"), new DateTime(2026, 5, 8, 2, 20, 34, 844, DateTimeKind.Utc).AddTicks(5755), null, "girls", false, null, 30, 100, 20, false, false, "both", null, new Guid("d0000001-0000-0000-0000-000000000010"), false }
                });

            migrationBuilder.InsertData(
                table: "WithdrawalRequests",
                columns: new[] { "Id", "AdminNote", "BankOrUpi", "Coins", "CreatedAt", "DeletedAt", "IsDeleted", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { new Guid("e1000001-0000-0000-0000-000000000002"), null, "priya@paytm", 700, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "pending", null, new Guid("d0000001-0000-0000-0000-000000000002") },
                    { new Guid("e1000001-0000-0000-0000-000000000004"), null, "neha@gpay", 400, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, false, "approved", null, new Guid("d0000001-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "IsDeleted", "MatchId", "UpdatedAt" },
                values: new object[] { new Guid("a1000001-0000-0000-0000-000000000002"), new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, false, new Guid("a1000001-0000-0000-0000-000000000001"), null });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "ChatId", "CoinAmount", "CoinsDeducted", "CreatedAt", "DeletedAt", "GiftCost", "GiftName", "ImageUrl", "IsDeleted", "ReadAt", "SenderId", "Text", "Type", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("c1000001-0000-0000-0000-000000000001"), new Guid("a1000001-0000-0000-0000-000000000002"), null, 10, new DateTime(2024, 1, 2, 0, 30, 0, 0, DateTimeKind.Utc), null, null, null, null, false, new DateTime(2024, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc), new Guid("d0000001-0000-0000-0000-000000000003"), "Hey Priya! We matched 🎉 How are you?", "text", null },
                    { new Guid("c1000001-0000-0000-0000-000000000002"), new Guid("a1000001-0000-0000-0000-000000000002"), null, null, new DateTime(2024, 1, 2, 1, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, new DateTime(2024, 1, 2, 2, 0, 0, 0, DateTimeKind.Utc), new Guid("d0000001-0000-0000-0000-000000000002"), "Hi Rahul! I'm great, thanks! How about you? 😊", "text", null },
                    { new Guid("c1000001-0000-0000-0000-000000000003"), new Guid("a1000001-0000-0000-0000-000000000002"), null, 10, new DateTime(2024, 1, 2, 2, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, new DateTime(2024, 1, 2, 3, 0, 0, 0, DateTimeKind.Utc), new Guid("d0000001-0000-0000-0000-000000000003"), "Doing well! I saw you love dancing 💃", "text", null },
                    { new Guid("c1000001-0000-0000-0000-000000000004"), new Guid("a1000001-0000-0000-0000-000000000002"), null, null, new DateTime(2024, 1, 2, 3, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, new Guid("d0000001-0000-0000-0000-000000000002"), "Yes! I've been dancing since I was 8 🎵", "text", null },
                    { new Guid("c1000001-0000-0000-0000-000000000005"), new Guid("a1000001-0000-0000-0000-000000000002"), null, 10, new DateTime(2024, 1, 2, 4, 0, 0, 0, DateTimeKind.Utc), null, null, null, null, false, null, new Guid("d0000001-0000-0000-0000-000000000003"), "That's amazing! I play guitar 🎸", "text", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_BlockedUserId",
                table: "Blocks",
                column: "BlockedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_BlockerId_BlockedUserId",
                table: "Blocks",
                columns: new[] { "BlockerId", "BlockedUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CallSessions_CallerId",
                table: "CallSessions",
                column: "CallerId");

            migrationBuilder.CreateIndex(
                name: "IX_CallSessions_ReceiverId",
                table: "CallSessions",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_MatchId",
                table: "Chats",
                column: "MatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoinTransactions_UserId",
                table: "CoinTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositRequests_UserId",
                table: "DepositRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_User1Id",
                table: "Matches",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_User2Id",
                table: "Matches",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyAgreements_UserId",
                table: "PrivacyAgreements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportedUserId",
                table: "Reports",
                column: "ReportedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Swipes_SwiperId_TargetId",
                table: "Swipes",
                columns: new[] { "SwiperId", "TargetId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Swipes_TargetId",
                table: "Swipes",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserImages_UserId",
                table: "UserImages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_InterestId",
                table: "UserInterests",
                column: "InterestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLocations_UserId",
                table: "UserLocations",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Phone",
                table: "Users",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_PlanId",
                table: "UserSubscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscriptions_UserId",
                table: "UserSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_UserId",
                table: "WithdrawalRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "CallSessions");

            migrationBuilder.DropTable(
                name: "CoinTransactions");

            migrationBuilder.DropTable(
                name: "DepositRequests");

            migrationBuilder.DropTable(
                name: "Gifts");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PrivacyAgreements");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Swipes");

            migrationBuilder.DropTable(
                name: "UserImages");

            migrationBuilder.DropTable(
                name: "UserInterests");

            migrationBuilder.DropTable(
                name: "UserLocations");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "UserSubscriptions");

            migrationBuilder.DropTable(
                name: "WithdrawalRequests");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
