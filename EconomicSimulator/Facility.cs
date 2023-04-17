using System.Runtime.InteropServices.ComTypes;
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

    public void QueueWorker(Worker worker, JobPost post)
    {
        Workers.Add(worker);
        var job = JobQueue.FirstOrDefault(a => a.Type == post.Type) ?? JobQueue.MaxBy(a => a.GetLeftToMaxWorkers());
        job?.TryAddWorker(worker);

        var jobPost = JobPosts.FirstOrDefault(a => a.Type == post.Type);
        JobPosts.Remove(jobPost);
        AddPost(post.Type);
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
            foreach (var (item, count) in job.Type.Outputs)
            {
                Inventory.Add(new(item, count));
            }

            JobQueue.Remove(job);
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

        var job = new Job(type)
        {
            Workers = freeWorkers.Take(type.MinWorkers).ToList()
        };
        JobQueue.Add(job);
        return true;
    }


    public void ProcessJobs()
    {
        CollectCompleted();
        foreach (var jobPost in JobPosts.Where(a => a.Type.MinWorkers >= Workers.Count).ToList())
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
        // todo add workers quitting if dissatisfied
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