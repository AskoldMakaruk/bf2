using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Properties;
using EconomicSimulator.Lib.Types;
using Geolocation;

namespace EconomicSimulator.Lib;

public static class Map
{
    private static Facility? GetProducerNear(Location workerLocation, ItemRequirement itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetOutputs().Any(itemNeededItem.Matches));
    }

    private static Facility? GetProducerNear(Location workerLocation, ItemRequirements requirements)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetOutputs().Any(type => requirements.Requirements.Any(a => a.Matches(type))));
    }

    private static Facility? GetProducerNear(Location workerLocation, ItemType itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.CanProduce(itemNeededItem));
    }

    public static List<Facility> GetFacilitiesNear(Location location)
    {
        return Game.Facilities.Where(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters) < 5000).ToList();
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