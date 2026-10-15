using AutoFixture;
using FluentAssertions;
using SudokuSolver.SudokuLoader;

namespace SudokuSolver.Tests;

public class SudokuLoaderTests
{
    private readonly Fixture _fixture = new Fixture();

    [Fact]
    public void legalSudokuString_ConvertToSudoku()
    {
        // Arrange
        var sudokuPuzzleString =
            "1204" +
            "3012" +
            "2300" +
            "4023";
        var oneLineSudoku = _fixture
            .Build<SudokuLoader.SudokuLoader.OneLineSudoku>()
            .With(s => s.Puzzle, sudokuPuzzleString)
            .Create();

        // Act
        var result = OneLineSudokuConverter.ConvertToSudoku(oneLineSudoku);
        
        // Assert
        result.SudokuSize.Should().Be(4);
        result.IncludedValues.Should().BeEquivalentTo([1,2,3,4]);
        ExpectedCell[] expectedCells = 
        [
            new(1, 1, 1, 1),
            new(1, 2, 1, 2),
            new(1, 3, 2, null),
            new(1, 4, 2, 4),

            new(2, 1, 1, 3),
            new(2, 2, 1, null),
            new(2, 3, 2, 1),
            new(2, 4, 2, 2),

            new(3, 1, 3, 2),
            new(3, 2, 3, 3),
            new(3, 3, 4, null),
            new(3, 4, 4, null),

            new(4, 1, 3, 4),
            new(4, 2, 3, null),
            new(4, 3, 4, 2),
            new(4, 4, 4, 3)
        ];

        var actualCells = result.Cells.Select(c => new
        {
            c.Row,
            c.Column,
            c.Square,
            c.CellValue
        });
        actualCells.Should().BeEquivalentTo(expectedCells);
        
        var cellsWithCellValueEmpty = result.Cells.Where(c => c.CellValue == null);
        cellsWithCellValueEmpty
            .Should()
            .AllSatisfy(
                c => c.PossibleValues
                    .Should()
                    .BeEquivalentTo([1, 2, 3, 4])
                );
        var cellsWithSpecifiedCellValue = result.Cells.Where(c => c.CellValue != null);
        cellsWithSpecifiedCellValue
            .Should()
            .AllSatisfy(
                c => c.PossibleValues
                    .Should()
                    .BeEquivalentTo([c.CellValue])
                );
    }
    
    [Theory]
    [InlineData(0, "1000000000000000")]
    [InlineData(1, "2000000000000000")]
    [InlineData(2, "3000000000000000")]
    public void csvFile_LoadSudoku_ReturnsRequestedRow(int index, string expectedOneLinePuzzle)
    {
        // Arrange
        var file = WriteCsv(
            "puzzle,solution\r\n" +
            "1000000000000000,1234341221434321\r\n" +
            "2000000000000000,1234341221434321\r\n" +
            "3000000000000000,1234341221434321");

        // Act
        var result = SudokuLoader.SudokuLoader.LoadSudoku(file, index);

        // Assert
        var actualOneLinePuzzle = string.Concat(result.Cells.Select(c => c.CellValue?.ToString() ?? "0"));
        actualOneLinePuzzle.Should().Be(expectedOneLinePuzzle);
        File.Delete(file);
    }

    [Theory]
    [InlineData("puzzle,solution\n1000000000000000,1234341221434321\n2000000000000000,1234341221434321\n")]
    [InlineData("puzzle,solution\n1000000000000000,1234341221434321\n2000000000000000,1234341221434321")]
    public void indexPastEnd_LoadSudoku_ThrowsWithMaxIndex(string content)
    {
        // Arrange
        var file = WriteCsv(content);

        // Act
        var act = () => SudokuLoader.SudokuLoader.LoadSudoku(file, 2);

        // Assert
        act.Should().Throw<IndexOutOfRangeException>()
            .WithMessage(
                $"Requested puzzle 3 does not exist." + 
                $"The file contains 2 puzzles."
                );
        File.Delete(file);
    }

    private static string WriteCsv(string content)
    {
        var file = Path.GetTempFileName();
        File.WriteAllText(file, content);
        return file;
    }

    private record ExpectedCell(
        int Row,
        int Column,
        int Square,
        int? CellValue);
}