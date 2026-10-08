using AutoFixture;
using FluentAssertions;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;

namespace SudokuSolver.Tests.SolverFunctions;

public class SinglePlacementSolverTests
{
    private readonly Fixture _fixture = new Fixture();
    
    [Fact]
    public void OnlyOnePossibleValue_CellValueUpdatedToTheValue()
    {
        // Arrange
        var cell1 = CreateCell([1, 3, 4], row: 1);
        var cell2 = CreateCell([1, 2, 4], row: 1);
        var cell3 = CreateCell([1, 2, 4], row: 1);
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.IncludedValues, [1,2,3,4])
            .With(s => s.Cells, [cell1, cell2, cell3])
            .Create();
        
        // Act
        var result = SinglePlacementSolver.Execute(sudoku);
        
        // Assert
        result.Cells.Count(c => c.CellValue == null).Should().Be(2);
        result.Cells.Count(c => c.CellValue == 3).Should().Be(1);
    }    
    
    
    [Fact]
    public void LayeredOnlyOnePossibleValue_BothCellValueUpdatedToTheValue()
    {
        // Arrange
        var cell1 = CreateCell([1, 2, 4], row: 1);
        var cell2 = CreateCell([2, 3, 4], row: 1);
        var cell3 = CreateCell([3, 4], row: 1);
        var cell4 = CreateCell([3, 4], row: 1);
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.IncludedValues, [1,2,3,4])
            .With(s => s.Cells, [cell1, cell2, cell3, cell4])
            .Create();
        
        // Act
        var result = SinglePlacementSolver.Execute(sudoku);
        
        // Assert
        result.Cells.Count(c => c.CellValue == null).Should().Be(2);
        result.Cells.Count(c => c.CellValue == 1).Should().Be(1);
        result.Cells.Count(c => c.CellValue == 2).Should().Be(1);
    }

    private Cell CreateCell(
        HashSet<int> possibleValues, 
        int? cellValue = null,
        int? row = null, 
        int? column = null, 
        int? square = null
        )
    // Leaving cellValue is null returns a cell with unspecified cellValue, while leaving row, column or square as null
    // leaves Fixture to create a value since they are non-nullable properties
    {
        var cell = _fixture
            .Build<Cell>()
            .With(c => c.PossibleValues, possibleValues)
            .With(c => c.CellValue, cellValue);
        if (row != null)
            cell = cell.With(c => c.Row, row);
        if (column != null)
            cell = cell.With(c => c.Column, column);
        if (square != null)
            cell = cell.With(c => c.Square, square);
        return cell.Create();
    }
    
}