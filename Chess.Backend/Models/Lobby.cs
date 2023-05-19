namespace Chess.Backend.Models;

public class Lobby
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public List<User> Users { get; set; }
}