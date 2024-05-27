using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable(nameof(Student));

        builder.HasKey(s => s.Id);

        builder.HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Student>(s => s.Id)
            .IsRequired();

        builder.HasOne(s => s.Faculty)
            .WithMany(f => f.Students)
            .HasForeignKey(s => s.FacultyId)
            .IsRequired();
    }
}
