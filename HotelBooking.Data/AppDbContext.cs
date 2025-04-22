using HotelBooking.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq.Expressions;
using System.Security.Claims;

namespace HotelBooking.Data
{
    /// <summary>
    /// Represents the database context for the Hotel Booking application.
    /// Manages database connections and entity configurations.
    /// </summary>
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor for accessing current user information.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) 
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region DbSet Properties

        /// <summary>
        /// Gets or sets the users DbSet.
        /// </summary>
        public virtual DbSet<UserModel> Users { get; set; }

        /// <summary>
        /// Gets or sets the user roles DbSet.
        /// </summary>
        public virtual DbSet<UserRoleModel> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the roles DbSet.
        /// </summary>
        public virtual DbSet<RoleModel> Roles { get; set; }

        /// <summary>
        /// Gets or sets the files DbSet.
        /// </summary>
        public virtual DbSet<FileModel> Files { get; set; }

        /// <summary>
        /// Gets or sets the JWTs DbSet.
        /// </summary>
        public virtual DbSet<JWTModel> Jwts { get; set; }

        /// <summary>
        /// Gets or sets the notifications DbSet.
        /// </summary>
        public virtual DbSet<NotificationModel> Notifications { get; set; }

        /// <summary>
        /// Gets or sets the posts DbSet.
        /// </summary>
        public virtual DbSet<PostModel> Posts { get; set; }

        /// <summary>
        /// Gets or sets the room types DbSet.
        /// </summary>
        public virtual DbSet<RoomTypeModel> RoomTypes { get; set; }

        /// <summary>
        /// Gets or sets the rooms DbSet.
        /// </summary>
        public virtual DbSet<RoomModel> Rooms { get; set; }

        /// <summary>
        /// Gets or sets the bookings DbSet.
        /// </summary>
        public virtual DbSet<BookingModel> Bookings { get; set; }

        /// <summary>
        /// Gets or sets the bills DbSet.
        /// </summary>
        public virtual DbSet<BillModel> Bills { get; set; }

        public virtual DbSet<BranchModel> Branches { get; set; }

        #endregion

        #region Override Methods

        /// <summary>
        /// Configures the database context options.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureUserModel(modelBuilder);
            ConfigureRoleModel(modelBuilder);
            ConfigureBookingModel(modelBuilder);
            ConfigureSoftDelete(modelBuilder);

            base.OnModelCreating(modelBuilder);
            new DbInitializer(modelBuilder).Seed();
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        public override int SaveChanges()
        {
            try
            {
                AddTimestamps();
                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Asynchronously saves all changes made in this context to the database.
        /// </summary>
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                AddTimestamps();
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Configures the UserModel entity.
        /// </summary>
        private static void ConfigureUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey("UserId")
                .IsRequired();

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.Jwts)
                .WithOne(e => e.User)
                .HasForeignKey("UserId")
                .IsRequired();

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.Posts)
                .WithOne(e => e.User)
                .HasForeignKey("UserId");

            modelBuilder.Entity<UserModel>()
                .HasMany(e => e.FromUserNotifications)
                .WithOne(e => e.FromUserNotification)
                .HasForeignKey("FromUserNotificationId")
                .OnDelete(DeleteBehavior.Restrict);
        }

        /// <summary>
        /// Configures the RoleModel entity.
        /// </summary>
        private static void ConfigureRoleModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleModel>()
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey("RoleId")
                .IsRequired();
        }

        /// <summary>
        /// Configures the BookingModel entity.
        /// </summary>
        private static void ConfigureBookingModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingModel>()
                .HasOne(b => b.Room)
                .WithMany()
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BookingModel>()
                .HasOne(b => b.Guest)
                .WithMany()
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        /// <summary>
        /// Configures soft delete for entities that implement it.
        /// </summary>
        private static void ConfigureSoftDelete(ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes();

            foreach (var entityType in entityTypes)
            {
                var entityClrType = entityType.ClrType;
                var isSoftDeleteEnabled = entityClrType.GetProperty("IsDeleted") != null;

                if (isSoftDeleteEnabled)
                {
                    var parameter = Expression.Parameter(entityClrType, "e");
                    var property = Expression.Property(parameter, "IsDeleted");
                    var notDeleted = Expression.Not(property);
                    var lambda = Expression.Lambda(notDeleted, parameter);

                    entityType.SetQueryFilter(lambda);
                }
            }
        }

        /// <summary>
        /// Adds timestamps to entities that inherit from BaseModel.
        /// </summary>
        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is BaseModel && 
                    (x.State == EntityState.Added || x.State == EntityState.Modified));

            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserId = !string.IsNullOrEmpty(userId) ? userId : "0";

            foreach (var entity in entities)
            {
                var now = GetCurrentTime();
                var user = int.Parse(currentUserId);

                if (entity.State == EntityState.Added)
                {
                    ((BaseModel)entity.Entity).CreatedAt = now;
                    ((BaseModel)entity.Entity).CreatedBy = user;
                }

                ((BaseModel)entity.Entity).UpdatedAt = now;
                ((BaseModel)entity.Entity).ChangedBy = user;
            }
        }

        /// <summary>
        /// Gets the current time in the Asia Standard Time zone.
        /// </summary>
        private static DateTime GetCurrentTime()
        {
            var nowUtc = DateTime.UtcNow;
            var asiaZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(nowUtc, asiaZone);
        }

        #endregion
    }
}