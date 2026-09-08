using AutoFixture;
using FluentAssertions;
using SudokuSolver.SudokuLoader;
using Xunit;

namespace SudokuSolver.SudokuSolver.Tests;

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
        result.IncludedValues.Should().Contain([1,2,3,4]);
    }
}