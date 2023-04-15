public static class JobTypes
{
    public static readonly JobType collect_water_from_well_with_bucket =
        new JobType("набирати воду відром з джерела", "таак", "collect_water_w_bucket",
            WorkHoursNeeded: 1.5,
            MinWorkers: 1,
            MaxWorkers: 3,
            new List<IOItem>(),
            new List<IOItem>() { ("water", 2) },
            new List<ToolType>()
            {
                "water_bucket"
            }
        );

    private static readonly IReadOnlyCollection<JobType> _jobTypes = new[]
    {
        collect_water_from_well_with_bucket
    };

    public static JobType Get(string name)
    {
        return _jobTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}