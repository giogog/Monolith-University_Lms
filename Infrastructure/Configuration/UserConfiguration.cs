using Domain.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(e => e.Id);
        builder.Ignore(e => e.TwoFactorEnabled);

        builder.HasOne(u => u.ExamResults)
            .WithOne()
            .HasForeignKey<ExamResults>(e => e.Id)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade); 


        builder.HasMany(UserRole => UserRole.Roles)
            .WithOne(user => user.User)
            .HasForeignKey(user => user.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); 

    }
}
