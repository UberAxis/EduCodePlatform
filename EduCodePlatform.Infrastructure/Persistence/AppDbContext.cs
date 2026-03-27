using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public DbSet<Module> Modules => Set<Module>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<LessonTask> LessonTasks => Set<LessonTask>();
        public DbSet<TaskSubmission> TaskSubmissions => Set<TaskSubmission>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State is not (EntityState.Added or EntityState.Modified))
                    continue;

                var now = DateTime.UtcNow;
                var isAdded = entry.State == EntityState.Added;

                switch (entry.Entity)
                {
                    case BaseEntity:
                        entry.Property(isAdded ? "CreatedAt" : "UpdatedAt").CurrentValue = now;
                        break;

                    case User user:
                        if (isAdded) user.CreatedAt = now;
                        else user.UpdatedAt = now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
