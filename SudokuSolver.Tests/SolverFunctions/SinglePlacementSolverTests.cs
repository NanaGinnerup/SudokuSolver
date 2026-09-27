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
        
        var cellValue = _fixture.Create<int>();
        var cell = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.PossibleValues, [cellValue])
            .Create();
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, [cell])
            .Create();
        
        // Act
        var result = sut.Execute(sudoku);
        
        // Assert
        result.Cells.Select(c => c.CellValue).Should().AllBeEquivalentTo(cellValue);
    }
    
    [Fact]
    public void MultiplePossibleValues_CellValueStaysNull()
    {
        // Arrange
        var sut = _fixture.Create<SinglePlacementSolver>();
        
        var cell = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.PossibleValues, _fixture.CreateMany<int>().ToHashSet())
            .Create();
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, [cell])
            .Create();
        
        // Act
        var result = sut.Execute(sudoku);
        
        // Assert
        result.Cells.Select(c => c.CellValue)
            .Should().AllBeEquivalentTo((int?) null);
    }
}