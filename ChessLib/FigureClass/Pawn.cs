namespace ChessLib.FigureClass;

public class Pawn : UniversalFigure
{
    private readonly FigureColor _color;
    private Position [] _finalPositions
    {
        get
        {
            return new Position[]
            {
                    new Position(_color == FigureColor.White ? (Column.A, Row.Eight) : (Column.A, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.B, Row.Eight) : (Column.B, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.C, Row.Eight) : (Column.C, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.D, Row.Eight) : (Column.D, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.E, Row.Eight) : (Column.E, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.F, Row.Eight) : (Column.F, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.G, Row.Eight) : (Column.G, Row.One)),
                    new Position(_color == FigureColor.White ? (Column.H, Row.Eight) : (Column.H, Row.One))
            };
        }
    }
    public string figurePictures => 
        _color == FigureColor.White 
            ? "♙" 
            : "♟";
    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(_color == FigureColor.White ? (Column.A, Row.Two) : (Column.A, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.B, Row.Two) : (Column.B, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.C, Row.Two) : (Column.C, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.D, Row.Two) : (Column.D, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.E, Row.Two) : (Column.E, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.F, Row.Two) : (Column.F, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.G, Row.Two) : (Column.G, Row.Seven)),
            new Position(_color == FigureColor.White ? (Column.H, Row.Two) : (Column.H, Row.Seven))
        };

    public Pawn(FigureColor color)
    {
        _color = color;
    }
    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        var movePositions = 
            _color == FigureColor.White 
                ? CastVector(board, 1, new Vector(0, 1)) 
                : CastVector(board, 1, new Vector(0, -1));
        optionPositions = null;
        return movePositions.Contains(turn.Second);
    }

    public Position[] PawnMoves(Board board , FigureColor color)
    {
        var optionMoves = new List<Position>();
        
        
        return optionMoves.ToArray();
    }
    public Position[] PawnCastVector(Turn turn, Board board, FigureColor color)
    { 
        var optionMoves = new List<Position>();
        var vector = 
            color == FigureColor.White 
                ? new Vector(0, 1)
                : new Vector(0, -1);
        
        var newRow = (int)Position.Row + vector.Y;
        
        var stop = board.Positions.FirstOrDefault(p =>
            p.Position.Column == turn.Second.Column && p.Position.Row == (Row)newRow);
        
        if (DefaultPosition.Contains(Position))
        {
            
        }
        else
        {
            var mv = CastVector(board, 1, vectors);
            optionMoves.AddRange(mv);
        }

        if (cPos > 7 || rPos > 7 || cPos < 0 || rPos < 0 || (stop != null && stop.color == color))
        {
           
        }

        optionMoves.Add(new Position((Column)cPos, (Row)rPos));
        return optionMoves.ToArray();
    }
}
// optionMoves.AddRange(_color == FigureColor.White
// ? new Position[]
// {
    // Position with { Row = (Row)(int)Position.Row + 1 },
    // Position with { Row = (Row)(int)Position.Row + 2 }
// }
// : new Position[]
// {
    // Position with { Row = (Row)(int)Position.Row - 1 },
    // Position with { Row = (Row)(int)Position.Row - 2 }
// });