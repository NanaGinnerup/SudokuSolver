using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class LegalValuesCheck
{
    public Sudoku UpdatePossibleValues(Sudoku sudoku)
    {
        var emptySudokuCells = sudoku.Cells.Where(c => c.CellValue == null);
        foreach (var cell in emptySudokuCells)
        {
            cell.PossibleValues
                .RemoveWhere(possibleValue => 
                    sudoku.Cells
                        .Any(otherCell =>
                            (
                                otherCell.Row == cell.Row 
                                || otherCell.Column == cell.Column 
                                || otherCell.Square == cell.Square
                            )
                        && otherCell.CellValue == possibleValue
                        )
                    );
        }
        return sudoku;
    }
}