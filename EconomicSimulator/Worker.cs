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

    public ItemType? GetNeededItemType()
    {
        return GetNeededItem()?.Item;
    }

    public IEnumerable<IOItem> GetNeededItems()
    {
        return Needs.Where(a => a.IsNeeded()).Select(a => a.Type.Items.FirstOrDefault());
    }

    public IOItem? GetNeededItem()
    {
        return Needs.FirstOrDefault(a => a.IsNeeded())?.Type.Items.FirstOrDefault();
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