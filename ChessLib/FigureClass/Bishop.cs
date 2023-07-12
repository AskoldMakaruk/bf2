namespace ChessLib.FigureClass;

public class Bishop : UniversalFigure
{
    private readonly FigureColor _color;
    public  string figurePictures => _color == FigureColor.White ? "♗" : "♝";

    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(_color == FigureColor.White ? (Column.C, Row.One) : (Column.C, Row.Eight)),
            new Position(_color == FigureColor.White ? (Column.F, Row.One) : (Column.F, Row.Eight)),
        };

    public Bishop(FigureColor color)
    {
        _color = color;
    }
   
    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = DiagonalOptionMoves(turn, board);
        return optionPositions.Contains(turn.Second);
    }
    public Position[] DiagonalOptionMoves(Turn turn ,Board board, int lengthFor = 8 )
    {
        var optionMoves = new List<Position>();
        var column = (int)turn.First.Column;
        var row = (int)turn.First.Row;
        var posToFig = board.positions
            .FirstOrDefault(p=> 
                p.position.Column == turn.First.Column 
                && p.position.Row == turn.First.Row
            );

        for (var i = 1; i < lengthFor; i++)
        {
            var cPos = column + i;
            var rPos = row + i;
            var stop = board.positions.FirstOrDefault(p =>
                p.position.Column == (Column)cPos && p.position.Row == (Row)rPos);
            if (stop.figure != null && stop.figure.color != posToFig.figure.color)
            {
                optionMoves.Add(new Position((Column)cPos,(Row)rPos));
                break;
            }
            if (cPos > 7 || rPos > 7 || stop.figure != null && stop.figure.color == posToFig.figure.color)
            {
                break;
            }
            optionMoves.Add(new Position((Column)cPos,(Row)rPos));
        }
        for (var i = 1; i < lengthFor; i++)
        {
            var cPos = column + i;
            var rNeg = row - i;
            var stop = board.positions.FirstOrDefault(p =>
                p.position.Column == (Column)cPos && p.position.Row == (Row)rNeg);
            if (stop.figure != null && stop.figure.color != posToFig.figure.color)
            {
                optionMoves.Add(new Position((Column)cPos,(Row)rNeg));
                break;
            }
            if (cPos > 7 || rNeg < 0 || stop.figure != null && stop.figure.color == posToFig.figure.color)
            {
                break;
            }
            optionMoves.Add(new Position((Column)cPos,(Row)rNeg));
        }
        for (var i = 1; i < lengthFor; i++)
        {
            var cNeg = column - i;
            var rNeg = row - i;
            var stop = board.positions.FirstOrDefault(p =>
                p.position.Column == (Column)cNeg && p.position.Row == (Row)rNeg);
            if (stop.figure != null && stop.figure.color != posToFig.figure.color)
            {
                optionMoves.Add(new Position((Column)cNeg,(Row)rNeg));
                break;
            }
            if (cNeg < 0 || rNeg < 0 || stop.figure != null && stop.figure.color == posToFig.figure.color)
            {
                break;
            }
            optionMoves.Add(new Position((Column)cNeg,(Row)rNeg));
        }
        for (var i = 1; i < lengthFor; i++)
        {
            var cNeg = column - i;
            var rPos = row + i;
            var stop = board.positions.FirstOrDefault(p =>
                p.position.Column == (Column)cNeg && p.position.Row == (Row)rPos);
            if (stop.figure != null && stop.figure.color != posToFig.figure.color)
            {
                optionMoves.Add(new Position((Column)cNeg,(Row)rPos));
                break;
            }
            if (cNeg < 0 || rPos < 0 || stop.figure != null && stop.figure.color == posToFig.figure.color)
            {
                break;
            }
            optionMoves.Add(new Position((Column)cNeg,(Row)rPos));
        }
        return optionMoves.ToArray();
    }
}
