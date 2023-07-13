namespace ChessLib.FigureClass;

public class Knight : UniversalFigure
{
    public string figurePictures => Color == FigureColor.White ? "♘" : "♞";
    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(Color == FigureColor.White ? (Column.B, Row.One) : (Column.B, Row.Eight)),
            new Position(Color == FigureColor.White ? (Column.G, Row.One) : (Column.G, Row.Eight))
        };

    public Knight(FigureColor color, Position position) : base(color, position)
    {
    
    }
    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = CastVector(board, 1, new Vector(2, 1), new Vector(2, -1), new Vector(1, 2),
            new Vector(-1, 2), new Vector(-2, 1), new Vector(-2, -1), new Vector(1, -2), new Vector(-1, -2));
        return optionPositions.Contains(turn.Second);
    }
}