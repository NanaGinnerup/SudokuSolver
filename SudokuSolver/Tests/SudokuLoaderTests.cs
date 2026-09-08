using AutoFixture;
using SudokuSolver.Contracts;
using Xunit;
using SudokuSolver.SudokuLoader;

namespace SudokuSolver.Tests;

public class SudokuLoaderTests
{
    private readonly Fixture fixture = new Fixture();

    [Fact]
    public void legalSudokuString_ConvertToSudoku()
    {
        // Arrange
        var sudokuPuzzleString =
            "1204" +
            "3012" +
            "2300" +
            "4023";
        var oneLineSudoku = fixture
            .Build<SudokuLoader.SudokuLoader.OneLineSudoku>()
            .With(s => s.Puzzle, sudokuPuzzleString)
            .Create();

        // Act
        var result = OneLineSudokuConverter.ConvertToSudoku(oneLineSudoku);
        
        // Assert
        var test = 0;
    }
}