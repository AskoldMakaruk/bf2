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
        optionPositions = null;
        return true;
    }

    public Position[] VerticalOptionMoves(Turn turn, Board board, int lengthFor = 8)
    {
        var optionMoves = new List<Position>();
        var column = (int)turn.First.Column;
        var row = (int)turn.First.Row;
        var posToFig = board.Positions
            .FirstOrDefault(p =>
                p.Position.Column == turn.First.Column
                && p.Position.Row == turn.First.Row
            );
        /*optionMoves.AddRange(Enumerable.Range(1, lengthFor - 1)
                .Select(i =>
                {
                    var cPos = column + i;
                    var stop = board.positions.FirstOrDefault(p =>
                        p.position.Column == (Column)cPos 
                        && p.position.Row == (Row)row);
                    return  (stop.figure != null && stop.figure.color != posToFig.figure.color) 
                        ? new Position((Column)cPos, (Row)row)
                        : (cPos > 7 || stop.figure != null 
                            && stop.figure.color == posToFig.figure.color) 
                            ? default 
                            : new Position((Column)cPos, (Row)row);
                })
                .TakeWhile(position => position != default)
                .ToList()
        );*/

        for (var i = 1; i < lengthFor; i++)
        {
            var cPos = column + i;
            var rPos = row + i;
            var stop = board.Positions.FirstOrDefault(p =>
                p.Position.Column == (Column)cPos && p.Position.Row == (Row)rPos);
            if (stop != null && stop.color != posToFig.color)
            {
                optionMoves.Add(new Position((Column)cPos, (Row)rPos));
                break;
            }

            if (cPos > 7 || rPos > 7 || stop != null && stop.color == posToFig.color)
            {
                break;
            }

            optionMoves.Add(new Position((Column)cPos, (Row)rPos));
        }

        return null;
    }
}