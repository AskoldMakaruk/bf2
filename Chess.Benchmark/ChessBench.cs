using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ChessLib;
using ChessLib.FigureClass;

[SimpleJob(RuntimeMoniker.NativeAot70)]
[RPlotExporter]
public class ChessBench
{
    private byte[] data;

    private Bishop _whiteBishop;
    private Board _board;

    [GlobalSetup]
    public void Setup()
    {
        _whiteBishop = new Bishop(FigureColor.White, new Position(Column.C, Row.One));
        _board = new Board();
        _board.Positions.Add(_whiteBishop);
    }

    [Benchmark]
    public Position[] Old()
    {
        var positions = _whiteBishop.DiagonalOptionMoves(_board);
        return positions;
    }

    [Benchmark]
    public Position[] New()
    {
        var positions =
            _whiteBishop.CastVector(_board, new Vector(1, -1))
                .Concat(_whiteBishop.CastVector(_board, new Vector(1, 1)))
                .Concat(_whiteBishop.CastVector(_board, new Vector(-1, 1)))
                .Concat(_whiteBishop.CastVector(_board, new Vector(-1, -1)))
                .ToArray();
        return positions;
    }

    [Benchmark]
    public Position[] NewOptimized()
    {
        var positions = _whiteBishop.CastVector(_board, new Vector(1, -1), new Vector(1, 1), new Vector(-1, 1), new Vector(-1, -1));
        return positions;
    }
}