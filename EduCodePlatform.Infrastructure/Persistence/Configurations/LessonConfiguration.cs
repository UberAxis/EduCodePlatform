using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Infrastructure.Persistence.Configurations
{
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(u => u.Id);

            builder.HasOne(l => l.Module)
                .WithMany(s => s.Lessons)
                .HasForeignKey(l => l.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => l.ModuleId);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.CoverImage)
                .IsRequired(false)
                .HasMaxLength(255);

            builder.Property(u => u.MarkdownContent)
                .IsRequired()
                .HasMaxLength(4200);

            builder.Property(u => u.OrderIndex)
                .IsRequired();
        }
    }
}
