using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCodePlatform.Infrastructure.Persistence.Configurations
{
    public class TaskSubmissionConfiguration : IEntityTypeConfiguration<TaskSubmission>
    {
        public void Configure(EntityTypeBuilder<TaskSubmission> builder)
        {
            builder.ToTable("TaskSubmissions");

            builder.HasKey(u => u.Id);

            builder.HasOne(l => l.LessonTask)
                .WithMany(s => s.TaskSubmissions)
                .HasForeignKey(l => l.LessonTaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(l => l.LessonTaskId);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Answer)
                .IsRequired();

            builder.Property(u => u.Result)
                .IsRequired();
        }
    }
}
