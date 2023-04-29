namespace EconomicSimulator.Lib.Types;

public static class FacilityTypes
{
    // public static readonly FacilityType Well = new()
    // {
    //     Name = "Колодязь",
    //     Description = "Джерело води",
    //     TypeName = "water_well",
    //     Jobs = new List<JobType>()
    //     {
    //         "collect_water_w_bucket"
    //     },
    //     ConcurrentJobLimit = 2
    // };
    //
    // public static readonly FacilityType GroundFarm = new FacilityType()
    // {
    //     Name = "Ферма",
    //     Description = "робить їжу",
    //     Jobs = new List<JobType>()
    //     {
    //         JobTypes.GrowWheat
    //     },
    //     TypeName = "ground_farm",
    //     ConcurrentJobLimit = 5
    // };

    // 
    public static readonly FacilityType Reef = new FacilityType()
    {
        Name = "Рудна жила",
        Description = "можно збирати залізну руду",
        Jobs = new List<JobType>()
        {
            JobTypes.CollectIronOre
        },
        TypeName = "iron_reef",
        ConcurrentJobLimit = 5
    };

    public static readonly FacilityType StoneQuery = new FacilityType()
    {
        Name = "Каменна запасна",
        Description = "Камень запасні",
        Jobs = new List<JobType>()
        {
            JobTypes.CollectStone
        },
        TypeName = "stone_query",
        ConcurrentJobLimit = 10
    };

    public static readonly IReadOnlyCollection<FacilityType> _facilityTypes = new[]
    {
        Reef, StoneQuery
        // Well, GroundFarm
    };

    public static FacilityType Get(string name)
    {
        return _facilityTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}