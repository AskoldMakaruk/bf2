namespace ChessLib;


public abstract class UniversalFigure
{
    public FigureColor color;
    public virtual string  figurePictures { get; }
    public abstract Position [] DefaultPosition { get; }
    public abstract bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions);

    public void Move(Turn turn, Board board)
    {
        var oldFigurePosition = board.positions
            .FirstOrDefault(p =>
                p.position == turn.First
                );
        var newFigurePosition = board.positions
            .FirstOrDefault(p => 
                p.position == turn.Second
                );
        
        if (newFigurePosition == default)
        {
            oldFigurePosition.position = turn.Second;
            return;
        }
        
        newFigurePosition.figure = this;
        board.positions
            .Remove(oldFigurePosition);
    }
}


public record struct Position(Column Column, Row Row)
{
    public Position((Column, Row) posToTuple) : this(posToTuple.Item1, posToTuple.Item2)
    {
        
    }
}; 
public record struct Turn(Position First, Position Second);
