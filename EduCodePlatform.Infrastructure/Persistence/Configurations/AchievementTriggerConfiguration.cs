using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCodePlatform.Infrastructure.Persistence.Configurations
{
    public class AchievementTriggerConfiguration : IEntityTypeConfiguration<AchievementTrigger>
    {
        public void Configure(EntityTypeBuilder<AchievementTrigger> builder)
        {
            builder.ToTable("AchievementTriggers");

            builder.HasKey(at => at.Id);

            builder.Property(at => at.Id)
                .ValueGeneratedOnAdd();

            builder.Property(at => at.TriggerType)
                .IsRequired();

            builder.Property(at => at.RequiredValue)
                .IsRequired();

            builder.Property(at => at.IsActive)
                .HasDefaultValue(true);

            // Foreign key to Achievement
            builder.HasOne(at => at.Achievement)
                .WithMany(a => a.Triggers)
                .HasForeignKey(at => at.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional foreign key to Module
            builder.HasOne(at => at.TargetModule)
                .WithMany()
                .HasForeignKey(at => at.TargetModuleId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Optional foreign key to Lesson
            builder.HasOne(at => at.TargetLesson)
                .WithMany()
                .HasForeignKey(at => at.TargetLessonId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        }
    }
}
