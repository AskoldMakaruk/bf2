using EconomicSimulator;
using EconomicSimulator.Interfaces;
using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Types;

public class Worker : ITrading
{
    protected bool Equals(Worker other)
    {
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Worker)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public long TotalExperience { get; set; }
    public Location Location { get; set; }

    // todo determine prices for worker
    public Dictionary<ItemType, HumanHours> Prices { get; } = new Counter<ItemType, HumanHours>();
    public Inventory Inventory { get; set; }
    public HumanHours Balance { get; set; }

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

    public void ReceivePayout(Payout? payout)
    {
        if (payout == null)
        {
            // todo quit job
            return;
        }

        Inventory.AddRange(payout.IoItems);
        Balance += payout.Hours;
    }
}