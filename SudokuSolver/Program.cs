// See https://aka.ms/new-console-template for more information

using SudokuSolver;
using SudokuSolver.Contracts;
using SudokuSolver.SolverFunctions;
using SudokuSolver.SudokuLoader;

var originalSudoku = SudokuLoader.LoadSudoku(null, 2);


var sudokuUnsolved = true;
var sudoku = originalSudoku;

while (sudokuUnsolved)
{
    var unalteredSudoku = DeepCopy(sudoku);
    sudoku = LegalValuesCheck.UpdatePossibleValues(sudoku);
    sudoku = SingleLegalValueSolver.Execute(sudoku);
    sudoku = SinglePlacementSolver.Execute(sudoku);

    if (CellsAreSame(sudoku.Cells, unalteredSudoku.Cells))
    {
        Console.WriteLine("Sudoku unsolved");
        break;
    }

    if (sudoku.Cells.Any(c => c.CellValue == null))
    {
        var numberUnsolvedCells = sudoku.Cells.Count(c => c.CellValue == null);
        Console.WriteLine($"{numberUnsolvedCells} unsolved cells");
        continue;
    }
    sudokuUnsolved = false;
    Console.WriteLine("Sudoku successfully solved!");
}

return;


static Sudoku DeepCopy(Sudoku sudoku) => sudoku with
{
    SudokuSize =  sudoku.SudokuSize,
    Cells = sudoku.Cells
        .Select(c => c with { PossibleValues = new HashSet<int>(c.PossibleValues) })
        .ToList(),
    IncludedValues = new HashSet<int>(sudoku.IncludedValues)
};

static bool CellsAreSame(List<Cell> first, List<Cell> second) =>
    first.Count == second.Count &&
    first.Zip(second).All(pair =>
        pair.First.CellValue == pair.Second.CellValue &&
        pair.First.PossibleValues.SetEquals(pair.Second.PossibleValues));
