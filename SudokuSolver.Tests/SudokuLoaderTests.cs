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
            new(2, 1, 1, 2),
            new(3, 1, 2, null),
            new(4, 1, 2, 4),

            new(1, 2, 1, 3),
            new(2, 2, 1, null),
            new(3, 2, 2, 1),
            new(4, 2, 2, 2),

            new(1, 3, 3, 2),
            new(2, 3, 3, 3),
            new(3, 3, 4, null),
            new(4, 3, 4, null),

            new(1, 4, 3, 4),
            new(2, 4, 3, null),
            new(3, 4, 4, 2),
            new(4, 4, 4, 3)
        ];

        var actualCells = result.Cells.Select(c => new
        {
            XValue = c.XValue,
            YValue = c.YValue,
            SquareValue = c.SquareValue,
            CellValue = c.CellValue
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
    
    private record ExpectedCell(
        int XValue,
        int YValue,
        int SquareValue,
        int? CellValue);
}