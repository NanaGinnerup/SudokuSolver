using AutoFixture;
using FluentAssertions;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;

namespace SudokuSolver.Tests.SolverFunctions;

public class LegalValuesCheckTests
{
    private readonly Fixture _fixture = new Fixture();

    public static TheoryData<int?[], int[]?, int[]?, int[]?> TestData => new()
    {
        { [null, 9], [9, 9], null, null },
        { [null, 9], null, [8, 8], null },
        { [null, 9], null, null, [7, 7] },
        { [8, null, null, null], [9, 1, 1, 9], [9, 1, 2, 3], [9, 1, 2, 3] },
    };
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void valuesExistInSharedEntity_ShouldExcludeValuesFromSharedEntity(
        int?[] cellValues, int[]? rows, int[]? cols, int[]? squares
        )
    {
        // Arrange
        var sut = _fixture.Create<LegalValuesCheck>();
        var cells = new List<Cell>();
        var numberCells = cellValues.Length;
        for (var i = 0; i < numberCells; i++)
        {
            // If no specified value in TheoryData, cell row/column/square gets enumerated starting at 1 to keep the
            // cells in different row/columns/squares
            var cell = CreateNineByNineSudokuCell(
                rows != null ? rows[i] : i + 1, 
                cols != null ? cols[i] : i + 1, 
                squares != null ? squares[i] : i + 1, 
                cellValues[i]
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
        var allPossibleCellValues = Enumerable.Range(1, 9).ToHashSet();
        var cellValuesLeft = allPossibleCellValues.Except(cellValues.OfType<int>());
            
        result.Cells
            .Where(c => c.CellValue == null)
            .Should()
            .AllSatisfy(c => 
                c.PossibleValues.Should().Contain(cellValuesLeft)
                );
        
        result.Cells
            .Where(c => c.CellValue != null)
            .Should()
            .AllSatisfy(c => c.PossibleValues.Should().Contain(allPossibleCellValues));
    }


    private Cell CreateNineByNineSudokuCell(
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
            PossibleValues = Enumerable.Range(1, 9).ToHashSet()
        };
    }
}