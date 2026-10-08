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
        { [null, 9], [9, 9], null, null }, // Shared row
        { [null, 9], null, [8, 8], null }, // Shared column
        { [null, 9], null, null, [7, 7] }, // Shared square
        { [8, null, null, null], [9, 1, 1, 9], [9, 1, 2, 3], [9, 1, 2, 3] }, // Shared row, higher amount of cells
    };
    
    [Theory]
    [MemberData(nameof(TestData))]
    public void valuesExistInSharedEntity_ShouldExcludeValuesFromSharedEntity(
        int?[] cellValues, int[]? rows, int[]? cols, int[]? squares
        )
    {
        // Arrange
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
        var result = LegalValuesCheck.UpdatePossibleValues(sudoku);

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
            .AllSatisfy(c => c.PossibleValues.Should().BeEmpty());
    }

    
    // First cell has a CellValue, second cell shares row/column/square with the first cell, third cell shares
    // nothing with the other two cells
    public static TheoryData<int[], int[], int[]> UnrelatedCellTestData => new()
    {
        { [1, 1, 9], [1, 7, 9], [1, 8, 9] }, // Shared row
        { [1, 6, 9], [1, 1, 9], [1, 8, 9] }, // Shared column
        { [1, 6, 9], [1, 7, 9], [1, 1, 9] }, // Shared square
        { [1, 1, 9], [1, 7, 9], [1, 1, 9] }, // Shared row and square
        { [1, 6, 9], [1, 1, 9], [1, 1, 9] }, // Shared column and square
    };

    [Theory]
    [MemberData(nameof(UnrelatedCellTestData))]
    public void valueExcludedInSharedEntity_ShouldNotExcludeValueFromUnrelatedCell(
        int[] rows, int[] cols, int[] squares
        )
    {
        // Arrange
        const int cellValue = 5;
        var cellValues = new int?[] { cellValue, null, null };
        var cells = new List<Cell>();
        for (var i = 0; i < cellValues.Length; i++)
        {
            var cell = CreateNineByNineSudokuCell(rows[i], cols[i], squares[i], cellValues[i]);
            cells.Add(cell);
        }

        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, cells)
            .Create();
        // Act
        var result = LegalValuesCheck.UpdatePossibleValues(sudoku);

        //Assert
        var allPossibleCellValues = Enumerable.Range(1, 9).ToHashSet();

        result.Cells[0].PossibleValues.Should().BeEmpty();
        result.Cells[1].PossibleValues.Should().BeEquivalentTo(allPossibleCellValues.Except([cellValue]));
        result.Cells[2].PossibleValues.Should().BeEquivalentTo(allPossibleCellValues);
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
            Row = row,
            Column = col,
            Square = squareValue,
            CellValue = cellValue,
            PossibleValues = cellValue == null ? Enumerable.Range(1, 9).ToHashSet() : []
        };
    }
}