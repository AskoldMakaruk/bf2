using EconomicSimulator;
using EconomicSimulator.Interfaces;

public class Worker : ITrading
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public long TotalExperience { get; set; }
    public Location Location { get; set; }

    // todo determine prices for worker
    public List<SellingPrice> Prices { get; } = new List<SellingPrice>();
    public Inventory Inventory { get; set; }
    public WorkHours Balance { get; set; }

    public List<WorkerNeed> Needs { get; set; }
    public WorkerStatus Status { get; set; }


    public void ProcessNeeds()
    {
        foreach (var need in Needs)
        {
            need.Progress.Value += 0.01m;

            if (need.IsNeeded() && Status == WorkerStatus.Idle)
            {
                Status = WorkerStatus.SatisfyNeed;
            }
        }
    }

    public bool NeedsSomething()
    {
        return Needs.Any(a => a.IsNeeded());
    }

    public IEnumerable<ItemRequirement> GetRequirements()
    {
        return Needs.Where(a => a.IsNeeded()).SelectMany(a => a.Type.Requirements.Requirements);
    }

    public void Consume()
    {
        foreach (var need in Needs)
        {
            if (need.TryToSatisfy(Inventory))
            {
                // Console.WriteLine($"need is in {need.Type.Name} satisfyed progess: {need.Progress}");
            }
        }
    }
}