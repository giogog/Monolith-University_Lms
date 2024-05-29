using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
{
    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable(nameof(Teacher));

        builder.HasKey(t => t.Id);

        builder.HasOne(t => t.User)
            .WithOne()
            .HasForeignKey<Teacher>(t => t.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
