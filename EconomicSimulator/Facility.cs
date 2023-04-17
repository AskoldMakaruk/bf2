using EconomicSimulator.Interfaces;
using EconomicSimulator.Types;

public class Facility : ITrading
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public FacilityType Type { get; set; }
    public Location Location { get; set; }
    public Inventory Inventory { get; set; }
    public WorkHours Balance { get; set; }
    public List<SellingPrice> Prices { get; set; }

    public List<Job> JobQueue { get; } = new List<Job>();
    public List<Worker> Workers { get; set; } = new();

    public void QueueWorker(Worker worker)
    {
        Workers.Add(worker);
    }

    public void ProcessWorkers()
    {
        //restart completed jobs
        var completedJobs = JobQueue.Where(j => j.CurrentProgress >= j.Type.WorkHoursNeeded).ToList();
        foreach (var job in completedJobs)
        {
            job.CurrentProgress = 0;
            foreach (var (item, count) in job.Type.Outputs)
            {
                Inventory.Add(new(item, count));
            }
        }

        if (Workers.Count <= 0) return;
        Workers.RemoveAll(a => a.Status != WorkerStatus.Working);
        foreach (var worker in Workers)
        {
            worker.TotalExperience++;
            worker.Balance = new WorkHours(worker.Balance + 1);
            var job = JobQueue.SkipWhile(j => j.CurrentProgress > j.Type.WorkHoursNeeded).FirstOrDefault();
            if (job == null)
            {
                continue;
            }

            job.CurrentProgress += 1;
        }
    }

    public bool CanProduce(ItemType itemNeededItem)
    {
        return GetProducibleItems().Any(b => b == itemNeededItem);
    }

    public IEnumerable<ItemType> GetProducibleItems()
    {
        return JobQueue.SelectMany(a => a.Type.Outputs).Select(a => a.Item);
    }

    public CanProduceAnswer CanProduce(ItemRequirement itemNeededItem)
    {
        var possibleProducts = itemNeededItem.Matches(GetProducibleItems()).ToList();
        var product = possibleProducts.FirstOrDefault();
        var job = JobQueue.FirstOrDefault(a => a.IsProducing(product));
        if (job == null)
        {
            return new CanProduceAnswer(false, new(itemNeededItem));
        }

        return new CanProduceAnswer(possibleProducts.Any(), new(job.GetRequirements()));
    }
}

public record CanProduceAnswer(bool Value, ItemRequirements Requirements);

public class DeliveryRequirement : ItemRequirements
{
    public Facility DeliverTo { get; }

    public DeliveryRequirement(Facility facility, IEnumerable<ItemRequirement> requirements) : base(requirements)
    {
        DeliverTo = facility;
    }
}