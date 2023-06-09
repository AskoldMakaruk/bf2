namespace ChessLib;

public abstract class UniversalFigure
{
    public FigureColor Color;
    
    public virtual string figurePictures { get; }
    public abstract Position[] DefaultPosition { get; }
    public abstract bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions);
    public Position Position { get; set; }
    protected UniversalFigure(FigureColor color, Position position)
    {
        Color = color;
        Position = position;
    }


    public void Move(Turn turn, Board board)
    {
        var newFigurePosition = board.Positions
            .FirstOrDefault(p =>
                p.Position == turn.Second
            );

        // on empty position
        if (newFigurePosition == default)
        {
            this.Position = turn.Second;
            return;
        }

        // take figure
        this.Position = newFigurePosition.Position;
        board.Positions.Remove(newFigurePosition);
    }

    public Position[] CastVector(Board board, int lenghtMove, params Vector[] vectors)
    {
        var optionMoves = new List<Position>();
        for (int v = 0; v < vectors.Length; v++)
        {
            var vector = vectors[v];
            for (var i = 1; i < 1 + lenghtMove; i++)
            {
                var cPos = (int)Position.Column + i * vector.X;
                var rPos = (int)Position.Row + i * vector.Y;
                var stop = board.Positions.FirstOrDefault(p =>
                    p.Position.Column == (Column)cPos && p.Position.Row == (Row)rPos);
                if (stop != null && stop.Color != Color)
                {
                    optionMoves.Add(new Position((Column)cPos, (Row)rPos));
                    break;
                }

                if (cPos > 7 || rPos > 7 || cPos < 0 || rPos < 0 || (stop != null && stop.Color == Color))
                {
                    break;
                }

                optionMoves.Add(new Position((Column)cPos, (Row)rPos));
            }
        }
        return optionMoves.ToArray();
    }
}

public record struct Vector(int X, int Y);

public record struct Position(Column Column, Row Row)
{
    public Position((Column, Row) posToTuple) : this(posToTuple.Item1, posToTuple.Item2)
    {
    }
};

public record struct Turn(Position First, Position Second);