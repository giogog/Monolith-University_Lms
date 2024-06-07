using Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Configuration;

public class LectureConfiguration : IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable(nameof(Lecture));

        builder.HasKey(x => x.Id);

        builder.HasOne(l => l.Teacher)
            .WithMany(t=>t.Lectures)
            .HasForeignKey(t => t.TeacherId)
            .IsRequired();


        builder.HasMany(l =>l.StudentEnrollments)
            .WithOne(se=>se.Lecture)
            .HasForeignKey(se=>se.LectureId)
            .IsRequired(false);

        builder.OwnsOne(lecture => lecture.Schedule, schedule =>
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
