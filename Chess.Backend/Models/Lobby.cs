using Chess.Backend.Views;

namespace Chess.Backend.Models;

public class Lobby
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public HostColor Color{ get; set; }
    public User Host { get; set; }
    public User? Opponent { get; set; } 
}