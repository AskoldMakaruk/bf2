using System.Data;
using EconomicSimulator.Types;

public class Worker
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long TotalExperience { get; set; }
    public WorkHours WorkHours { get; set; }
    public Location Location { get; set; }
    public Inventory Inventory { get; set; }

    public List<WorkerNeed> Needs { get; set; }
    public WorkerStatus Status { get; set; }

    public bool NeedsSomething()
    {
        return Needs.Any(a => a.IsNeeded());
    }

    public IEnumerable<ItemRequirement> GetRequirements()
    {
        return Needs.Where(a => a.IsNeeded()).SelectMany(a => a.Type.Requirements);
    }

    public void Consume()
    {
        foreach (var need in Needs)
        {
            if (need.TryToSatisfy(Inventory))
            {
                Console.WriteLine($"need is in {need.Type.Name} satisfyed progess: {need.Progress}");
            }
        }
    }
}