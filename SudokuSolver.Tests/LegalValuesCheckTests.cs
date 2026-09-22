using AutoFixture;
using FluentAssertions;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;

namespace SudokuSolver.Tests;

public class LegalValuesCheckTests
{
    private readonly Fixture _fixture = new Fixture();

    public static TheoryData<int?[], int[]?, int[]?, int[]?> TestData => new()
    {
        { [null, 2], [4, 4], null, null },
        { [null, 2], null, [4, 4], null },
        { [null, 2], null, null, [4, 4] },
    };
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void valuesExistInSharedEntity_ShouldExcludeValuesFromSharedEntity(
        int?[] values, int[]? rows, int[]? cols, int[]? squares
        )
    {
        // Arrange
        var sut = _fixture.Create<LegalValuesCheck>();
        var cells = new List<Cell>();
        for (var i = 0; i < 2; i++)
        {
            var cell = CreateTwoByTwoSudokuCell(
                rows != null ? rows[i] : i + 1, 
                cols != null ? cols[i] : i + 1, 
                squares != null ? squares[i] : i + 1, 
                values[i]
                );
            cells.Add(cell);
        }

        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, cells)
            .Create();
        // Act
        var result = sut.UpdatePossibleValues(sudoku);

        //Assert
        result.Cells
            .Where(c => c.CellValue == null)
            .Should()
            .AllSatisfy(c => c.PossibleValues.Should().NotContain(4));
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