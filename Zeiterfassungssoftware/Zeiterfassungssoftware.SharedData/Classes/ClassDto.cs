using Zeiterfassungssoftware.SharedData.RequiredTimes;
using Zeiterfassungssoftware.SharedData.Users;

namespace Zeiterfassungssoftware.SharedData.Classes
{
    public class ClassDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<RequiredTimeDto> RequiredTimes { get; set; } = new();

        public override bool Equals(object? obj)
        {
            if (obj is ClassDto other)
                return other.Id == this.Id;
            return false;
        }

    }
}
