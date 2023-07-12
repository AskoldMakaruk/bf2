using Chess.Backend.Models;
using Chess.Backend.Services;
using Chess.Backend.Views;
using Microsoft.EntityFrameworkCore;

namespace Chess.Backend.Repositories;

public class LobbyRepository
{ 
    private readonly  ChessContext _context;
    
    public LobbyRepository(ChessContext context)
    {
            _context = context;
    }

    public async Task<bool> IsUserInLobbyAsync(Guid userId)
    {
        return await _context.Lobbies.Where(lobby=>lobby.IsActive).AnyAsync(x => x.Host.Id == userId || x.Opponent.Id == userId );
    }

    public async Task<Lobby> CreateLobbyAsync(LobbyCreateView view, User user)
    {
    var lobby = new Lobby()
    {
            Name = view.Name,
            Password = view.Password,
            IsActive = true,
            Color = view.Color,
            Host = user
    };
    
        await _context.Lobbies.AddAsync(lobby);
        await _context.SaveChangesAsync();
        return lobby;
    }
    
    public async Task<bool> IsLobbyFilledAsync(Guid lobbyId)
    {
       return  await _context.Lobbies.Where(lobby=>lobby.IsActive).AnyAsync(x => x.Id == lobbyId && x.Opponent != null);
    }
    public async Task<Lobby?> GetLobbyAsync(Guid lobbyId)
    {
        return  await _context.Lobbies.Where(lobby=>lobby.IsActive).FirstOrDefaultAsync(x => x.Id == lobbyId);
    }
}