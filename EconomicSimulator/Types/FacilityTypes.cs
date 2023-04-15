namespace EconomicSimulator;

public static class FacilityTypes
{
    public static readonly FacilityType Well = new()
    {
        Name = "Колодязь",
        Description = "Джерело води",
        TypeName = "water_well",
        Jobs = new List<JobType>()
        {
            "collect_water_w_bucket"
        }
    };

    private static readonly IReadOnlyCollection<FacilityType> _facilityTypes = new[]
    {
        Well
    };

    public static FacilityType Get(string name)
    {
        return _facilityTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}