using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Properties;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib;

public class StartStatic
{
    public static void Start()
    {
        var reef = new Facility()
        {
            Id = Guid.NewGuid(),
            Location = new Location(1, 1),
            Name = "Iron Reef",
            Type = "iron_reef",
            Prices = new Dictionary<ItemType, HumanHours>()
            {
                { "iron_ore", 40 },
                { "water", 9 }
            },
            JobTypes =
            {
                "pickup_iron_ore",
            }
        };

        var stone_query = new Facility()
        {
            Id = Guid.NewGuid(),
            Location = new Location(1, 1.0001),
            Name = "Farm 1",
            Type = "stone_query",
            Prices = new Dictionary<ItemType, HumanHours>()
            {
                { "stone", 20 }
            },
            JobTypes =
            {
                JobTypes.CollectStone
            },
            Workers = new List<Worker>(),
            Balance = 1000m,
        };

        var facilities = new[]
        {
            reef,
            stone_query
        };

        Game.Facilities = facilities.ToList();
        Game.Workers = new[]
            {
                "KOK", "balls", "Sir de La CUm", "Cumbotron 4000", "la fishe un chocolate", "hui", "pisun",
                "joker", "totoro", "kupalnik", "kokroach", "oposum", "openheimer", "chad", "broski", "lemon",
                "dick", "penis"
            }.Select(CreateWorker)
            .ToList();
        return;
        while (true)
        {
            Run();
        }

        Worker CreateWorker(string name)
        {
            return new Worker()
            {
                Id = Guid.NewGuid(),
                Location = new Location(1.001, 1),
                Name = name,
                Inventory = new Inventory(),
                Needs = new List<WorkerNeed>()
                {
                    "thirst", "hunger"
                },
                TotalExperience = 1,
                Balance = 0
            };
        }
    }

    public static void Run()
    {
        Game.Process();
    }
}