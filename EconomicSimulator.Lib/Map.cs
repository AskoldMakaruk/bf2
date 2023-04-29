using System.Text;
using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Properties;
using EconomicSimulator.Lib.Types;
using Geolocation;

namespace EconomicSimulator.Lib;

public class Country
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<ItemType, HumanHours> Wages { get; private set; } = new();
}

public static class Map
{
    public static List<Facility> Facilities { get; set; }
    public static List<Worker> Workers { get; set; }

    private static IEnumerable<JobPost> PreviousJobs = Array.Empty<JobPost>();

    public static void ProcessWorkers()
    {
        foreach (var worker in Workers)
        {
            worker.Process();
        }
    }

    private static Facility? GetProducerNear(Location workerLocation, ItemRequirement itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetOutputs().Any(itemNeededItem.Matches));
    }

    public static IEnumerable<JobPost> GetJobPost(ItemRequirements requirements)
    {
        return PreviousJobs.Where(a => a.Slots > 0).Where(a => requirements.CanBeSatisfied(new ManyItems(a.Type.Outputs)));
    }


    private static Facility? GetProducerNear(Location workerLocation, ItemRequirements requirements)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetOutputs().Any(type => requirements.Requirements.Any(a => a.Matches(type))));
    }

    public static void ProcessFacilities()
    {
        var posts = new LinkedList<JobPost>();
        foreach (var facility in Facilities)
        {
            facility.Process();
            foreach (var post in facility.GetJobPosts())
            {
                posts.AddLast(post);
            }
        }

        PreviousJobs = posts;
    }

    private static Facility? GetProducerNear(Location workerLocation, ItemType itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.CanProduce(itemNeededItem));
    }

    public static string Report()
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

    public static List<Facility> GetFacilitiesNear(Location location)
    {
        return Facilities.Where(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters) < 5000).ToList();
    }


    public static IEnumerable<ItemType> ProducedNear(Location location)
    {
        return GetFacilitiesNear(location).SelectMany(a => a.GetOutputs());
    }

    public static Facility? GetClosestFacilityWithRequirement(Location location, ItemRequirement itemRequirement)
    {
        var facilities = GetFacilitiesNear(location);
        return facilities.OrderBy(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters))
            .FirstOrDefault(a => itemRequirement.GetProposals(a.Inventory).Any());
    }
}