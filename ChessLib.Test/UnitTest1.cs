using ChessLib.FigureClass;
using FluentAssertions;

namespace ChessLib.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        var whiteBishop = new Bishop(FigureColor.White, new Position(Column.C, Row.One));

        var board = new Board();
        board.Positions.Add(whiteBishop);

        var positions = whiteBishop.DiagonalOptionMoves(board).OrderBy(a => a.Column).ToArray();
        // assert positions
        var expectedPositions = new Position[]
        {
            new(Column.A, Row.Three),
            new(Column.B, Row.Two),
            new(Column.D, Row.Two),
            new(Column.E, Row.Three),
            new(Column.F, Row.Four),
            new(Column.G, Row.Five),
            new(Column.H, Row.Six),
        };

        positions.Should().BeEquivalentTo(expectedPositions);
    }

    [Test]
    public void Test2()
    {
        var whiteBishop = new Bishop(FigureColor.White, new Position(Column.C, Row.One));

        var board = new Board();
        board.Positions.Add(whiteBishop);

        var positions =
            whiteBishop.CastVector(board, new Vector(1, -1))
                .Concat(whiteBishop.CastVector(board, new Vector(1, 1)))
                .Concat(whiteBishop.CastVector(board, new Vector(-1, 1)))
                .Concat(whiteBishop.CastVector(board, new Vector(-1, -1)))
                .OrderBy(a => a.Column)
                .ToArray();
        // assert positions
        var expectedPositions = new Position[]
        {
            new(Column.A, Row.Three),
            new(Column.B, Row.Two),
            new(Column.D, Row.Two),
            new(Column.E, Row.Three),
            new(Column.F, Row.Four),
            new(Column.G, Row.Five),
            new(Column.H, Row.Six),
        };

        positions.Should().BeEquivalentTo(expectedPositions);
    }
}