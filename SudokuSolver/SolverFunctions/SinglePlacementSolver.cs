using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class SinglePlacementSolver
{
    public Sudoku Execute(Sudoku sudoku)
    {
        sudoku = sudoku with
        {
            Cells = sudoku.Cells
                .Select(c => 
                    (c.PossibleValues.Count == 1 && c.CellValue == null)
                    ? c with { CellValue = c.PossibleValues.First() }
                    : c
                )
                .ToList()
        };
        return sudoku;
    }
}