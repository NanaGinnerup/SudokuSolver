using AutoFixture;
using Xunit;

namespace SudokuSolver.Tests;

public class SudokuLoaderTests
{
    private readonly Fixture fixture = new Fixture();

    [Fact]
    public void legalSudokuString_ConvertToSudoku()
    {
        // Arrange
        var sut = fixture.Create<SudokuLoader>();

        var sudokuPuzzleString =
            "1204" +
            "3012" +
            "2300" +
            "4023";
        var oneLineSudoku = new SudokuLoader.OneLineSudoku
        {
            Puzzle = sudokuPuzzleString,
            Solution = sudokuPuzzleString
        };
        
        // Act
        var result = sut.
    
}