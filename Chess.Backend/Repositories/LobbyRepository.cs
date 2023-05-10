using Chess.Backend.Models;
using Chess.Backend.Services;
using Chess.Backend.Views;

namespace Chess.Backend.Repositories;

public class LobbyRepository
{ 
    private readonly  ChessContext _context;
    
    public LobbyRepository(ChessContext context)
    {
            _context = context;
    }
    
    
    public async Task<Lobby> CreateLobbyAsync(LobbyCreateView view)
    {
    var lobby = new Lobby()
    {
            Name = view.Name,
            Password = view.Password,
            
            
    };
    
        await _context.Lobbies.AddAsync(lobby);
        await _context.SaveChangesAsync();
        return lobby;
    }
}