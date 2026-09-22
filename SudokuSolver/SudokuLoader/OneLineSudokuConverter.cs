using SudokuSolver.Contracts;

namespace SudokuSolver.SudokuLoader;

public static class OneLineSudokuConverter
{
    public static Sudoku ConvertToSudoku(
        SudokuLoader.OneLineSudoku oneLineSudoku
        )
    {
        var n = GetSudokuSize(oneLineSudoku);
        var legalValues = Enumerable.Range(1, n).ToHashSet();
        var squareN = (int) Math.Sqrt(n);
        
        var cellList = new List<Cell>();
        var cellIndex = 0;
        foreach (var row in Enumerable.Range(1, n))
        {
            foreach (var col in Enumerable.Range(1, n))
            {
                var cellValue = oneLineSudoku.Puzzle[cellIndex] != '0' 
                    ? (int) char.GetNumericValue(oneLineSudoku.Puzzle[cellIndex])
                    : (int?) null;
                var rowIntegerDivision = (row - 1) / squareN;
                var colIntegerDivision = (col - 1) / squareN;
                var cell = new Cell
                {
                    XValue = col,
                    YValue = row,
                    SquareValue = 1 + squareN * rowIntegerDivision + colIntegerDivision,
                    CellValue = cellValue,
                    PossibleValues = cellValue != null 
                        ? [(int) cellValue] 
                        : legalValues,
                };
                cellList.Add(cell);
                cellIndex++;
            }
        }
        var sudoku = new Sudoku 
        {
            SudokuSize = n, Cells = cellList, IncludedValues = legalValues
        };
        return sudoku;
    }
    
    private static int GetSudokuSize(SudokuLoader.OneLineSudoku oneLineSudoku)
    {
        var cellCounts = oneLineSudoku.Puzzle.Length;
        var n = (int) Math.Sqrt(cellCounts);
        if (n * n != cellCounts)
        {
            throw new ArgumentException(
                $"Expected an n x n sudoku, but the sudoku is not a perfect square as it has {cellCounts} cells.");
        }
        return n;
    }
}