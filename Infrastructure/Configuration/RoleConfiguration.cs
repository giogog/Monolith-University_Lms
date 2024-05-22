using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;


public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasData(
            new List<Role>
            {
                new Role { Id = -1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = -2, Name = "Applicant", NormalizedName = "APPLICANT" },
                new Role { Id = -3, Name = "Student", NormalizedName = "STUDENT" },
                new Role { Id = -4, Name = "Teacher", NormalizedName = "TEACHER" },
            }
        );

        builder.HasMany(UserRole => UserRole.Users)
            .WithOne(user => user.Role)
            .HasForeignKey(user => user.RoleId)
            .IsRequired();


    }
}
