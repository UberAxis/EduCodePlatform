using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<LessonTask> LessonTasks => Set<LessonTask>();
        public DbSet<TaskSubmission> TaskSubmissions => Set<TaskSubmission>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var Entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in Entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(nameof(BaseEntity.CreatedAt)).CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(nameof(BaseEntity.UpdatedAt)).CurrentValue = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
