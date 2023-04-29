namespace EconomicSimulator.Lib.Types;

public readonly record struct FacilityType(string Name,
    string Description,
    string TypeName,
    int ConcurrentJobLimit,
    List<JobType> Jobs,
    Payouts Payouts)
{
    public static implicit operator FacilityType(string name) => FacilityTypes.Get(name);
}

public readonly record struct Payouts(decimal MaxSubstitution);