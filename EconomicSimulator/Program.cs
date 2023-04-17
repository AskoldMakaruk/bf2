using EconomicSimulator;
using EconomicSimulator.Types;
using Geolocation;

Console.WriteLine(GeoCalculator.GetDistance(1.001, 1, 1, 1, distanceUnit: DistanceUnit.Meters));
var worker = new Worker()
{
    Id = Guid.NewGuid(),
    Location = new Location(1.001, 1),
    Name = "Kok",
    Inventory = new Inventory(),
    Needs = new List<WorkerNeed>()
    {
        "thirst", "hunger"
    },
    TotalExperience = 1,
    Balance = 0
};

var well1 = new Facility()
{
    Id = Guid.NewGuid(),
    Inventory = new Inventory(),
    Location = new Location(1, 1),
    Name = "Water Well 1",
    Type = "water_well",
    Prices = new List<SellingPrice>()
    {
        ("water", 1)
    },
    JobQueue =
    {
        new Job()
        {
            Id = Guid.NewGuid(),
            Type = "collect_water_w_bucket",
            CurrentProgress = 0
        }
    }
};

var farm1 = new Facility()
{
    Id = Guid.NewGuid(),
    Inventory = new Inventory(),
    Location = new Location(1, 1.0001),
    Name = "Farm 1",
    Type = "ground_farm",
    Prices = new List<SellingPrice>()
    {
        ("wheat", 3)
    },
    JobQueue =
    {
        new Job()
        {
            Id = Guid.NewGuid(),
            Type = JobTypes.GrowWheat,
            CurrentProgress = 0
        }
    },
    Workers = new List<Worker>()
};

var facilities = new[]
{
    well1,
    farm1
};

var map = new Map()
{
    Facilities = facilities.ToList(),
    Workers = new List<Worker>() { worker }
};

while (true)
{
    map.ProcessWorkers();
    map.ProcessFacilities();
    Console.WriteLine(map.Report());
    Console.ReadLine();
}


public class Location
{
    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static implicit operator Coordinate(Location location) => new Coordinate(location.Latitude, location.Longitude);
    public static implicit operator Location(Coordinate coordinate) => new Location(coordinate.Latitude, coordinate.Longitude);
}

public enum Compass
{
    North,
    South,
    East,
    West
}

public enum WorkerStatus
{
    Idle,
    Working,
    SeekingWork,
    SatisfyNeed
}

public record Progress(decimal Value)
{
    public decimal Value { get; set; } = Value;
    public static implicit operator Progress(decimal value) => new Progress(value);
    public static implicit operator decimal(Progress value) => value.Value;
}

public readonly record struct WorkHours(decimal Value)
{
    public static implicit operator WorkHours(decimal value) => new WorkHours(value);
    public static implicit operator decimal(WorkHours value) => value.Value;
}

public record SellingPrice(ItemType Item, WorkHours Price)
{
    public static implicit operator SellingPrice((string, decimal) tuple) => new(ItemTypes.Get(tuple.Item1), new WorkHours(tuple.Item2));
}

public readonly record struct IOItem(ItemType Item, int Count)
{
    public static implicit operator IOItem((string, int ) tuple) => new IOItem(ItemTypes.Get(tuple.Item1), tuple.Item2);
    public static implicit operator IOItem(KeyValuePair<ItemType, int> tuple) => new IOItem(tuple.Key, tuple.Value);

    public decimal GetPrice(SellingPrice price) => Count * price.Price;
}

public class Item
{
    public Guid Id { get; set; }
    public ItemType Type { get; set; }
}


public class Tool : Item
{
}

public class Job
{
    public Guid Id { get; set; }
    public JobType Type { get; set; }
    public WorkHours CurrentProgress { get; set; }

    public bool IsProducing(ItemType type)
    {
        return Type.Outputs.Any(a => a.Item == type);
    }

    public IEnumerable<ItemRequirement> GetRequirements()
    {
        return Type.Inputs.Select(a => new ItemRequirement(a.Item, a.Count));
    }
}