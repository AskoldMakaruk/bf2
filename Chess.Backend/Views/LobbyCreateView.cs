using System.ComponentModel.DataAnnotations;

namespace Chess.Backend.Views;

public class LobbyCreateView
{
   [Required]
   public string Name { get; set; } 
   
   [Required]
   public HostColor Color{ get; set; }
   public string Password { get; set; } 
}

public enum HostColor
{
   White,
   Black,
   Random
}

