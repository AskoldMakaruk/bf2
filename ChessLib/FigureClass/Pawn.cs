namespace ChessLib.FigureClass;

public class Pawn : UniversalFigure
{
    private Position [] _finalPositions
    {
        get
        {
            var row = Color == FigureColor.White ? Row.Eight : Row.One;
            return Enum.GetValues<Column>().Select(a => new Position(a, row)).ToArray();
        }
    }
    public string figurePictures => 
        Color == FigureColor.White 
            ? "♙" 
            : "♟";
    public override Position[] DefaultPosition 
    {
        get
        {
            var row = Color == FigureColor.White ? Row.Two : Row.Seven;
            return Enum.GetValues<Column>().Select(a => new Position(a, row)).ToArray();
        }
    }

    public Pawn(FigureColor color, Position position) : base(color, position)
    {
    
    }
    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        
        optionPositions = PawnCastVector(board);
        return optionPositions.Contains(turn.Second);
    }
    
    private Position[] PawnCastVector(Board board)
    { 
        var optionMoves = new List<Position>();
        var vector = 
            Color == FigureColor.White 
                ? new Vector(0, 1)
                : new Vector(0, -1);
        
        var newRow = (int)Position.Row + vector.Y;
        
        var stop = board.Positions.FirstOrDefault(p =>
            p.Position.Column == Position.Column && p.Position.Row == (Row)newRow);
        var possibleAttackPos =
            board.Positions.FirstOrDefault(p => p.Position.Column == (Column)(int)Position.Column + 1 && p.Position.Row == (Row)newRow);
        var possibleAttackNeg =
            board.Positions.FirstOrDefault(p => p.Position.Column == (Column)(int)Position.Column - 1 && p.Position.Row == (Row)newRow);
        
        if (DefaultPosition.Contains(Position))
        {
            optionMoves.AddRange(new Position[]
            {
                Position with { Row = (Row)newRow },
                Position with { Row = (Row)newRow + vector.Y }
            });
        }
        
        if (possibleAttackPos != null && possibleAttackPos.Color != Color)
        {
            optionMoves.Add(possibleAttackPos.Position);
        }

        if (possibleAttackNeg != null && possibleAttackNeg.Color != Color)
        {
            optionMoves.Add(possibleAttackNeg.Position);
        }
        
        if (newRow is > 7 or < 0 || stop != null)
        {
            return optionMoves.ToArray();
        }
        optionMoves.Add(Position with { Row = (Row)newRow});
        
        return optionMoves.ToArray();
    }
}
