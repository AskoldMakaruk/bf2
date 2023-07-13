using ChessLib.FigureClass;

namespace ChessLib;

public class Board
{
    public List<UniversalFigure> Positions = new();
    public List<Turn> Turns = new();

    public Board()
    {
        Positions = new List<UniversalFigure>();
        var figureTypes = new List<Type>
        {
            typeof(Pawn),
            typeof(Rook),
            typeof(Knight),
            typeof(Bishop),
            typeof(Queen),
            typeof(King)
        };
        
    var whiteAndBlack = figureTypes.Select(a=>
        Activator.CreateInstance(a, FigureColor.White, new Position(Column.A, Row.One)))
        .Concat(figureTypes.Select(a=>
        Activator.CreateInstance(a, FigureColor.Black, new Position(Column.A, Row.One)))).Cast<UniversalFigure>();
        foreach (var figure in whiteAndBlack)
        {
        
            Positions.AddRange(figure.DefaultPosition.Select(a=>Activator.CreateInstance(figure.GetType(), figure.Color, a) as UniversalFigure));
        }
    }
}
