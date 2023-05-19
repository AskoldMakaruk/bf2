using System.ComponentModel.DataAnnotations;

namespace Chess.Backend.Models;

public class User
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } 
    // public string HistoryList { get; set; } = string.Empty;
}