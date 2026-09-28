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
        var sut = _fixture.Create<SinglePlacementSolver>();
        
        var cell = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.XValue, 1)
            .With(c => c.PossibleValues, [2,4])
            .Create();
        var cell2 = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.XValue, 1)
            .With(c => c.PossibleValues, [1,3,4])
            .Create();
        var cell3 = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.XValue, 1)
            .With(c => c.PossibleValues, [1,2,4])
            .Create();
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.IncludedValues, [1,2,3,4])
            .With(s => s.Cells, [cell, cell2, cell3])
            .Create();
        
        // Act
        var result = sut.Execute(sudoku);
        
        // Assert
        result.Cells.Count(c => c.CellValue == null).Should().Be(2);
        result.Cells.Count(c => c.CellValue == 3).Should().Be(1);
    }
    
}