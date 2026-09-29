using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class SinglePlacementSolver
{
    public Sudoku Execute(Sudoku sudoku)
    {
        var outputSudoku = sudoku;
        outputSudoku = SolveForSinglePlacement(outputSudoku, c => c.Row);
        outputSudoku = SolveForSinglePlacement(outputSudoku, c => c.Column);
        outputSudoku = SolveForSinglePlacement(outputSudoku, c => c.Square);
        if (outputSudoku != sudoku)
            outputSudoku = Execute(outputSudoku);
        return outputSudoku;
    }

    private Sudoku SolveForSinglePlacement(Sudoku sudoku, Func<Cell, int> cellPropertySelector)
    {
        foreach (var i in sudoku.IncludedValues)
        {
            var cellWithUniquePossibleValue = sudoku.Cells
                .Where(c => cellPropertySelector(c) == i && c.CellValue == null)
                .SelectMany(c => c.PossibleValues.Select(v => new
                {
                    Cell = c,
                    PossibleValue = v
                }))
                .GroupBy(x => x.PossibleValue)
                .Where(g => g.Count() == 1)
                .Select(g => g.Single())
                .ToList();
            if (cellWithUniquePossibleValue.Count == 0)
                return sudoku;
            
            sudoku = sudoku with
            {
                Cells = sudoku.Cells
                    .Select(c =>
                        {
                            var matchedCell = cellWithUniquePossibleValue
                                .FirstOrDefault(x => x.Cell == c);
                            return matchedCell != null
                                ? c with { CellValue = matchedCell.PossibleValue }
                                : c;
                        }
                    ).ToList()
            };
        }
        return sudoku;
    }
}