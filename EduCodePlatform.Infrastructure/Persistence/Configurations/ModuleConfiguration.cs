using EduCodePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EduCodePlatform.Infrastructure.Persistence.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Models");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(s => s.CoverImage)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Description)
                .IsRequired()
                .HasMaxLength(4200);

            builder.Property(u => u.OrderIndex)
                .IsRequired();
        }
    }
}
