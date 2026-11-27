namespace Zeiterfassungssoftware.Data.Jiffy.Models
{
    public partial class RequiredTime
    {
        public Guid Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Required { get; set; }
        public DateTime ValidUntil { get; set; }
        public Guid ClassId { get; set; }
        public virtual Class Class { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj is not RequiredTime other)
                return false;

            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
