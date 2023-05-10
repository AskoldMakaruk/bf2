using System.ComponentModel.DataAnnotations;

namespace Chess.Backend.Views;

public class LobbyCreateView
{
   [Required]
   public string Name { get; set; } 
   public string Password { get; set; } 
}