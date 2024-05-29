using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class SeminarConfiguration : IEntityTypeConfiguration<Seminar>
{
    public void Configure(EntityTypeBuilder<Seminar> builder)
    {
        builder.ToTable(nameof(Seminar));

        builder.HasKey(e => e.Id);

        builder.HasOne(s => s.Teacher)
            .WithMany(t => t.Seminars)
            .HasForeignKey(s => s.TeacherId)
            .IsRequired();

        builder.HasMany(s => s.StudentEnrollments)
            .WithOne(se => se.Seminar)
            .HasForeignKey(se => se.SeminarId)
            .IsRequired(false);

        builder.OwnsOne(seminar => seminar.Schedule, schedule =>
        {
            schedule.Property(s => s.DayOfWeek).IsRequired();
            schedule.Property(s => s.StartTime).IsRequired();
            schedule.Property(s => s.EndTime).IsRequired();

            // If you want to store StartTime and EndTime as strings in the format HH:mm
            schedule.Property(s => s.StartTime)
                    .HasConversion(
                        v => v.ToString(@"hh\:mm"),
                        v => TimeSpan.ParseExact(v, @"hh\:mm", null));
            schedule.Property(s => s.EndTime)
                    .HasConversion(
                        v => v.ToString(@"hh\:mm"),
                        v => TimeSpan.ParseExact(v, @"hh\:mm", null));
        });

    }
}
