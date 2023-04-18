using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.JavaScript;
using EconomicSimulator;
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

    public List<JobType> JobTypes { get; set; } = new();
    private List<Job> JobQueue { get; } = new();
    private List<JobPost> JobPosts { get; } = new();

    public List<Worker> Workers { get; set; } = new();
    public IEnumerable<Worker> FreeWorkers => Workers.Where(w => JobQueue.All(q => !q.Workers.Contains(w)));

    public bool TryQueueWorker(Worker worker, JobPost post)
    {
        if (JobPosts.FirstOrDefault(a => a.Type == post.Type) is not { } jobPost)
        {
            return false;
        }

        if (jobPost.Slots < 1)
        {
            return false;
        }

        jobPost.Slots--;
        Workers.Add(worker);
        var job = JobQueue.FirstOrDefault(a => a.Type == post.Type) ?? JobQueue.MaxBy(a => a.GetLeftToMaxWorkers());
        job?.TryAddWorker(worker);
        return true;
    }

    private void AddPost(JobType type)
    {
        JobPosts.Add(new JobPost(this, type, type.MaxWorkers - FreeWorkers.Count()));
    }

    public IEnumerable<JobPost> GetJobPosts()
    {
        return JobPosts.Where(a => a.Slots > 0);
    }

    public void AddRandomJobPost()
    {
        var jobType = JobTypes.RandomShuffle().FirstOrDefault();
        AddPost(jobType);
    }


    private void CollectCompleted()
    {
        var completedJobs = JobQueue.Where(j => j.CurrentProgress >= j.Type.WorkHoursNeeded).ToList();
        foreach (var job in completedJobs)
        {
            var completedCount = (int)(job.CurrentProgress / job.Type.WorkHoursNeeded);
            foreach (var (item, count) in job.Type.Outputs)
            {
                Inventory.Add(new(item, completedCount * count));
            }

            JobQueue.Remove(job);
            Balance = new WorkHours(Value: Balance.Value - job.CurrentProgress);
        }
    }

    public bool TryStartJob(JobType type)
    {
        if (JobQueue.Count >= Type.ConcurrentJobLimit)
        {
            return false;
        }

        var freeWorkers = FreeWorkers.ToList();
        if (freeWorkers.Count < type.MinWorkers)
        {
            return false;
        }

        var requirements = type.GetRequirements();
        if (requirements.NotEmpty())
        {
            if (!requirements.CanBeSatisfied(Inventory))
            {
                return false;
            }

            var proposals = requirements.GetProposals(Inventory).ToList();
            var proposal = proposals.RandomShuffle().FirstOrDefault();
            if (proposal == default || proposal.Proposal == null)
            {
                return false;
            }

            if (!Inventory.TryRemoveItems(proposal.Proposal))
            {
                return false;
            }
        }


        var job = new Job(type)
        {
            Workers = freeWorkers.Take(Math.Min(freeWorkers.Count, type.MaxWorkers)).ToList()
        };
        JobQueue.Add(job);
        // todo fix recursive starting to loop
        TryStartJob(job.Type);
        return true;
    }


    public void ProcessJobs()
    {
        CollectCompleted();
        foreach (var jobPost in JobPosts.Where(a => a.Slots < 1).ToList())
        {
            if (TryStartJob(jobPost.Type))
            {
                JobPosts.Remove(jobPost);
            }
        }

        if (JobPosts.Count + JobQueue.Count < Type.ConcurrentJobLimit)
        {
            AddRandomJobPost();
        }
        
        // todo add workers quitting if dissatisfied or facility overproduced and need to sell
        // Workers.RemoveAll(a => a.Status != WorkerStatus.Working);


        foreach (var job in JobQueue)
        {
            job.Process();
        }
    }

    public bool CanProduce(ItemType itemNeededItem)
    {
        return GetProducibleItems().Any(b => b == itemNeededItem);
    }

    public IEnumerable<ItemType> GetProducibleItems()
    {
        return JobTypes.SelectMany(a => a.Outputs).Select(a => a.Item);
    }

    public CanProduceAnswer CanProduce(ItemRequirement itemNeededItem)
    {
        var possibleProducts = itemNeededItem.Matches(GetProducibleItems()).ToList();
        var product = possibleProducts.FirstOrDefault();
        var job = JobTypes.FirstOrDefault(a => a.Outputs.Any(a => a.Item == product));
        if (job == null)
        {
            return new CanProduceAnswer(false, new(itemNeededItem));
        }

        return new CanProduceAnswer(possibleProducts.Any(), job.GetRequirements());
    }

    public IEnumerable<DeliveryRequirement> GetDeliveryRequirements()
    {
        var requirementsForNextFiveJobs = JobTypes.Select(a => a.Inputs.Select(a => new ItemRequirement(a.Item, a.Count * 5)));
        return requirementsForNextFiveJobs.Select(a => new DeliveryRequirement(this, a)).Where(a => a.NotEmpty() && !a.CanBeSatisfied(Inventory));
    }
}

public record CanProduceAnswer(bool Value, ItemRequirements Requirements);

public class DeliveryRequirement : ItemRequirements
{
    public Facility Facility { get; }

    public DeliveryRequirement(Facility facility, IEnumerable<ItemRequirement> requirements) : base(requirements)
    {
        Facility = facility;
    }
}