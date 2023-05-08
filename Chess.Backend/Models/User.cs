namespace Chess.Backend.Models;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PasswordHash { get; set; } 
    // public string HistoryList { get; set; } = string.Empty;
}