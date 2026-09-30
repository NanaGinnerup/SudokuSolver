using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class SingleLegalValueSolver
{
    public static Sudoku Execute(Sudoku sudoku)
    {
        var outputSudoku = sudoku with
        {
            Cells = sudoku.Cells
                .Select(c => 
                    (c.PossibleValues.Count == 1 && c.CellValue == null)
                    ? c with { CellValue = c.PossibleValues.First(), PossibleValues = []}
                    : c
                )
                .ToList()
        };
        outputSudoku = LegalValuesCheck.UpdatePossibleValues(outputSudoku);
        
        if (!CellsAreSame(outputSudoku.Cells, sudoku.Cells))
            outputSudoku = Execute(outputSudoku);
        return outputSudoku;
    }
    
    private static bool CellsAreSame(
        IEnumerable<Cell> first,
        IEnumerable<Cell> second)
    {
        return first.All(cell =>
        {
            var other = second.Single(c =>
                c.Row == cell.Row &&
                c.Column == cell.Column);

            return cell.CellValue == other.CellValue &&
                   cell.PossibleValues.SetEquals(other.PossibleValues);
        });
    }
}