using System.Collections.Concurrent;
namespace Chess.Backend.Services;


public class SessionService
{
    record TokenId(Guid _TokenId);
    record UserId(Guid _UserId);

    private ConcurrentDictionary<TokenId, UserId> Sessions { get; } = new();
    
    public Guid CreateSession(Guid userId)
    {
        var token = Guid.NewGuid();
        Sessions.TryAdd(new TokenId(token), new UserId(userId));
        return token;
    }
    
    public Guid? GetUserId(Guid token)
    {
        if (Sessions.TryGetValue(new TokenId(token), out var userId))
        {
            return userId._UserId;
        }
        return null;
    }
}