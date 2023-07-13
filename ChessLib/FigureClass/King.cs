namespace ChessLib.FigureClass;

public class King : UniversalFigure
{
    private readonly FigureColor _color;
    public string figurePictures => _color == FigureColor.White ? "♔" : "♚";
    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(_color == FigureColor.White ? (Column.D, Row.One) : (Column.D, Row.Eight)),
        };

    public King(FigureColor color)
    {
        _color = color;
    }
    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = CastVector(board, 1, new Vector(1, 0), new Vector(0, 1), new Vector(-1, 0),
            new Vector(0, -1), new Vector(1, 1), new Vector(1, -1), new Vector(-1, 1), new Vector(-1, -1));
        return optionPositions.Contains(turn.Second);
    }
}