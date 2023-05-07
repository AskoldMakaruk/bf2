using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

public abstract class Department
{
    protected readonly IFacility Facility;

    public Department(IFacility facility)
    {
        Facility = facility;
    }
}

public class OperationalDepartment : Department
{
    private List<Job> JobQueue { get; } = new();

    public void ProcessJobs()
    {
        CollectCompleted();

        // todo add workers quitting if dissatisfied or facility overproduced and need to sell
        // Workers.RemoveAll(a => a.Status != WorkerStatus.Working);

        TryStartJob(Facility.Type.Jobs.FirstOrDefault());

        foreach (var job in JobQueue)
        {
            job.Process();
        }
    }

    private void CollectCompleted()
    {
        foreach (var job in JobQueue)
        {
            if (!job.TryFinish(out var result))
            {
                continue;
            }

            foreach (var ioItem in result.Products)
            {
                Facility.Inventory.Add(ioItem);
                GameStats.Post($"production#{ioItem.Item.TypeName}", ioItem.Count);
            }

            Facility.SubmitProgress(result);
        }
    }

    public bool TryStartJob(JobType type)
    {
        if (JobQueue.Count >= Facility.Type.ConcurrentJobLimit)
        {
            return false;
        }

        var freeWorkers = Facility.GetFreeWorkers().ToList();
        if (freeWorkers.Count < type.MinWorkers)
        {
            return false;
        }
        
        

        if (!TrySatisfyJobRequirements(type)) return false;


        var job = new Job(type);
        var free = freeWorkers.Take(Math.Min(freeWorkers.Count, type.MaxWorkers)).ToList();
        foreach (var freeWorker in free)
        {
            job.TryAddWorker(freeWorker);
        }

        JobQueue.Add(job);
        return true;
    }

    private bool TrySatisfyJobRequirements(JobType type)
    {
        var requirements = type.GetRequirements();
        if (!requirements.NotEmpty()) return true;
        if (!requirements.CanBeSatisfied(Facility.Inventory))
        {
            return false;
        }

        var proposals = requirements.GetProposals(Facility.Inventory).ToList();
        var proposal = proposals.RandomShuffle().FirstOrDefault();
        if (proposal == default || proposal.Proposal == null)
        {
            return false;
        }

        if (!Facility.Inventory.TryRemoveItems(proposal.Proposal))
        {
            return false;
        }

        return true;
    }

    public bool IsWorkerFree(Worker worker)
    {
        return JobQueue.All(q => !q.IsWorkerFree(worker));
    }

    public int ActiveJobCount => JobQueue.Count;

    public OperationalDepartment(IFacility facility) : base(facility)
    {
    }
}