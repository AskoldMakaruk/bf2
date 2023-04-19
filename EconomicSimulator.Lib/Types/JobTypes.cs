public static class JobTypes
{
    public static readonly JobType collect_water_from_well_with_bucket =
        new JobType("набирати воду відром з джерела", "таак", "collect_water_w_bucket",
            WorkHoursNeeded: 1.5m,
            MinWorkers: 1,
            MaxWorkers: 3,
            new List<IOItem>(),
            new List<IOItem>() { ("water", 2) },
            new List<ToolType>()
            {
                "water_bucket"
            }
        );

    public static readonly JobType GrowWheat = new JobType()
    {
        Name = "grow_wheat",
        Description = "вирощувати пшеницю",
        TypeName = "grow_wheat",
        WorkHoursNeeded = 3,
        MinWorkers = 2,
        MaxWorkers = 3,
        Inputs = new IOItem[] { ("water", 10) },
        Outputs = new IOItem[] { ("wheat", 3) },
        Tools = new ToolType[] { "wheat_saw" }
    };

    private static readonly IReadOnlyCollection<JobType> _jobTypes = new[]
    {
        collect_water_from_well_with_bucket
    };

    public static JobType Get(string name)
    {
        return _jobTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}