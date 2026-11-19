using Zeiterfassungssoftware.SharedData.Users;

namespace Zeiterfassungssoftware.SharedData.RequiredTimes
{
    public class RequiredTimeDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Should { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is RequiredTimeDto other)
                return other.Id == this.Id;
            return false;
        }

    }
}
