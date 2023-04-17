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

    public static readonly FacilityType GroundFarm = new FacilityType()
    {
        Name = "Ферма",
        Description = "робить їжу",
        Jobs = new List<JobType>()
        {
        },
        TypeName = "ground_farm"
    };

    private static readonly IReadOnlyCollection<FacilityType> _facilityTypes = new[]
    {
        Well, GroundFarm
    };

    public static FacilityType Get(string name)
    {
        return _facilityTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}