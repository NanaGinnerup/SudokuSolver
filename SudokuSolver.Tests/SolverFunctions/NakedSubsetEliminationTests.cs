using AutoFixture;
using FluentAssertions;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;

namespace SudokuSolver.Tests.SolverFunctions;

public class NakedSubsetEliminationTests
{
    private readonly Fixture _fixture = new Fixture();

    // First two cells form the naked pair, third cell shares the row/column/square with the pair, fourth cell shares
    // nothing with the other three cells
    public static TheoryData<int[], int[], int[]> SharedUnitTestData => new()
    {
        { [1, 1, 1, 9], [1, 2, 9, 8], [1, 2, 3, 9] }, // Shared row
        { [1, 2, 9, 8], [1, 1, 1, 9], [1, 2, 3, 9] }, // Shared column
        { [1, 2, 3, 9], [1, 2, 3, 9], [1, 1, 1, 9] }, // Shared square
    };

    [Theory]
    [MemberData(nameof(SharedUnitTestData))]
    public void NakedPairInSharedUnit_ShouldExcludePairValuesFromOtherCellsInUnit(
        int[] rows, int[] cols, int[] squares
        )
    {
        // Arrange
        HashSet<int>[] possibleValues = [[1, 2], [1, 2], [1, 3], [1, 2, 3]];
        var cells = new List<Cell>();
        for (var i = 0; i < possibleValues.Length; i++)
            cells.Add(CreateCell(possibleValues[i], rows[i], cols[i], squares[i]));
        var sudoku = CreateSudoku(cells);

        // Act
        var result = NakedSubsetElimination.Execute(sudoku);

        // Assert
        result.Cells[0].PossibleValues.Should().BeEquivalentTo([1, 2]);
        result.Cells[1].PossibleValues.Should().BeEquivalentTo([1, 2]);
        result.Cells[2].PossibleValues.Should().BeEquivalentTo([3]);
        result.Cells[3].PossibleValues.Should().BeEquivalentTo([1, 2, 3]);
    }
    

    [Fact]
    public void TwoCellsWithDifferentPairs_ShouldNotExcludeAnyValues()
    {
        // Arrange
        var cells = new List<Cell>
        {
            CreateCell([1, 2], row: 1, column: 1, square: 1),
            CreateCell([1, 3], row: 1, column: 2, square: 1),
            CreateCell([1, 2, 3], row: 1, column: 3, square: 1),
        };
        var sudoku = CreateSudoku(cells);

        // Act
        var result = NakedSubsetElimination.Execute(sudoku);

        // Assert
        result.Should().BeSameAs(sudoku);
        result.Cells[2].PossibleValues.Should().BeEquivalentTo([1, 2, 3]);
    }
    

    [Fact]
    public void EliminationCreatesNewNakedPair_ShouldExcludeValuesOfNewPairAsWell()
    {
        // Arrange
        // The {1, 2} pair turns the third and fourth cell into a {3, 4} pair, which then leaves only 5 in the last cell
        var cells = new List<Cell>
        {
            CreateCell([1, 2], row: 1, column: 1, square: 1),
            CreateCell([1, 2], row: 1, column: 2, square: 1),
            CreateCell([1, 3, 4], row: 1, column: 3, square: 1),
            CreateCell([2, 3, 4], row: 1, column: 4, square: 2),
            CreateCell([3, 4, 5], row: 1, column: 5, square: 2),
        };
        var sudoku = CreateSudoku(cells);

        // Act
        var result = NakedSubsetElimination.Execute(sudoku);

        // Assert
        result.Cells[2].PossibleValues.Should().BeEquivalentTo([3, 4]);
        result.Cells[3].PossibleValues.Should().BeEquivalentTo([3, 4]);
        result.Cells[4].PossibleValues.Should().BeEquivalentTo([5]);
    }
    

    [Fact]
    public void NoNakedPairs_ShouldReturnSameSudoku()
    {
        // Arrange
        var cells = new List<Cell>
        {
            CreateCell([1, 2], row: 1, column: 1, square: 1),
            CreateCell([1, 2, 3], row: 1, column: 2, square: 1),
            CreateCell([1, 2, 3], row: 1, column: 3, square: 1),
        };
        var sudoku = CreateSudoku(cells);

        // Act
        var result = NakedSubsetElimination.Execute(sudoku);

        // Assert
        result.Should().BeSameAs(sudoku);
    }
    

    [Fact]
    public void NakedTripleInUnit_ShouldExcludeTripleValuesFromOtherCellsInUnit()
    {
        // Arrange
        var cells = new List<Cell>
        {
            CreateCell([1, 2, 3], row: 1, column: 1, square: 1),
            CreateCell([1, 2, 3], row: 1, column: 2, square: 1),
            CreateCell([1, 2, 3], row: 1, column: 3, square: 1),
            CreateCell([1, 2, 3, 4], row: 1, column: 4, square: 1),
            CreateCell([1, 2, 3, 4], row: 9, column: 9, square: 9),
        };
        var sudoku = CreateSudoku(cells);

        // Act
        var result = NakedSubsetElimination.Execute(sudoku);

        // Assert
        result.Cells[0].PossibleValues.Should().BeEquivalentTo([1, 2, 3]);
        result.Cells[1].PossibleValues.Should().BeEquivalentTo([1, 2, 3]);
        result.Cells[2].PossibleValues.Should().BeEquivalentTo([1, 2, 3]);
        result.Cells[3].PossibleValues.Should().BeEquivalentTo([4]);
        result.Cells[4].PossibleValues.Should().BeEquivalentTo([1,2,3,4]);
    }

    
    private Sudoku CreateSudoku(List<Cell> cells)
    {
        return _fixture
            .Build<Sudoku>()
            .With(s => s.Cells, cells)
            .Create();
    }

    private static Cell CreateCell(HashSet<int> possibleValues, int row, int column, int square)
    {
        return new Cell()
        {
            Row = row,
            Column = column,
            Square = square,
            CellValue = null,
            PossibleValues = possibleValues
        };
    }
}
