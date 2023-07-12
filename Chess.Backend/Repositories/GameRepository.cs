using Chess.Backend.Models;
using Chess.Backend.Services;
using Chess.Backend.Views;

namespace Chess.Backend.Repositories;

public class GameRepository
{
    private readonly ChessContext _context;

    public GameRepository(ChessContext context)
    {
        _context = context;
    }

    public async Task<Game> StartGameAsync(Lobby lobby)
    {
        User white = null;
        User black = null;
        switch (lobby.Color)
        {
            case HostColor.White:
                white = lobby.Host;
                black = lobby.Opponent!;
                break;
            case HostColor.Black:
                black = lobby.Host;
                white = lobby.Opponent!; 
                break;
            case HostColor.Random:
                var random = new Random();
                var players = new []{lobby.Host, lobby.Opponent}
                    .OrderBy(_ => random.Next()).ToList();
                white = players[0]!;
                black = players[1]!;
                break;
        }

        var game = new Game(){
        
        PlayerWhite = white,
        PlayerBlack = black,
        CreatedAt = DateTime.Now,
        
        };
        await _context.Games.AddAsync(game);
        await _context.SaveChangesAsync();
        return game;
    }
}