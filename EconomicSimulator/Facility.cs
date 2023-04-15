using EconomicSimulator.Types;

public class Facility
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public FacilityType Type { get; set; }
    public Location Location { get; set; }
    public Inventory Inventory { get; set; }
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
            worker.WorkHours = new WorkHours(worker.WorkHours + 1);
            var job = JobQueue.SkipWhile(j => j.CurrentProgress > j.Type.WorkHoursNeeded).FirstOrDefault();
            if (job == null)
            {
                continue;
            }

            job.CurrentProgress += 1;
        }
    }

    public int GetCount(ItemType type)
    {
        var count = Inventory[type];
        return count;
    }

    public double? GetPrice(ItemType itemNeeded)
    {
        return Prices.FirstOrDefault(a => a.Item == itemNeeded)?.Price;
    }

    public IOItem? Buy(ItemType type, int buyCount, ref WorkHours hours)
    {
        var count = GetCount(type);
        if (GetPrice(type) is not { } price)
        {
            return null;
        }

        var maxToBuy = Math.Min((int)Math.Floor(hours / price), buyCount);
        if (maxToBuy == 0)
        {
            return null;
        }

        if (maxToBuy > count)
        {
            maxToBuy = count;
        }

        var hoursNeeded = price * maxToBuy;
        hours -= hoursNeeded;
        if (Inventory.TryRemoveItem(type, maxToBuy))
        {
            return new IOItem(type, maxToBuy);
        }

        return null;
    }

    public bool CanProduce(ItemType itemNeededItem)
    {
        return JobQueue.Any(a => a.Type.Outputs.Any(b => b.Item == itemNeededItem));
    }
}