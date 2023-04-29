using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

public class Job
{
    private Counter<Worker, HumanHours> _humanHoursMap = new();

    public Job(JobType type)
    {
        Id = Guid.NewGuid();
        Type = type;
    }

    public Guid Id { get; set; }
    public JobType Type { get; set; }
    public HumanHours CurrentProgress { get; set; }
    private List<Worker> Workers { get; set; } = new();

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

        _humanHoursMap.Add(worker, 0);
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
        if (Workers.Count < Type.MinWorkers)
        {
            Workers.Clear();
            return;
        }

        CurrentProgress += Workers.Count;
        _humanHoursMap.AddForEach(Workers.Count);
        GameStats.Post("workhours#total", Workers.Count);
    }

    public bool TryFinish(out JobResult result)
    {
        if (CurrentProgress < Type.WorkHoursNeeded)
        {
            result = new(false, new(), new());
            return false;
        }

        var completedTimes = (int)(CurrentProgress / Type.WorkHoursNeeded);

        result = new JobResult(true, _humanHoursMap, Type.Outputs.Select(a => a with
            {
                Count = a.Count * completedTimes
            })
            .ToList());
        CurrentProgress = 0;
        _humanHoursMap = new Counter<Worker, HumanHours>(Workers.ToDictionary(a => a, a => a.Balance));
        return true;
    }

    public bool IsWorkerFree(Worker wo) => !Workers.Contains(wo);
}


public record JobResult(bool IsOk, Dictionary<Worker, HumanHours> ProgressMade, List<IOItem> Products);