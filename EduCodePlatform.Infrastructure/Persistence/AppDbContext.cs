using EduCodePlatform.Domain.Entities;
using EduCodePlatform.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduCodePlatform.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
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
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        SetTimestamp(entry.Entity, "CreatedAt", now);
                        break;

                    case EntityState.Modified:
                        SetTimestamp(entry.Entity, "UpdatedAt", now);
                        break;
                }
            }

            void SetTimestamp(object entity, string prop, DateTime date)
            {
                if (entity is BaseEntity or User)
                {
                    var property = entity.GetType().GetProperty(prop);
                    property?.SetValue(entity, date);
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
