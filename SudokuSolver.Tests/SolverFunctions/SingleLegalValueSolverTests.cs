using AutoFixture;
using FluentAssertions;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;

namespace SudokuSolver.Tests.SolverFunctions;

public class SingleLegalValueSolverTests
{
    private readonly Fixture _fixture = new Fixture();
    
    [Fact]
    public void OnlyOnePossibleValue_CellValueUpdatedToTheValue()
    {
        // Arrange
        var sut = _fixture.Create<SingleLegalValueSolver>();
        
        var cellValue = _fixture.Create<int>();
        var cells = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.PossibleValues, [cellValue])
            .CreateMany();
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, cells.ToList())
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
        var sut = _fixture.Create<SingleLegalValueSolver>();
        
        var cells = _fixture
            .Build<Cell>()
            .With(c => c.CellValue, (int?) null)
            .With(c => c.PossibleValues, _fixture.CreateMany<int>().ToHashSet())
            .CreateMany();
        var sudoku = _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, cells.ToList())
            .Create();
        
        // Act
        var result = sut.Execute(sudoku);
        
        // Assert
        result.Cells.Select(c => c.CellValue)
            .Should().AllBeEquivalentTo((int?) null);
    }
}