namespace ChessLib.FigureClass;

public class Rook : UniversalFigure
{
    private readonly FigureColor _color;
    public string figurePictures => _color == FigureColor.White ? "♖" : "♜";

    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(_color == FigureColor.White ? (Column.A, Row.One) : (Column.A, Row.Eight)),
            new Position(_color == FigureColor.White ? (Column.H, Row.One) : (Column.H, Row.Eight)),
        };

    public Rook(FigureColor color)
    {
        _color = color;
    }

    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = CastVector(board,7 , new Vector(1,0), new Vector(0,1), new Vector(-1,0), new Vector(0,-1));
        return optionPositions.Contains(turn.Second);
    }
}