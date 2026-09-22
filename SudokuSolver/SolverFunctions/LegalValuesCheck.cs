using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class LegalValuesCheck
{
    public Sudoku UpdatePossibleValues(Sudoku sudoku)
    {
        var rows = sudoku.Cells
            .GroupBy(c => c.XValue)
            .Select(g => new 
                {
                    XValue = g.Key, 
                    CellValues = g
                        .Where(c => c.CellValue.HasValue)
                        .Select(c => c.CellValue!.Value)
                        .ToHashSet()
                }
            );
        var columns = sudoku.Cells
            .GroupBy(c => c.YValue)
            .Select(g => new
                {
                    YValue = g.Key,
                    CellValues = g
                        .Where(c => c.CellValue.HasValue)
                        .Select(c => c.CellValue!.Value)
                        .ToHashSet()
                }
            );
        var squares = sudoku.Cells
            .GroupBy(c => c.SquareValue)
            .Select(g => new
                {
                    SquareValue = g.Key,
                    CellValues = g
                        .Where(c => c.CellValue.HasValue)
                        .Select(c => c.CellValue!.Value)
                        .ToHashSet()
                }
            );
        
        
        return sudoku;
    }
}