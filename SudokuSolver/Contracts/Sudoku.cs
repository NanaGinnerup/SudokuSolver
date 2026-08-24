namespace SudokuSolver.Contracts;

public record Sudoku
{
    public required int SudokuSize { get; init; }
    public required List<Cell> Cells { get; set; }
}

public record Cell
{
    public int XValue { get; set; }
    public int YValue { get; set; }
    public int SquareValue { get; set; }
    public int? Value { get; set; }
}