using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder.ToTable(nameof(University));

        builder.HasKey(x => x.Id);

        builder.HasMany(u => u.Faculties)
            .WithOne(u => u.University)
            .HasForeignKey(f => f.UniversityId)
            .IsRequired();


        builder.HasData(
            new List<University>
            {
                new University { Id = 1, Name = "Gondor State University", SemesterPayment = 1125,CreditsToGraduate = 240, SubjectPayment = 200 }
            });


    }
}
