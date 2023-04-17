using System.Text;
using EconomicSimulator;
using EconomicSimulator.Interfaces;
using EconomicSimulator.Types;
using Geolocation;


public readonly record struct JobPost(Facility Facility, JobType Type, int Slots);

public class Map
{
    public List<Facility> Facilities { get; set; }
    public List<Worker> Workers { get; set; }
    private IEnumerable<JobPost> PreviousJobs = Array.Empty<JobPost>();

    public void ProcessWorkers()
    {
        foreach (var worker in Workers)
        {
            worker.ProcessNeeds();

            foreach (var requirement in worker.GetRequirements())
            {
                if (GetClosestFacilityWithRequirement(worker.Location, requirement) is not { } facilityNear) continue;
                if (facilityNear.CanProduce(requirement) is { Value: true } answer)
                {
                    worker.Deliverys.Add(new DeliveryRequirement(facilityNear, answer.Requirements.Requirements));
                }

                ITrading me = worker;
                ITrading trader = facilityNear;
                if (me.TryBuyFrom(trader, requirement))
                {
                    Console.WriteLine("happy purcashe");
                }
            }

            foreach (var delivery in worker.Deliverys)
            {
                foreach (var requirement in delivery.GetSatisfiedBy(worker.Inventory))
                {
                }
            }

            worker.Consume();
            if (!worker.NeedsSomething())
            {
                worker.Status = WorkerStatus.Idle;
            }

            if (worker.Status == WorkerStatus.SeekingWork)
            {
                var jobPost = GetJobPost(new ItemRequirements(worker.GetRequirements()));
                if (jobPost != null)
                {
                    worker.Status = WorkerStatus.Working;
                    jobPost.Value.Facility.QueueWorker(worker, jobPost.Value);
                }
            }

            if (worker.Status == WorkerStatus.SatisfyNeed)
            {
                worker.Status = WorkerStatus.SeekingWork;
            }
        }
    }

    private JobPost? GetJobPost(ItemRequirements requirements)
    {
        return PreviousJobs.FirstOrDefault(a => requirements.CanBeSatisfied(new ManyItems(a.Type.Outputs)));
    }


    private Facility? GetProducerNear(Location workerLocation, IEnumerable<ItemRequirement> items)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetProducibleItems().Any(type => items.Any(a => a.Matches(type))));
    }

    public void ProcessFacilities()
    {
        var posts = new LinkedList<JobPost>();
        foreach (var facility in Facilities)
        {
            facility.ProcessJobs();
            foreach (var post in facility.GetJobPosts())
            {
                posts.AddLast(post);
            }
        }

        PreviousJobs = posts;
    }

    private Facility? GetProducerNear(Location workerLocation, ItemType itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.CanProduce(itemNeededItem));
    }

    public string Report()
    {
        var report = new StringBuilder();
        report.AppendLine("Facilities:");
        foreach (var facility in Facilities)
        {
            report.AppendLine($"  {facility.Name} - {facility.Type} - {facility.Inventory.Report()}");
        }

        report.AppendLine("Workers:");
        foreach (var worker in Workers)
        {
            report.AppendLine($"  {worker.Name} - {worker.Status} - {worker.TotalExperience} - {worker.Balance} - {string.Join(",", worker.Needs.Select(a => a.Type.Name + " " + a.Progress.Value))}");
            report.AppendLine($"  Інвентар:");
            foreach (var item in worker.Inventory.Items)
            {
                report.AppendLine($"    {item.Key.Name} - {item.Value}");
            }
        }

        return report.ToString();
    }

    public List<Facility> GetFacilitiesNear(Location location)
    {
        return Facilities.Where(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters) < 5000).ToList();
    }

    public IEnumerable<ItemType> ProducedNear(Location location)
    {
        return GetFacilitiesNear(location).SelectMany(a => a.GetProducibleItems());
    }

    public Facility? GetClosestFacilityWithRequirement(Location location, ItemRequirement itemRequirement)
    {
        var facilities = GetFacilitiesNear(location);
        return facilities.OrderBy(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters))
            .FirstOrDefault(a => itemRequirement.GetProposals(a.Inventory).Any());
    }
}