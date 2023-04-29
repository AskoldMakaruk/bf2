using System.Collections.Immutable;
using EconomicSimulator.Lib.Entities;

namespace EconomicSimulator.Lib.Types;

public static class JobTypes
{
    // public static readonly JobType collect_water_from_well_with_bucket =
    //     new JobType("набирати воду відром з джерела", "таак", "collect_water_w_bucket",
    //         WorkHoursNeeded: 1.5m,
    //         MinWorkers: 1,
    //         MaxWorkers: 3,
    //         new List<IOItem>(),
    //         new List<IOItem>() { ("water", 2) },
    //         new List<ToolType>()
    //         {
    //             "water_bucket"
    //         }
    //     );
    //
    // public static readonly JobType GrowWheat = new JobType()
    // {
    //     Name = "grow_wheat",
    //     Description = "вирощувати пшеницю",
    //     TypeName = "grow_wheat",
    //     WorkHoursNeeded = 3,
    //     MinWorkers = 2,
    //     MaxWorkers = 3,
    //     Inputs = new IOItem[] { ("water", 10) },
    //     Outputs = new IOItem[] { ("wheat", 3) },
    //     Tools = new ToolType[] { "wheat_saw" }
    // };

    public static readonly JobType CollectIronOre = new JobType()
    {
        Name = "pickup_iron_ore",
        Description = "збирати руду",
        TypeName = "pickup_iron_ore",
        WorkHoursNeeded = 3,
        MinWorkers = 2,
        MaxWorkers = 3,
        Inputs = ImmutableArray<IOItem>.Empty,
        Outputs = new IOItem[] { ("iron_ore", 3) },
        Tools = ImmutableArray<ToolType>.Empty
    };

    public static readonly JobType CollectStone = new JobType()
    {
        Name = "pickup_stone",
        Description = "збирати камінь",
        TypeName = "pickup_stone",
        WorkHoursNeeded = 3,
        MinWorkers = 2,
        MaxWorkers = 3,
        Inputs = ImmutableArray<IOItem>.Empty,
        Outputs = new IOItem[] { ("stone", 3) },
        Tools = ImmutableArray<ToolType>.Empty
    };

    public static readonly IReadOnlyCollection<JobType> _jobTypes = new[]
    {
        CollectIronOre,
        CollectStone
        // collect_water_from_well_with_bucket, GrowWheat
    };

    public static JobType Get(string name)
    {
        return _jobTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}