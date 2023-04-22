using EconomicSimulator.Interfaces;
using EconomicSimulator.Types;

namespace EconomicSimulator.Lib.Entities;

public class Facility : ITrading, IFacility
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public FacilityType Type { get; set; }
    public Location Location { get; set; }
    public Dictionary<ItemType, HumanHours> Prices { get; set; }
    public Inventory Inventory { get; set; }

    public List<JobType> JobTypes { get; set; } = new();

    public List<Worker> Workers { get; set; } = new();

    private readonly AccountantDepartment AccountantDepartment;
    private readonly OperationalDepartment OperationalDepartment;
    private readonly HrDepartment HrDepartment;

    public Facility()
    {
        HrDepartment = new HrDepartment(this);
        AccountantDepartment = new AccountantDepartment(this);
        OperationalDepartment = new OperationalDepartment(this);
    }

    public void AddRandomJobPost()
    {
        if (HrDepartment.GetJobPosts().Count() + OperationalDepartment.ActiveJobCount >= Type.ConcurrentJobLimit)
        {
            return;
        }

        var jobType = JobTypes.RandomShuffle().FirstOrDefault();
        HrDepartment.AddPost(jobType);
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

    public IEnumerable<Worker> GetFreeWorkers() => Workers.Where(w => OperationalDepartment.IsWorkerFree(w));

    public bool TryHire(Worker worker, JobPost post) => HrDepartment.TryHire(worker, post);


    public HumanHours? GetPrice(ItemType itemType)
    {
        return Prices.FirstOrDefault(a => a.Key == itemType).Value;
    }

    public void Income(HumanHours humanHours)
    {
        //todo 
    }

    public void SubmitProgress(JobResult result)
    {
        AccountantDepartment.ProcessPayouts(result);
    }

    public HumanHours Balance { get; set; }

    public void Process()
    {
        OperationalDepartment.ProcessJobs();
        AddRandomJobPost();
    }

    public IEnumerable<JobPost> GetJobPosts()
    {
        return HrDepartment.GetJobPosts();
    }
}