using Zeiterfassungssoftware.SharedData.Users;

namespace Zeiterfassungssoftware.SharedData.Times
{
    public class TimeEntryDto
    {
        private const string TIME_FORMAT = "HH:mm";
        //private const string DURATION_FORMAT = @"h\h\ m\m\i\n\ s\s";
        private const string DATE_FORMAT = "dd MMM yyyy";

        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        
        public TimeSpan Time => (End != null ? End - Start : DateTime.Now-Start).Value;
        public TimeSpan ShouldTime { get; set; }
        public TimeSpan Overtime => Time - ShouldTime;


        
        public string GetTimeString()
        {
            return FormatTime(Time);
        }

        public static string FormatTime(TimeSpan time)
        {
            var hours = (int)time.TotalHours;
            var minutes = Math.Abs(time.Minutes);

            if (hours != 0)
                return $"{hours} h {minutes} min";

            if (minutes != 0)
                return $"{(int)time.TotalMinutes} min";

            return $"{time.Seconds} s";
        }

        public string GetStartString()
        {
            return Start.ToString(TIME_FORMAT);
        }

        public string GetEndString()
        {
            return End?.ToString(TIME_FORMAT) ?? "?";
        }


        public string GetDateString()
        {
            return Start.ToString(DATE_FORMAT);
        }

        public override bool Equals(object? obj)
        {
            if (obj is TimeEntryDto other)
                return other.Id == this.Id;
            return false;
        }

    }
}
