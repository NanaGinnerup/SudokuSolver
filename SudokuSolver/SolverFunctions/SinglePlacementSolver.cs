using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class SinglePlacementSolver
{
    public Sudoku Execute(Sudoku sudoku)
    {
        foreach (var cellValue in sudoku.IncludedValues)
        {
            sudoku = tempFunc(sudoku);
            break;
            // sudoku = sudoku with
            // {
            //     Cells = sudoku.Cells
            //         .GroupBy(c => c.XValue)
            //         .Select(x => 
            //             x.
            //             )
            // }
        }

        return sudoku;
    }

    private Sudoku tempFunc(Sudoku sudoku)
    {
        foreach (var i in sudoku.IncludedValues)
        {
            var cellWithUniquePossibleValue = sudoku.Cells
                .Where(c => c.XValue == i && c.CellValue == null)
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