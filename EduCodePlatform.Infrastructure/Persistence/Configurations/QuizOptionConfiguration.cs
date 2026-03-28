using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EduCodePlatform.Domain.Entities;

namespace EduCodePlatform.Infrastructure.Persistence.Configurations
{
    public class QuizOptionConfiguration : IEntityTypeConfiguration<QuizOption>
    {
        public void Configure(EntityTypeBuilder<QuizOption> builder)
        {
            builder.ToTable("QuizOptions");

            builder.HasKey(q => q.Id);

            builder.HasOne(q => q.LessonTask)
                .WithMany(t => t.QuizOptions)
                .HasForeignKey(q => q.LessonTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(q => q.Text)
                .IsRequired()
                .HasMaxLength(500);
        }
    }
}
