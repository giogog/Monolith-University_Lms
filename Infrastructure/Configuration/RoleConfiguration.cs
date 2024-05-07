using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;


public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("AspNetRoles");
        builder.HasData(
            new List<Role>
            {
                new Role { Id = -1, Name = "Admin", NormalizedName = "ADMIN" },
                new Role { Id = -2, Name = "User", NormalizedName = "USER" },
            }
        );

        builder.HasMany(UserRole => UserRole.Users)
            .WithOne(user => user.Role)
            .HasForeignKey(user => user.RoleId)
            .IsRequired();


    }
}
