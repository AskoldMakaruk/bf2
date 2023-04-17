using EconomicSimulator;

public readonly record struct FacilityType(string Name, string Description, string TypeName, int ConcurrentJobLimit, List<JobType> Jobs)
{
    public static implicit operator FacilityType(string name) => FacilityTypes.Get(name);
}