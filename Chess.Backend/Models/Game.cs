using System.ComponentModel.DataAnnotations.Schema;

namespace Chess.Backend.Models;

public class Game
{
    public Guid Id { get; set; }
    public User PlayerWhite { get; set; }
    public User PlayerBlack { get; set; }
    [Column(TypeName = "jsonb")] public List<Turn> Turns { get; set; }
    public bool IsFinished { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public GameResult? Result { get; set; }
}

public readonly record struct Turn(Cell From, Cell To);

public readonly record struct Cell(Row Row, Column Column);

public enum GameResult
{
    WhiteWon,
    BlackWon,
    Draw
}

public enum Row
{
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight
}

public enum Column
{
    A,
    B,
    C,
    D,
    E,
    F,
    G,
    H
}