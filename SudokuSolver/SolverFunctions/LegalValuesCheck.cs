using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class LegalValuesCheck : ISolverFunction
{
    public Sudoku Execute(Sudoku sudoku)
    {
        foreach (var cell in sudoku.Cells)
        {
            if (cell.CellValue != null)
            {
                cell.PossibleValues.Clear();
                continue;
            }

            var valuesExistingInRelevantCells = sudoku.Cells
                .Where(otherCell => 
                    otherCell.CellValue != null
                    && (otherCell.Row == cell.Row || otherCell.Column == cell.Column || otherCell.Square == cell.Square)
                    )
                .Select(c => c.CellValue)
                .ToHashSet();
            
            cell.PossibleValues
                .RemoveWhere(possibleValue => valuesExistingInRelevantCells.Contains(possibleValue));

        }
        return sudoku;
    }
}