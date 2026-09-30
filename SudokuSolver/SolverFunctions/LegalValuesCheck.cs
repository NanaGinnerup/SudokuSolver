using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class LegalValuesCheck
{
    public static Sudoku UpdatePossibleValues(Sudoku sudoku)
    {
        // var emptySudokuCells = sudoku.Cells.Where(c => c.CellValue == null);
        // foreach (var cell in emptySudokuCells)
        foreach (var cell in sudoku.Cells)
        {
            cell.PossibleValues
                .RemoveWhere(possibleValue => 
                    sudoku.Cells
                        .Any(otherCell =>
                            (
                                (
                                    otherCell.Row == cell.Row
                                    || otherCell.Column == cell.Column
                                    || otherCell.Square == cell.Square
                                )
                            && otherCell.CellValue == possibleValue
                            ) 
                            || cell.CellValue != null
                        )
                    );
        }
        return sudoku;
    }
}