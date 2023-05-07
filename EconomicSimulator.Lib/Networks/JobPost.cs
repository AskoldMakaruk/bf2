using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib;

public record JobPost(Guid Guid, IFacility Facility, JobType Type, int Slots)
{
    public virtual bool Equals(JobPost? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Guid.Equals(other.Guid);
    }

    public override int GetHashCode()
    {
        return Guid.GetHashCode();
    }

    private int _slots = Slots;

    public JobPost(IFacility Facility, JobType Type, int Slots) : this(Guid.NewGuid(), Facility, Type, Slots)
    {
    }

    public int Slots
    {
        get => _slots;
        set => _slots = value;
    }
};