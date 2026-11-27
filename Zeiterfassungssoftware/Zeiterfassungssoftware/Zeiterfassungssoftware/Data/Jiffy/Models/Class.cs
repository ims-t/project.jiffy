namespace Zeiterfassungssoftware.Data.Jiffy.Models;

public partial class Class
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<RequiredTime> RequiredTimes { get; set; } = new List<RequiredTime>();

    public override bool Equals(object? obj)
    {
        if (obj is not Class other)
            return false;

        return this.Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}