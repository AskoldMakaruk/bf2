namespace ChessLib.FigureClass;

public class Bishop : UniversalFigure
{
    public string figurePictures => Color == FigureColor.White ? "♗" : "♝";

    public override Position[] DefaultPosition =>
        new Position[]
        {
            new Position(Color == FigureColor.White ? (Column.C, Row.One) : (Column.C, Row.Eight)),
            new Position(Color == FigureColor.White ? (Column.F, Row.One) : (Column.F, Row.Eight)),
        };

    public Bishop(FigureColor color, Position position) : base(color, position)
    {
    }

    public override bool CanFigureMove(Turn turn, Board board, out Position[] optionPositions)
    {
        optionPositions = CastVector(board,7 , new Vector(1, 1), new Vector(1,-1), new Vector(-1,1), new Vector(-1,-1));
        return optionPositions.Contains(turn.Second);
    }
}
/*     public Position[] DiagonalOptionMoves(Board board, int lengthFor = 8)
    {
        var optionMoves = new List<Position>();
        var column = (int)Position.Column;
        var row = (int)Position.Row;

        for (var i = 1; i < lengthFor; i++)
        {
            var cPos = column + i;
            var rPos = row + i;
            var stop = board.Positions.FirstOrDefault(p =>
                p.Position.Column == (Column)cPos && p.Position.Row == (Row)rPos);
            if (stop != null && stop.color != color)
            {
                optionMoves.Add(new Position((Column)cPos, (Row)rPos));
                break;
            }

            if (cPos > 7 || rPos > 7 || stop != null && stop.color == color)
            {
                break;
            }

            optionMoves.Add(new Position((Column)cPos, (Row)rPos));
        }

        for (var i = 1; i < lengthFor; i++)
        {
            var cPos = column + i;
            var rNeg = row - i;
            var stop = board.Positions.FirstOrDefault(p =>
                p.Position.Column == (Column)cPos && p.Position.Row == (Row)rNeg);
            if (stop != null && stop.color != color)
            {
                optionMoves.Add(new Position((Column)cPos, (Row)rNeg));
                break;
            }

            if (cPos > 7 || rNeg < 0 || stop != null && stop.color == color)
            {
                break;
            }

            optionMoves.Add(new Position((Column)cPos, (Row)rNeg));
        }

        for (var i = 1; i < lengthFor; i++)
        {
            var cNeg = column - i;
            var rNeg = row - i;
            var stop = board.Positions.FirstOrDefault(p =>
                p.Position.Column == (Column)cNeg && p.Position.Row == (Row)rNeg);
            if (stop != null && stop.color != color)
            {
                optionMoves.Add(new Position((Column)cNeg, (Row)rNeg));
                break;
            }

            if (cNeg < 0 || rNeg < 0 || stop != null && stop.color == color)
            {
                break;
            }

            optionMoves.Add(new Position((Column)cNeg, (Row)rNeg));
        }

        for (var i = 1; i < lengthFor; i++)
        {
            var cNeg = column - i;
            var rPos = row + i;
            var stop = board.Positions.FirstOrDefault(p =>
                p.Position.Column == (Column)cNeg && p.Position.Row == (Row)rPos);
            if (stop != null && stop.color != color)
            {
                optionMoves.Add(new Position((Column)cNeg, (Row)rPos));
                break;
            }

            if (cNeg < 0 || rPos < 0 || stop != null && stop.color == color)
            {
                break;
            }

            optionMoves.Add(new Position((Column)cNeg, (Row)rPos));
        }

        return optionMoves.ToArray();
    }
*/