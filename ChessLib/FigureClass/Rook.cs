namespace ChessLib.FigureClass;

public class Rook : UniversalFigure
{
    public string figurePictures => Color == FigureColor.White ? "♖" : "♜";

    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(Color == FigureColor.White ? (Column.A, Row.One) : (Column.A, Row.Eight)),
            new Position(Color == FigureColor.White ? (Column.H, Row.One) : (Column.H, Row.Eight)),
        };

    public Rook(FigureColor color, Position position) : base(color, position)
    {
    
    }

    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = CastVector(board,7 , new Vector(1,0), new Vector(0,1), new Vector(-1,0), new Vector(0,-1));
        return optionPositions.Contains(turn.Second);
    }
}