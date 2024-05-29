using System.ComponentModel.DataAnnotations;

namespace Domain.Props;

public record ScheduleProperty
{
    public DayOfWeek DayOfWeek { get; set; }

    [RegularExpression(@"^(?:[01]\d|2[0-3]):(?:[0-5]\d)$", ErrorMessage = "StartTime must be in the format HH:mm.")]
    public TimeSpan StartTime { get; set; }
    [RegularExpression(@"^(?:[01]\d|2[0-3]):(?:[0-5]\d)$", ErrorMessage = "EndTime must be in the format HH:mm.")]
    public TimeSpan EndTime { get; set; }
}
