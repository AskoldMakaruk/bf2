using ChessLib.FigureClass;

namespace ChessLib;

public class Board
{
    public List<UniversalFigure> Positions = new();
    public List<Turn> Turns = new();

    public Board()
    {
    }
}
