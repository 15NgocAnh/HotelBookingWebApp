using HotelBooking.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;

namespace HotelBooking.Data
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasIndex(p => p.Email).IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.user_roles)
                .WithOne(e => e.user)
                .HasForeignKey("user_id")
                .IsRequired();

            modelBuilder.Entity<RoleModel>()
                .HasMany(e => e.UserRole)
                .WithOne(e => e.role)
                .HasForeignKey("role_id")
                .IsRequired();
            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.jwts)
                .WithOne(e => e.user)
                .HasForeignKey("user_id")
                .IsRequired();

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.posts)
                .WithOne(e => e.user)
                .HasForeignKey("user_id");

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.from_user_notifications)
                .WithOne(e => e.from_user_notification)
                .HasForeignKey("from_user_notifi_id")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.posts)
                .WithOne(u => u.user)
                .HasForeignKey("user_id")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingModel>()
                .HasOne(b => b.Hotel)
                .WithMany()
                .HasForeignKey(b => b.HotelCode)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<BookingModel>()
                .HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomNo)
                .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<BookingModel>()
                .HasOne(b => b.Guest)
                .WithMany()
                .HasForeignKey(b => b.GuestID)
                .OnDelete(DeleteBehavior.NoAction); 

            base.OnModelCreating(modelBuilder);
            var entityTypes = modelBuilder.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var entityClrType = entityType.ClrType;

                var isSoftDeleteEnabled = entityClrType.GetProperty("is_deleted") != null;

                if (isSoftDeleteEnabled)
                {
                    var parameter = Expression.Parameter(entityClrType, "e");
                    var property = Expression.Property(parameter, "is_deleted");
                    var notDeleted = Expression.Not(property);
                    var lambda = Expression.Lambda(notDeleted, parameter);

                    entityType.SetQueryFilter(lambda);
                }
            }
            new DbInitializer(modelBuilder).Seed();
        }

        #region DbSet implementation omitted
        public virtual DbSet<UserModel> users { get; set; }
        public virtual DbSet<UserRoleModel> user_roles { get; set; }
        public virtual DbSet<RoleModel> roles { get; set; }
        public virtual DbSet<FileModel> files { get; set; }
        public virtual DbSet<JWTModel> jwts { get; set; }
        public virtual DbSet<NotificationModel> notifications { get; set; }
        public virtual DbSet<PostModel> posts { get; set; }
        public virtual DbSet<RoomTypeModel> room_types { get; set; }
        public virtual DbSet<RoomModel> room { get; set; }
        public virtual DbSet<HotelModel> hotel { get; set; }
        public virtual DbSet<BookingModel> booking { get; set; }
        public virtual DbSet<BillModel> bill { get; set; }
        #endregion

        #region Auto add created-time, updated-time
        public override int SaveChanges()
        {
            try
            {
                AddTimestamps();
                return base.SaveChanges();
            }
            catch
            {
                throw;
            }
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                AddTimestamps();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }
        }

        private void AddTimestamps()
        {
            var Data = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var currentUserId = !string.IsNullOrEmpty(userId)
                ? userId
                : "0";

            foreach (var entity in Data)
            {
                // Get the current UTC time
                DateTime nowUtc = DateTime.UtcNow;  
                // Get the time zone information for Asia Standard Time
                TimeZoneInfo AsiaZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                // Convert the UTC time to Asia Standard Time
                DateTime now = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, AsiaZone);
                var user = int.Parse(currentUserId); 
                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).created_at = now;
                    ((BaseModel)entity.Entity).created_by = user;
                }
                ((BaseModel)entity.Entity).updated_at = now;
                ((BaseModel)entity.Entity).changed_by = user;
            }
        }

        #endregion
    }
}