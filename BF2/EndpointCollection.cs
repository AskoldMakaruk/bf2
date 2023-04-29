using Telegram.Bot.Types;

namespace BF2;

public class EndpointCollection
{
    public readonly IReadOnlyCollection<Endpoint> Endpoints;

    public EndpointCollection(IEnumerable<Endpoint> endpoints)
    {
        Endpoints = endpoints.ToList();
    }

    public Endpoint? Suitable(Update update) => Endpoints.FirstOrDefault(a => a.IsSuitable(update));
}