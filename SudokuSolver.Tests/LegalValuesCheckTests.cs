using AutoFixture;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;

namespace SudokuSolver.Tests;

public class LegalValuesCheckTests
{
    private readonly Fixture _fixture = new Fixture();
        
    [Theory]
    [InlineData(new[] {1, 2}, new [] {1, 2}, null, null)]
    [InlineData(new[] {1, 2}, null, new [] {1, 2}, null)]
    [InlineData(new[] {1, 2}, null, null, new [] {1, 2})]
    public void valuesExistInSharedEntity_ShouldExcludeValuesFromSharedEntity(
        int[]? values, int[]? rows, int[]? cols, int[]? squares
        )
    {
        // Arrange
        var sut = _fixture.Create<LegalValuesCheck>();
        var cells = new  List<Cell>();
        // var sudoku = _fixture
        //     .Build<Sudoku>()
        //     .With()
        //     ;
        // Act
        
        //Assert
    }


    private Cell CreateTwoByTwoSudokuCell(
        int row, 
        int col, 
        int squareValue, 
        int? cellValue = null
        )
    {
        return new Cell()
        {
            XValue = row,
            YValue = col,
            SquareValue = squareValue,
            CellValue = cellValue,
            PossibleValues = [1, 2, 3, 4]
        };
    }
}