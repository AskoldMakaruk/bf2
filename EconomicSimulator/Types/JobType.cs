public readonly record struct JobType(string Name,
    string Description,
    string TypeName,
    double WorkHoursNeeded,
    int MinWorkers,
    int MaxWorkers,
    IEnumerable<IOItem> Inputs,
    IEnumerable<IOItem> Outputs,
    IEnumerable<ToolType> Tools)
{
    public static implicit operator JobType(string name) => JobTypes.Get(name);
}