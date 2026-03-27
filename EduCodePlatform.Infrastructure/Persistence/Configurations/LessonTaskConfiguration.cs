using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCodePlatform.Infrastructure.Persistence.Configurations
{
    public class LessonTaskConfiguration : IEntityTypeConfiguration<LessonTask>
    {
        public void Configure(EntityTypeBuilder<LessonTask> builder)
        {
            builder.ToTable("LessonTasks");

            builder.HasKey(u => u.Id);

            builder.HasOne(l => l.Lesson)
                .WithMany(s => s.LessonTasks)
                .HasForeignKey(l => l.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => l.LessonId);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.MarkdownContent)
                .IsRequired()
                .HasMaxLength(4200);

            builder.Property(u => u.ExpectedAnswer)
                .IsRequired()
                .HasMaxLength(1000);
        }
    }
}
