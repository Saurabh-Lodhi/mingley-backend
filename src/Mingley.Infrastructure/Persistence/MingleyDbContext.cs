using Microsoft.EntityFrameworkCore;
using Mingley.Domain.Entities;

namespace Mingley.Infrastructure.Persistence;

public class MingleyDbContext : DbContext
{
    // ── Coin economy constants (match frontend exactly) ─────────────────
    public const int AudioCallCoinPerMin = 10;
    public const int VideoCallCoinPerMin = 100;
    public const int GiftRoseCost = 20;
    public const int GiftHeartCost = 10;
    public const int GiftCoffeeCost = 200;
    public const int GiftGenericCost = 50;
    public const int VerificationBonus = 50;
    public const double FemaleWithdrawPct = 0.70;
    public const int MinCoinPurchase = 1000; // ₹1000 = 1000 coins

    public MingleyDbContext(DbContextOptions<MingleyDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserLocation> UserLocations => Set<UserLocation>();
    public DbSet<UserPreference> UserPreferences => Set<UserPreference>();
    public DbSet<UserImage> UserImages => Set<UserImage>();
    public DbSet<Interest> Interests => Set<Interest>();
    public DbSet<UserInterest> UserInterests => Set<UserInterest>();
    public DbSet<Swipe> Swipes => Set<Swipe>();
    public DbSet<Match> Matches => Set<Match>();
    public DbSet<Chat> Chats => Set<Chat>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<SubscriptionPlan> SubscriptionPlans => Set<SubscriptionPlan>();
    public DbSet<UserSubscription> UserSubscriptions => Set<UserSubscription>();
    public DbSet<CoinTransaction> CoinTransactions => Set<CoinTransaction>();
    public DbSet<Gift> Gifts => Set<Gift>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Block> Blocks => Set<Block>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<DepositRequest> DepositRequests => Set<DepositRequest>();
    public DbSet<WithdrawalRequest> WithdrawalRequests => Set<WithdrawalRequest>();
    public DbSet<CallSession> CallSessions => Set<CallSession>();
    public DbSet<PrivacyAgreement> PrivacyAgreements => Set<PrivacyAgreement>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Global soft-delete filters
        mb.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        mb.Entity<Match>().HasQueryFilter(e => !e.IsDeleted);
        mb.Entity<Message>().HasQueryFilter(e => !e.IsDeleted);

        // Unique constraints
        mb.Entity<User>().HasIndex(u => u.Email).IsUnique().HasFilter("[Email] IS NOT NULL");
        mb.Entity<User>().HasIndex(u => u.Phone).IsUnique().HasFilter("[Phone] IS NOT NULL");
        mb.Entity<Block>().HasIndex(b => new { b.BlockerId, b.BlockedUserId }).IsUnique();
        mb.Entity<Swipe>().HasIndex(s => new { s.SwiperId, s.TargetId }).IsUnique();
        mb.Entity<UserInterest>().HasKey(ui => new { ui.UserId, ui.InterestId });

        // Relationships
        mb.Entity<UserPreference>().HasOne(p => p.User).WithOne(u => u.Preference)
            .HasForeignKey<UserPreference>(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
        mb.Entity<UserLocation>().HasOne(l => l.User).WithOne(u => u.Location)
            .HasForeignKey<UserLocation>(l => l.UserId).OnDelete(DeleteBehavior.Cascade);
        mb.Entity<Chat>().HasOne(c => c.Match).WithOne(m => m.Chat)
            .HasForeignKey<Chat>(c => c.MatchId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Match>().HasOne(m => m.User1).WithMany().HasForeignKey(m => m.User1Id).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Match>().HasOne(m => m.User2).WithMany().HasForeignKey(m => m.User2Id).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Message>().HasOne(m => m.Sender).WithMany().HasForeignKey(m => m.SenderId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Swipe>().HasOne(s => s.Swiper).WithMany().HasForeignKey(s => s.SwiperId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Swipe>().HasOne(s => s.Target).WithMany().HasForeignKey(s => s.TargetId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Block>().HasOne(b => b.Blocker).WithMany().HasForeignKey(b => b.BlockerId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Block>().HasOne(b => b.BlockedUser).WithMany().HasForeignKey(b => b.BlockedUserId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Report>().HasOne(r => r.Reporter).WithMany().HasForeignKey(r => r.ReporterId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<Report>().HasOne(r => r.ReportedUser).WithMany().HasForeignKey(r => r.ReportedUserId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<CallSession>().HasOne(c => c.Caller).WithMany().HasForeignKey(c => c.CallerId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<CallSession>().HasOne(c => c.Receiver).WithMany().HasForeignKey(c => c.ReceiverId).OnDelete(DeleteBehavior.Restrict);
        mb.Entity<SubscriptionPlan>().Property(p => p.Price).HasPrecision(18, 2);

        SeedData(mb);
    }

    private static void SeedData(ModelBuilder mb)
    {
        var hash = "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG"; // Admin@123456

        // ── Interests ─────────────────────────────────────────────────
        mb.Entity<Interest>().HasData(
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000001"), Name = "Music", Icon = "musical-notes-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000002"), Name = "Travel", Icon = "airplane-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000003"), Name = "Gym", Icon = "barbell-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000004"), Name = "Movies", Icon = "film-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000005"), Name = "Reading", Icon = "book-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000006"), Name = "Cooking", Icon = "restaurant-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000007"), Name = "Art", Icon = "color-palette-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000008"), Name = "Dancing", Icon = "body-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000009"), Name = "Photography", Icon = "camera-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000010"), Name = "Yoga", Icon = "body-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000011"), Name = "Travelling", Icon = "map-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000012"), Name = "Shopping", Icon = "bag-handle-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000013"), Name = "Video games", Icon = "game-controller-outline" },
            new Interest { Id = Guid.Parse("a0000001-0000-0000-0000-000000000014"), Name = "Run", Icon = "walk-outline" }
        );

        // ── Subscription Plans ─────────────────────────────────────────
        mb.Entity<SubscriptionPlan>().HasData(
            new SubscriptionPlan { Id = Guid.Parse("b0000001-0000-0000-0000-000000000001"), Name = "Silver", Price = 299, DurationDays = 30, Features = "[\"Unlimited likes\",\"No ads\",\"See who liked you\",\"Basic filters\"]", IsPopular = false },
            new SubscriptionPlan { Id = Guid.Parse("b0000001-0000-0000-0000-000000000002"), Name = "Gold", Price = 599, DurationDays = 30, Features = "[\"Unlimited likes\",\"No ads\",\"Verified filter\",\"Profile boost\",\"5 coins per message\"]", IsPopular = true },
            new SubscriptionPlan { Id = Guid.Parse("b0000001-0000-0000-0000-000000000003"), Name = "Platinum", Price = 999, DurationDays = 30, Features = "[\"All Gold features\",\"Top picks daily\",\"Priority support\",\"Read receipts\",\"Free video calls\"]", IsPopular = false }
        );

        // ── Gifts (coin costs from spec) ───────────────────────────────
        mb.Entity<Gift>().HasData(
            new Gift { Id = Guid.Parse("c0000001-0000-0000-0000-000000000001"), Name = "Heart", Icon = "heart-outline", CoinCost = 10 },
            new Gift { Id = Guid.Parse("c0000001-0000-0000-0000-000000000002"), Name = "Rose", Icon = "rose-outline", CoinCost = 20 },
            new Gift { Id = Guid.Parse("c0000001-0000-0000-0000-000000000003"), Name = "Gift", Icon = "gift-outline", CoinCost = 50 },
            new Gift { Id = Guid.Parse("c0000001-0000-0000-0000-000000000004"), Name = "Coffee Date", Icon = "cafe-outline", CoinCost = 200 }
        );

        // ── Users ──────────────────────────────────────────────────────
        var adminId = Guid.Parse("d0000001-0000-0000-0000-000000000001");
        var priyaId = Guid.Parse("d0000001-0000-0000-0000-000000000002");
        var rahulId = Guid.Parse("d0000001-0000-0000-0000-000000000003");
        var arjunId = Guid.Parse("d0000001-0000-0000-0000-000000000004");
        var nehaId = Guid.Parse("d0000001-0000-0000-0000-000000000005");
        var vikramId = Guid.Parse("d0000001-0000-0000-0000-000000000006");
        var ankitaId = Guid.Parse("d0000001-0000-0000-0000-000000000007");
        var deepakId = Guid.Parse("d0000001-0000-0000-0000-000000000008");
        var aishaId = Guid.Parse("d0000001-0000-0000-0000-000000000009");
        var rohitId = Guid.Parse("d0000001-0000-0000-0000-000000000010");

        mb.Entity<User>().HasData(
            new User { Id = adminId, FullName = "Super Admin", Email = "admin@mingley.app", PasswordHash = hash, Gender = "male", Role = "admin", IsVerified = true, CoinBalance = 0, DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc), Avatar = "https://randomuser.me/api/portraits/men/1.jpg" },
            new User { Id = priyaId, FullName = "Priya Sharma", Email = "priya@demo.com", PasswordHash = hash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1500, DateOfBirth = new DateTime(1998, 3, 15, 0, 0, 0, DateTimeKind.Utc), Bio = "Love dancing, yoga and cooking 🌺 | Delhi girl", Avatar = "https://randomuser.me/api/portraits/women/44.jpg", IsOnline = true },
            new User { Id = rahulId, FullName = "Rahul Mehta", Email = "rahul@demo.com", PasswordHash = hash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 2000, DateOfBirth = new DateTime(1995, 7, 22, 0, 0, 0, DateTimeKind.Utc), Bio = "Music lover 🎵 | Traveller | Software Engineer", Avatar = "https://randomuser.me/api/portraits/men/32.jpg" },
            new User { Id = arjunId, FullName = "Arjun Singh", Email = "arjun@demo.com", PasswordHash = hash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 5000, DateOfBirth = new DateTime(1993, 11, 5, 0, 0, 0, DateTimeKind.Utc), Bio = "Fitness enthusiast 💪 | Photographer | Noida", Avatar = "https://randomuser.me/api/portraits/men/45.jpg", IsPremium = true },
            new User { Id = nehaId, FullName = "Neha Kapoor", Email = "neha@demo.com", PasswordHash = hash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 800, DateOfBirth = new DateTime(1999, 7, 20, 0, 0, 0, DateTimeKind.Utc), Bio = "Singer and travel lover 🎵✈️ | Mumbai", Avatar = "https://randomuser.me/api/portraits/women/68.jpg" },
            new User { Id = vikramId, FullName = "Vikram Nair", Email = "vikram@demo.com", PasswordHash = hash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 600, DateOfBirth = new DateTime(1996, 4, 12, 0, 0, 0, DateTimeKind.Utc), Bio = "Entrepreneur | Coffee addict ☕ | Delhi", Avatar = "https://randomuser.me/api/portraits/men/75.jpg" },
            new User { Id = ankitaId, FullName = "Ankita Singh", Email = "ankita@demo.com", PasswordHash = hash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 400, DateOfBirth = new DateTime(2000, 11, 5, 0, 0, 0, DateTimeKind.Utc), Bio = "Foodie and photographer 📸🍕 | Pune", Avatar = "https://randomuser.me/api/portraits/women/90.jpg" },
            new User { Id = deepakId, FullName = "Deepak Verma", Email = "deepak@demo.com", PasswordHash = hash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 100, DateOfBirth = new DateTime(1997, 9, 30, 0, 0, 0, DateTimeKind.Utc), Bio = "Gym rat 🏋️ | Cricket fan | Noida", Avatar = "https://randomuser.me/api/portraits/men/88.jpg" },
            new User { Id = aishaId, FullName = "Aisha Khan", Email = "aisha@demo.com", PasswordHash = hash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1200, DateOfBirth = new DateTime(1999, 2, 14, 0, 0, 0, DateTimeKind.Utc), Bio = "Fashion lover 👗 | Artist | Hyderabad", Avatar = "https://randomuser.me/api/portraits/women/55.jpg" },
            new User { Id = rohitId, FullName = "Rohit Sharma", Email = "rohit@demo.com", PasswordHash = hash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 300, DateOfBirth = new DateTime(1994, 6, 25, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef 🍳 | Food blogger | Bengaluru", Avatar = "https://randomuser.me/api/portraits/men/60.jpg" }
        );

        // ── Preferences (gender-based defaults) ────────────────────────
        mb.Entity<UserPreference>().HasData(
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000001"), UserId = priyaId, InterestedIn = "boys", MinAge = 22, MaxAge = 35, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000002"), UserId = rahulId, InterestedIn = "girls", MinAge = 20, MaxAge = 30, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000003"), UserId = arjunId, InterestedIn = "girls", MinAge = 21, MaxAge = 32, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000004"), UserId = nehaId, InterestedIn = "boys", MinAge = 23, MaxAge = 33, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000005"), UserId = ankitaId, InterestedIn = "boys", MinAge = 24, MaxAge = 34, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000006"), UserId = vikramId, InterestedIn = "girls", MinAge = 21, MaxAge = 30, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000007"), UserId = deepakId, InterestedIn = "girls", MinAge = 20, MaxAge = 28, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000008"), UserId = aishaId, InterestedIn = "boys", MinAge = 22, MaxAge = 32, MaxDistance = 100 },
            new UserPreference { Id = Guid.Parse("a2000001-0000-0000-0000-000000000009"), UserId = rohitId, InterestedIn = "girls", MinAge = 20, MaxAge = 30, MaxDistance = 100 }
        );

        // ── Locations ──────────────────────────────────────────────────
        mb.Entity<UserLocation>().HasData(
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000001"), UserId = priyaId, City = "Delhi", Country = "India", Lat = 28.614, Lng = 77.209 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000002"), UserId = rahulId, City = "Noida", Country = "India", Lat = 28.535, Lng = 77.391 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000003"), UserId = arjunId, City = "Gurgaon", Country = "India", Lat = 28.459, Lng = 77.026 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000004"), UserId = nehaId, City = "Mumbai", Country = "India", Lat = 19.076, Lng = 72.877 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000005"), UserId = ankitaId, City = "Pune", Country = "India", Lat = 18.520, Lng = 73.856 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000006"), UserId = vikramId, City = "Delhi", Country = "India", Lat = 28.700, Lng = 77.100 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000007"), UserId = deepakId, City = "Noida", Country = "India", Lat = 28.540, Lng = 77.400 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000008"), UserId = aishaId, City = "Hyderabad", Country = "India", Lat = 17.385, Lng = 78.486 },
            new UserLocation { Id = Guid.Parse("b2000001-0000-0000-0000-000000000009"), UserId = rohitId, City = "Bengaluru", Country = "India", Lat = 12.972, Lng = 77.594 }
        );

        // ── Interests for users ────────────────────────────────────────
        mb.Entity<UserInterest>().HasData(
            new UserInterest { UserId = priyaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000001") }, // Music
            new UserInterest { UserId = priyaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000008") }, // Dancing
            new UserInterest { UserId = priyaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000010") }, // Yoga
            new UserInterest { UserId = rahulId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000001") }, // Music
            new UserInterest { UserId = rahulId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000002") }, // Travel
            new UserInterest { UserId = rahulId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000009") }, // Photography
            new UserInterest { UserId = arjunId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000003") }, // Gym
            new UserInterest { UserId = arjunId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000009") }, // Photography
            new UserInterest { UserId = nehaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000001") }, // Music
            new UserInterest { UserId = nehaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000002") }, // Travel
            new UserInterest { UserId = ankitaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000006") }, // Cooking
            new UserInterest { UserId = ankitaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000009") }, // Photography
            new UserInterest { UserId = vikramId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000002") }, // Travel
            new UserInterest { UserId = deepakId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000003") }, // Gym
            new UserInterest { UserId = aishaId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000007") }, // Art
            new UserInterest { UserId = rohitId, InterestId = Guid.Parse("a0000001-0000-0000-0000-000000000006") }  // Cooking
        );

        // ── Pre-seeded Match: Rahul ↔ Priya (chat + messages) ──────────
        var match1Id = Guid.Parse("a1000001-0000-0000-0000-000000000001");
        var chat1Id = Guid.Parse("a1000001-0000-0000-0000-000000000002");

        mb.Entity<Swipe>().HasData(
            new Swipe { Id = Guid.Parse("b1000001-0000-0000-0000-000000000001"), SwiperId = rahulId, TargetId = priyaId, Action = "like", CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
            new Swipe { Id = Guid.Parse("b1000001-0000-0000-0000-000000000002"), SwiperId = priyaId, TargetId = rahulId, Action = "like", CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) }
        );
        mb.Entity<Match>().HasData(
            new Match { Id = match1Id, User1Id = rahulId, User2Id = priyaId, IsActive = true, CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) }
        );
        mb.Entity<Chat>().HasData(
            new Chat { Id = chat1Id, MatchId = match1Id, CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) }
        );
        mb.Entity<Message>().HasData(
            new Message { Id = Guid.Parse("c1000001-0000-0000-0000-000000000001"), ChatId = chat1Id, SenderId = rahulId, Text = "Hey Priya! We matched 🎉 How are you?", Type = "text", CoinsDeducted = 10, ReadAt = new DateTime(2024, 1, 2, 1, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2024, 1, 2, 0, 30, 0, DateTimeKind.Utc) },
            new Message { Id = Guid.Parse("c1000001-0000-0000-0000-000000000002"), ChatId = chat1Id, SenderId = priyaId, Text = "Hi Rahul! I'm great, thanks! How about you? 😊", Type = "text", ReadAt = new DateTime(2024, 1, 2, 2, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2024, 1, 2, 1, 0, 0, DateTimeKind.Utc) },
            new Message { Id = Guid.Parse("c1000001-0000-0000-0000-000000000003"), ChatId = chat1Id, SenderId = rahulId, Text = "Doing well! I saw you love dancing 💃", Type = "text", CoinsDeducted = 10, ReadAt = new DateTime(2024, 1, 2, 3, 0, 0, DateTimeKind.Utc), CreatedAt = new DateTime(2024, 1, 2, 2, 0, 0, DateTimeKind.Utc) },
            new Message { Id = Guid.Parse("c1000001-0000-0000-0000-000000000004"), ChatId = chat1Id, SenderId = priyaId, Text = "Yes! I've been dancing since I was 8 🎵", Type = "text", CreatedAt = new DateTime(2024, 1, 2, 3, 0, 0, DateTimeKind.Utc) },
            new Message { Id = Guid.Parse("c1000001-0000-0000-0000-000000000005"), ChatId = chat1Id, SenderId = rahulId, Text = "That's amazing! I play guitar 🎸", Type = "text", CoinsDeducted = 10, CreatedAt = new DateTime(2024, 1, 2, 4, 0, 0, DateTimeKind.Utc) }
        );

        // ── Deposit + Withdrawal pending requests for admin to test ─────
        mb.Entity<DepositRequest>().HasData(
            new DepositRequest { Id = Guid.Parse("e1000001-0000-0000-0000-000000000001"), UserId = rahulId, UtrId = "UTR123456789", RequestedCoins = 1000, Status = "pending", CreatedAt = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
            new DepositRequest { Id = Guid.Parse("e1000001-0000-0000-0000-000000000003"), UserId = arjunId, UtrId = "UTR987654321", RequestedCoins = 2000, Status = "approved", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );
        mb.Entity<WithdrawalRequest>().HasData(
            new WithdrawalRequest { Id = Guid.Parse("e1000001-0000-0000-0000-000000000002"), UserId = priyaId, Coins = 700, BankOrUpi = "priya@paytm", Status = "pending", CreatedAt = new DateTime(2024, 1, 3, 0, 0, 0, DateTimeKind.Utc) },
            new WithdrawalRequest { Id = Guid.Parse("e1000001-0000-0000-0000-000000000004"), UserId = nehaId, Coins = 400, BankOrUpi = "neha@gpay", Status = "approved", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // ── Notifications ──────────────────────────────────────────────
        mb.Entity<Notification>().HasData(
            new Notification { Id = Guid.Parse("f1000001-0000-0000-0000-000000000001"), UserId = rahulId, Title = "New Match! 🎉", Body = "You matched with Priya Sharma!", Type = "match", IsRead = false, CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
            new Notification { Id = Guid.Parse("f1000001-0000-0000-0000-000000000002"), UserId = priyaId, Title = "New Match! 🎉", Body = "You matched with Rahul Mehta!", Type = "match", IsRead = true, CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc) },
            new Notification { Id = Guid.Parse("f1000001-0000-0000-0000-000000000003"), UserId = rahulId, Title = "New Message 💬", Body = "Priya sent you a message", Type = "message", IsRead = false, CreatedAt = new DateTime(2024, 1, 2, 1, 0, 0, DateTimeKind.Utc) }
        );


        // PASTE THIS AT THE END of SeedData() in MingleyDbContext.cs (before closing brace)

        var bulkHash = "$2b$10$pbJL9X1wbdKR2JWuJcGBnuduGgh0e/j1PDYzdwt4F5KelwMVzsFeG";

        // 50 FEMALE (011-060) + 50 MALE (061-110)
        mb.Entity<User>().HasData(
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000011"), FullName = "Divya Menon", Email = "divya@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1811, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1999, 7, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Books and coffee", Avatar = "https://randomuser.me/api/portraits/women/5.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000012"), FullName = "Pooja Reddy", Email = "pooja@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1301, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1998, 10, 19, 0, 0, 0, DateTimeKind.Utc), Bio = "Fitness enthusiast | Yoga instructor", Avatar = "https://randomuser.me/api/portraits/women/6.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000013"), FullName = "Shruti Verma", Email = "shruti@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 783, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 4, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef in the making | Food blogger", Avatar = "https://randomuser.me/api/portraits/women/7.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000014"), FullName = "Kavya Nair", Email = "kavya@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 559, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 3, 25, 0, 0, 0, DateTimeKind.Utc), Bio = "Software engineer by day, painter by night", Avatar = "https://randomuser.me/api/portraits/women/8.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000015"), FullName = "Meera Joshi", Email = "meera@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1367, IsPremium = true, IsOnline = true, DateOfBirth = new DateTime(2002, 11, 22, 0, 0, 0, DateTimeKind.Utc), Bio = "Travel addict | 23 countries visited", Avatar = "https://randomuser.me/api/portraits/women/9.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000016"), FullName = "Riya Gupta", Email = "riya@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 295, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 9, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Music is my therapy", Avatar = "https://randomuser.me/api/portraits/women/10.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000017"), FullName = "Simran Kaur", Email = "simran@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 236, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 1, 26, 0, 0, 0, DateTimeKind.Utc), Bio = "Dog mom | Nature lover", Avatar = "https://randomuser.me/api/portraits/women/11.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000018"), FullName = "Nandini Rao", Email = "nandini@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1163, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 11, 7, 0, 0, 0, DateTimeKind.Utc), Bio = "Entrepreneur | Dream chaser", Avatar = "https://randomuser.me/api/portraits/women/12.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000019"), FullName = "Trisha Das", Email = "trisha@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 1394, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(2002, 8, 19, 0, 0, 0, DateTimeKind.Utc), Bio = "Dancer | Theatre artist", Avatar = "https://randomuser.me/api/portraits/women/13.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000020"), FullName = "Sonali Patil", Email = "sonali@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 454, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 7, 14, 0, 0, 0, DateTimeKind.Utc), Bio = "Voracious reader | Tea enthusiast", Avatar = "https://randomuser.me/api/portraits/women/14.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000021"), FullName = "Kritika Sharma", Email = "kritika@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1036, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2002, 5, 3, 0, 0, 0, DateTimeKind.Utc), Bio = "Adventure seeker | Hiker", Avatar = "https://randomuser.me/api/portraits/women/15.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000022"), FullName = "Pallavi Iyer", Email = "pallavi@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 796, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1995, 9, 12, 0, 0, 0, DateTimeKind.Utc), Bio = "Makeup artist | Beauty blogger", Avatar = "https://randomuser.me/api/portraits/women/16.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000023"), FullName = "Ananya Bose", Email = "ananya@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 263, IsPremium = true, IsOnline = true, DateOfBirth = new DateTime(1995, 11, 11, 0, 0, 0, DateTimeKind.Utc), Bio = "Data scientist by profession, artist at heart", Avatar = "https://randomuser.me/api/portraits/women/17.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000024"), FullName = "Swati Mishra", Email = "swati@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 857, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 12, 28, 0, 0, 0, DateTimeKind.Utc), Bio = "Meditation and mindfulness", Avatar = "https://randomuser.me/api/portraits/women/18.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000025"), FullName = "Deepika Roy", Email = "deepika@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 949, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 8, 10, 0, 0, 0, DateTimeKind.Utc), Bio = "Loves cooking for people she cares about", Avatar = "https://randomuser.me/api/portraits/women/19.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000026"), FullName = "Aditi Pandey", Email = "aditi@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1132, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2002, 10, 21, 0, 0, 0, DateTimeKind.Utc), Bio = "Gym addict | Health coach", Avatar = "https://randomuser.me/api/portraits/women/20.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000027"), FullName = "Sakshi Yadav", Email = "sakshi@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 441, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(2002, 6, 13, 0, 0, 0, DateTimeKind.Utc), Bio = "Love dancing yoga and cooking", Avatar = "https://randomuser.me/api/portraits/women/21.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000028"), FullName = "Ishita Malik", Email = "ishita@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 667, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1998, 5, 23, 0, 0, 0, DateTimeKind.Utc), Bio = "Singer and travel lover", Avatar = "https://randomuser.me/api/portraits/women/22.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000029"), FullName = "Preeti Arora", Email = "preeti@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1300, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 6, 27, 0, 0, 0, DateTimeKind.Utc), Bio = "Foodie and photographer", Avatar = "https://randomuser.me/api/portraits/women/23.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000030"), FullName = "Varsha Kumar", Email = "varsha@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1043, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 8, 5, 0, 0, 0, DateTimeKind.Utc), Bio = "Fashion lover | Artist", Avatar = "https://randomuser.me/api/portraits/women/24.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000031"), FullName = "Tanvi Jain", Email = "tanvi@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 1005, IsPremium = true, IsOnline = true, DateOfBirth = new DateTime(2002, 5, 13, 0, 0, 0, DateTimeKind.Utc), Bio = "Books and coffee", Avatar = "https://randomuser.me/api/portraits/women/25.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000032"), FullName = "Rashmi Pillai", Email = "rashmi@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 446, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2002, 11, 20, 0, 0, 0, DateTimeKind.Utc), Bio = "Fitness enthusiast | Yoga instructor", Avatar = "https://randomuser.me/api/portraits/women/26.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000033"), FullName = "Komal Shah", Email = "komal@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 851, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 2, 11, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef in the making | Food blogger", Avatar = "https://randomuser.me/api/portraits/women/27.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000034"), FullName = "Preethi Nair", Email = "preethi@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 1159, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2002, 1, 26, 0, 0, 0, DateTimeKind.Utc), Bio = "Software engineer by day painter at night", Avatar = "https://randomuser.me/api/portraits/women/28.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000035"), FullName = "Lavanya Reddy", Email = "lavanya@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1312, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(2000, 2, 6, 0, 0, 0, DateTimeKind.Utc), Bio = "Travel addict | 23 countries visited", Avatar = "https://randomuser.me/api/portraits/women/29.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000036"), FullName = "Gauri Desai", Email = "gauri@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1152, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 9, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Music is my therapy", Avatar = "https://randomuser.me/api/portraits/women/30.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000037"), FullName = "Megha Tiwari", Email = "megha@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 615, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1998, 9, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Dog mom | Nature lover", Avatar = "https://randomuser.me/api/portraits/women/31.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000038"), FullName = "Sonal Mehta", Email = "sonal@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 708, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1998, 6, 15, 0, 0, 0, DateTimeKind.Utc), Bio = "Entrepreneur | Dream chaser", Avatar = "https://randomuser.me/api/portraits/women/32.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000039"), FullName = "Roshni Choudhary", Email = "roshni@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1428, IsPremium = true, IsOnline = true, DateOfBirth = new DateTime(1997, 6, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Dancer | Theatre artist", Avatar = "https://randomuser.me/api/portraits/women/33.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000040"), FullName = "Bhavna Saxena", Email = "bhavna@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 935, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1996, 10, 21, 0, 0, 0, DateTimeKind.Utc), Bio = "Voracious reader | Tea enthusiast", Avatar = "https://randomuser.me/api/portraits/women/34.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000041"), FullName = "Nisha Bajaj", Email = "nisha@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1838, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1995, 7, 23, 0, 0, 0, DateTimeKind.Utc), Bio = "Adventure seeker | Hiker", Avatar = "https://randomuser.me/api/portraits/women/35.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000042"), FullName = "Payal Ghosh", Email = "payal@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 981, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 5, 20, 0, 0, 0, DateTimeKind.Utc), Bio = "Makeup artist | Beauty blogger", Avatar = "https://randomuser.me/api/portraits/women/36.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000043"), FullName = "Monika Srivastava", Email = "monika@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 494, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1998, 9, 27, 0, 0, 0, DateTimeKind.Utc), Bio = "Data scientist by profession artist at heart", Avatar = "https://randomuser.me/api/portraits/women/37.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000044"), FullName = "Deeksha Singh", Email = "deeksha@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1586, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1995, 9, 25, 0, 0, 0, DateTimeKind.Utc), Bio = "Meditation and mindfulness", Avatar = "https://randomuser.me/api/portraits/women/38.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000045"), FullName = "Harpreet Bhatia", Email = "harpreet@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 858, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1998, 3, 6, 0, 0, 0, DateTimeKind.Utc), Bio = "Loves cooking for people she cares about", Avatar = "https://randomuser.me/api/portraits/women/39.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000046"), FullName = "Amrita Chatterjee", Email = "amrita@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 689, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 11, 18, 0, 0, 0, DateTimeKind.Utc), Bio = "Gym addict | Health coach", Avatar = "https://randomuser.me/api/portraits/women/40.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000047"), FullName = "Sunita Pillai", Email = "sunita@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 1330, IsPremium = true, IsOnline = true, DateOfBirth = new DateTime(1995, 5, 18, 0, 0, 0, DateTimeKind.Utc), Bio = "Love dancing yoga and cooking", Avatar = "https://randomuser.me/api/portraits/women/41.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000048"), FullName = "Poornima Rao", Email = "poornima@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 754, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 11, 27, 0, 0, 0, DateTimeKind.Utc), Bio = "Singer and travel lover", Avatar = "https://randomuser.me/api/portraits/women/42.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000049"), FullName = "Jayashree Nair", Email = "jayashree@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = false, CoinBalance = 1076, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 11, 5, 0, 0, 0, DateTimeKind.Utc), Bio = "Foodie and photographer", Avatar = "https://randomuser.me/api/portraits/women/43.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000050"), FullName = "Saranya Kumar", Email = "saranya@demo.com", PasswordHash = bulkHash, Gender = "female", Role = "user", IsVerified = true, CoinBalance = 206, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 4, 10, 0, 0, 0, DateTimeKind.Utc), Bio = "Fashion lover | Artist", Avatar = "https://randomuser.me/api/portraits/women/44.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000061"), FullName = "Amit Patel", Email = "amit@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3185, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1990, 3, 23, 0, 0, 0, DateTimeKind.Utc), Bio = "Biker | Mountain lover", Avatar = "https://randomuser.me/api/portraits/men/6.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000062"), FullName = "Karan Malhotra", Email = "karan@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 1626, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(1999, 5, 18, 0, 0, 0, DateTimeKind.Utc), Bio = "Startup founder | Tech geek", Avatar = "https://randomuser.me/api/portraits/men/7.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000063"), FullName = "Raj Kapoor", Email = "raj@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 735, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1990, 2, 22, 0, 0, 0, DateTimeKind.Utc), Bio = "Wildlife photographer", Avatar = "https://randomuser.me/api/portraits/men/8.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000064"), FullName = "Nikhil Joshi", Email = "nikhil@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3931, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1996, 7, 23, 0, 0, 0, DateTimeKind.Utc), Bio = "Musician | Weekend hiker", Avatar = "https://randomuser.me/api/portraits/men/9.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000065"), FullName = "Sanjay Gupta", Email = "sanjay@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 3883, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 10, 17, 0, 0, 0, DateTimeKind.Utc), Bio = "Doctor | Runner", Avatar = "https://randomuser.me/api/portraits/men/10.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000066"), FullName = "Aditya Kumar", Email = "aditya@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3183, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1999, 5, 16, 0, 0, 0, DateTimeKind.Utc), Bio = "Architect | Design enthusiast", Avatar = "https://randomuser.me/api/portraits/men/11.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000067"), FullName = "Manish Tiwari", Email = "manish@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3925, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 7, 28, 0, 0, 0, DateTimeKind.Utc), Bio = "Teacher | Book lover", Avatar = "https://randomuser.me/api/portraits/men/12.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000068"), FullName = "Gaurav Reddy", Email = "gaurav@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 1515, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(1995, 2, 19, 0, 0, 0, DateTimeKind.Utc), Bio = "Pilot | Adventure seeker", Avatar = "https://randomuser.me/api/portraits/men/13.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000069"), FullName = "Sumit Yadav", Email = "sumit@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1932, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 9, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Writer | Coffee connoisseur", Avatar = "https://randomuser.me/api/portraits/men/14.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000070"), FullName = "Vishal Chauhan", Email = "vishal@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1689, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 11, 21, 0, 0, 0, DateTimeKind.Utc), Bio = "Cricket player | Fitness freak", Avatar = "https://randomuser.me/api/portraits/men/15.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000071"), FullName = "Ankit Sharma", Email = "ankit@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 4294, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1991, 11, 25, 0, 0, 0, DateTimeKind.Utc), Bio = "Coder by day gamer by night", Avatar = "https://randomuser.me/api/portraits/men/16.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000072"), FullName = "Ravi Menon", Email = "ravi@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1767, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1995, 8, 27, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef in the making | Food critic", Avatar = "https://randomuser.me/api/portraits/men/17.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000073"), FullName = "Sandeep Iyer", Email = "sandeep@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3209, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1993, 2, 14, 0, 0, 0, DateTimeKind.Utc), Bio = "Yoga and meditation | Philosophy lover", Avatar = "https://randomuser.me/api/portraits/men/18.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000074"), FullName = "Akash Singh", Email = "akash@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 4732, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(1997, 4, 12, 0, 0, 0, DateTimeKind.Utc), Bio = "Marine biologist | Diver", Avatar = "https://randomuser.me/api/portraits/men/19.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000075"), FullName = "Pankaj Bose", Email = "pankaj@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3607, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1992, 11, 13, 0, 0, 0, DateTimeKind.Utc), Bio = "Stand-up comedian | Movie buff", Avatar = "https://randomuser.me/api/portraits/men/20.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000076"), FullName = "Tarun Saxena", Email = "tarun@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 2389, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1993, 2, 2, 0, 0, 0, DateTimeKind.Utc), Bio = "Music lover | Traveller", Avatar = "https://randomuser.me/api/portraits/men/21.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000077"), FullName = "Harish Pillai", Email = "harish@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 1440, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 1, 10, 0, 0, 0, DateTimeKind.Utc), Bio = "Fitness enthusiast | Photographer", Avatar = "https://randomuser.me/api/portraits/men/22.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000078"), FullName = "Vivek Srivastava", Email = "vivek@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 2628, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1996, 10, 6, 0, 0, 0, DateTimeKind.Utc), Bio = "Entrepreneur | Coffee addict", Avatar = "https://randomuser.me/api/portraits/men/23.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000079"), FullName = "Mohit Choudhary", Email = "mohit@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 414, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), Bio = "Gym rat | Cricket fan", Avatar = "https://randomuser.me/api/portraits/men/24.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000080"), FullName = "Ashish Bajaj", Email = "ashish@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 3432, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(1995, 4, 12, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef | Food blogger", Avatar = "https://randomuser.me/api/portraits/men/25.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000081"), FullName = "Praveen Ghosh", Email = "praveen@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3362, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1991, 6, 9, 0, 0, 0, DateTimeKind.Utc), Bio = "Biker | Mountain lover", Avatar = "https://randomuser.me/api/portraits/men/26.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000082"), FullName = "Suresh Nair", Email = "suresh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3101, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1990, 7, 13, 0, 0, 0, DateTimeKind.Utc), Bio = "Startup founder | Tech geek", Avatar = "https://randomuser.me/api/portraits/men/27.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000083"), FullName = "Dinesh Patil", Email = "dinesh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 1972, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 12, 26, 0, 0, 0, DateTimeKind.Utc), Bio = "Wildlife photographer", Avatar = "https://randomuser.me/api/portraits/men/28.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000084"), FullName = "Vinod Kulkarni", Email = "vinod@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 4773, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 8, 16, 0, 0, 0, DateTimeKind.Utc), Bio = "Musician | Weekend hiker", Avatar = "https://randomuser.me/api/portraits/men/29.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000085"), FullName = "Ramesh Rao", Email = "ramesh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1117, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1999, 7, 24, 0, 0, 0, DateTimeKind.Utc), Bio = "Doctor | Runner", Avatar = "https://randomuser.me/api/portraits/men/30.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000086"), FullName = "Ajay Desai", Email = "ajay@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 4010, IsPremium = true, IsOnline = true, DateOfBirth = new DateTime(1998, 11, 22, 0, 0, 0, DateTimeKind.Utc), Bio = "Architect | Design enthusiast", Avatar = "https://randomuser.me/api/portraits/men/31.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000087"), FullName = "Vijay Krishnan", Email = "vijay@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 4672, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1995, 9, 7, 0, 0, 0, DateTimeKind.Utc), Bio = "Teacher | Book lover", Avatar = "https://randomuser.me/api/portraits/men/32.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000088"), FullName = "Manoj Shah", Email = "manoj@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3133, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1992, 9, 4, 0, 0, 0, DateTimeKind.Utc), Bio = "Pilot | Adventure seeker", Avatar = "https://randomuser.me/api/portraits/men/33.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000089"), FullName = "Naresh Bansal", Email = "naresh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 3506, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1998, 12, 11, 0, 0, 0, DateTimeKind.Utc), Bio = "Writer | Coffee connoisseur", Avatar = "https://randomuser.me/api/portraits/men/34.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000090"), FullName = "Girish Suresh", Email = "girish@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 4031, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1994, 5, 21, 0, 0, 0, DateTimeKind.Utc), Bio = "Cricket player | Fitness freak", Avatar = "https://randomuser.me/api/portraits/men/35.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000091"), FullName = "Kartik Arora", Email = "kartik@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 846, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1996, 8, 13, 0, 0, 0, DateTimeKind.Utc), Bio = "Coder by day gamer by night", Avatar = "https://randomuser.me/api/portraits/men/36.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000092"), FullName = "Nitin Mishra", Email = "nitin@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 2916, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(1993, 10, 8, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef in the making | Food critic", Avatar = "https://randomuser.me/api/portraits/men/37.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000093"), FullName = "Satish Roy", Email = "satish@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 4750, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1990, 6, 2, 0, 0, 0, DateTimeKind.Utc), Bio = "Yoga and meditation | Philosophy lover", Avatar = "https://randomuser.me/api/portraits/men/38.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000094"), FullName = "Rakesh Pandey", Email = "rakesh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1086, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1992, 10, 3, 0, 0, 0, DateTimeKind.Utc), Bio = "Marine biologist | Diver", Avatar = "https://randomuser.me/api/portraits/men/39.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000095"), FullName = "Pradeep Malik", Email = "pradeep@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 404, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 3, 3, 0, 0, 0, DateTimeKind.Utc), Bio = "Stand-up comedian | Movie buff", Avatar = "https://randomuser.me/api/portraits/men/40.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000096"), FullName = "Sachin Bhatia", Email = "sachin@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 501, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(2000, 5, 19, 0, 0, 0, DateTimeKind.Utc), Bio = "Music lover | Traveller", Avatar = "https://randomuser.me/api/portraits/men/41.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000097"), FullName = "Devendra Chatterjee", Email = "devendra@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 4331, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 10, 10, 0, 0, 0, DateTimeKind.Utc), Bio = "Fitness enthusiast | Photographer", Avatar = "https://randomuser.me/api/portraits/men/42.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000098"), FullName = "Srinivas Rajan", Email = "srinivas@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 2496, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(2000, 4, 12, 0, 0, 0, DateTimeKind.Utc), Bio = "Entrepreneur | Coffee addict", Avatar = "https://randomuser.me/api/portraits/men/43.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000099"), FullName = "Krishnan Pillai", Email = "krishnan@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1887, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(2000, 2, 22, 0, 0, 0, DateTimeKind.Utc), Bio = "Gym rat | Cricket fan", Avatar = "https://randomuser.me/api/portraits/men/44.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000100"), FullName = "Venkat Rao", Email = "venkat@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3735, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1994, 12, 1, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef | Food blogger", Avatar = "https://randomuser.me/api/portraits/men/45.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000101"), FullName = "Sunil Jain", Email = "sunil@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 656, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1998, 6, 15, 0, 0, 0, DateTimeKind.Utc), Bio = "Biker | Mountain lover", Avatar = "https://randomuser.me/api/portraits/men/46.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000102"), FullName = "Rajesh Kumar", Email = "rajesh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 3040, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1995, 12, 26, 0, 0, 0, DateTimeKind.Utc), Bio = "Startup founder | Tech geek", Avatar = "https://randomuser.me/api/portraits/men/47.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000103"), FullName = "Hemant Singh", Email = "hemant@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 4416, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1994, 10, 27, 0, 0, 0, DateTimeKind.Utc), Bio = "Wildlife photographer", Avatar = "https://randomuser.me/api/portraits/men/48.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000104"), FullName = "Bharat Mehta", Email = "bharat@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 721, IsPremium = true, IsOnline = false, DateOfBirth = new DateTime(1994, 7, 17, 0, 0, 0, DateTimeKind.Utc), Bio = "Musician | Weekend hiker", Avatar = "https://randomuser.me/api/portraits/men/49.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000105"), FullName = "Alok Verma", Email = "alok@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1274, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1997, 2, 28, 0, 0, 0, DateTimeKind.Utc), Bio = "Doctor | Runner", Avatar = "https://randomuser.me/api/portraits/men/50.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000106"), FullName = "Deepesh Nair", Email = "deepesh@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 2100, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1993, 3, 15, 0, 0, 0, DateTimeKind.Utc), Bio = "Architect | Design lover", Avatar = "https://randomuser.me/api/portraits/men/51.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000107"), FullName = "Prashant Singh", Email = "prashant@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 980, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1998, 8, 8, 0, 0, 0, DateTimeKind.Utc), Bio = "Coder by day gamer by night", Avatar = "https://randomuser.me/api/portraits/men/52.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000108"), FullName = "Shyam Verma", Email = "shyam@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 1560, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1996, 4, 20, 0, 0, 0, DateTimeKind.Utc), Bio = "Chef in the making | Foodie", Avatar = "https://randomuser.me/api/portraits/men/53.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000109"), FullName = "Kapil Gupta", Email = "kapil@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = true, CoinBalance = 2900, IsPremium = false, IsOnline = false, DateOfBirth = new DateTime(1994, 1, 5, 0, 0, 0, DateTimeKind.Utc), Bio = "Yoga and meditation | Philosophy lover", Avatar = "https://randomuser.me/api/portraits/men/54.jpg" },
            new User { Id = Guid.Parse("d0000001-0000-0000-0000-000000000110"), FullName = "Sanket Kulkarni", Email = "sanket@demo.com", PasswordHash = bulkHash, Gender = "male", Role = "user", IsVerified = false, CoinBalance = 3200, IsPremium = false, IsOnline = true, DateOfBirth = new DateTime(1997, 6, 30, 0, 0, 0, DateTimeKind.Utc), Bio = "Marine biologist | Diver", Avatar = "https://randomuser.me/api/portraits/men/55.jpg" }
        );






    }
}