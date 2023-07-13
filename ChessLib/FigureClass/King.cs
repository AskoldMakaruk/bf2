namespace ChessLib.FigureClass;

public class King : UniversalFigure
{
    public string figurePictures => Color == FigureColor.White ? "♔" : "♚";
    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(Color == FigureColor.White ? (Column.D, Row.One) : (Column.D, Row.Eight)),
        };

    public King(FigureColor color, Position position) : base(color, position)
    {
    
    }
    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = CastVector(board, 1, new Vector(1, 0), new Vector(0, 1), new Vector(-1, 0),
            new Vector(0, -1), new Vector(1, 1), new Vector(1, -1), new Vector(-1, 1), new Vector(-1, -1));
        return optionPositions.Contains(turn.Second);
    }
}