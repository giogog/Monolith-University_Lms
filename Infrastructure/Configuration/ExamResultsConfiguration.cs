using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
namespace Infrastructure.Configuration
{
    public class ExamResultsConfiguration : IEntityTypeConfiguration<ExamResults>
    {
        public void Configure(EntityTypeBuilder<ExamResults> builder)
        {
            builder.ToTable(nameof(ExamResults));

            builder.HasKey(x => x.Id);

            builder.Property(p => p.SubjectNames)
                .HasColumnName("SubjectNames")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<string[]>(v)).Metadata
                .SetValueComparer(new ValueComparer<string[]>(
                    (x, y) => x.SequenceEqual(y), // Compare two arrays for equality
                    c => c.GetHashCode(),         // Generate hash code for the array
                    c => c.ToArray()));

            builder.Property(p => p.SubjectGrades)
                .HasColumnName("SubjectGrades")
                .HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<int[]>(v)).Metadata
                .SetValueComparer(new ValueComparer<int[]>(
                    (x, y) => x.SequenceEqual(y), // Compare two arrays for equality
                    c => c.GetHashCode(),         // Generate hash code for the array
                    c => c.ToArray()));
        }
    }
}
