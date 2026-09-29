namespace SudokuSolver.Contracts;

public record Sudoku
{
    public required int SudokuSize { get; init; }
    public required List<Cell> Cells { get; set; }
    public required HashSet<int> IncludedValues  { get; set; }
}

public record Cell
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int Square { get; set; }
    public int? CellValue { get; set; }
    public HashSet<int> PossibleValues { get; set; } 
}