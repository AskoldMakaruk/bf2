
using ChessLib.FigureClass;

namespace ChessLib;

public class Board
{
    public List<PosToFigure>  positions;

    public Board()
    {
    }
    
}

public record struct PosToFigure(Position position, UniversalFigure figure);


