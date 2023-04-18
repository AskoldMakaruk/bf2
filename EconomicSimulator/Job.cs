using EconomicSimulator.Types;

public class Job
{
    public Job(JobType type)
    {
        Id = Guid.NewGuid();
        Type = type;
    }

    public Guid Id { get; set; }
    public JobType Type { get; set; }
    public WorkHours CurrentProgress { get; set; }
    public List<Worker> Workers { get; set; } = new();

    public bool IsProducing(ItemType type)
    {
        return Type.Outputs.Any(a => a.Item == type);
    }


    public bool TryAddWorker(Worker worker)
    {
        if (Workers.Count >= Type.MaxWorkers)
        {
            return false;
        }

        Workers.Add(worker);
        return true;
    }

    public int GetWorkersNeeded()
    {
        return Math.Max(Type.MinWorkers - Workers.Count, 0);
    }

    public int GetLeftToMaxWorkers()
    {
        return Type.MaxWorkers - Workers.Count;
    }

    public void Process()
    {
        foreach (var worker in Workers)
        {
            worker.TotalExperience++;
            worker.Balance = new WorkHours(worker.Balance + 1);
            CurrentProgress += 1;
        }
    }
}