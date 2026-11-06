using System.Text;
using SudokuSolver.Contracts;

namespace SudokuSolver.Visualisations;

public static class PrintToTerminal
{
    public static void Print(Sudoku sudoku)
    {
        var sudokuSize = sudoku.SudokuSize;
        var sudokuSizeRoot = (int) Math.Sqrt(sudokuSize);
        var cells = sudoku.Cells
            .ToDictionary(cell => (cell.Row, cell.Column));
        
        var stringBuilder = new StringBuilder();
        var stringSeperator = new string('-', sudokuSize * 2 + sudokuSizeRoot + 1);

        for (var row = 1; row <= sudokuSize; row++)
        {
            if ((row - 1) % sudokuSizeRoot == 0)
                stringBuilder.AppendLine(stringSeperator);
            for (var col = 1; col <= sudokuSize; col++)
            {
                if ((col - 1) % sudokuSizeRoot == 0)
                    stringBuilder.Append('|');
                cells.TryGetValue((row, col), out var cell);
                var cellValue = CellValueForPrint(cell);
                stringBuilder.Append(cellValue.PadLeft(1));
            }
            stringBuilder.AppendLine("|");
        }
        stringBuilder.AppendLine(stringSeperator);

        Console.Write(stringBuilder.ToString());
        
    }
    
    private static string CellValueForPrint(Cell? cell)
    {
        var cellValue = cell?.CellValue?.ToString() ?? ".";
        cellValue = cellValue.PadLeft(2).PadRight(2);

        return cell?.CellValue is null ? ColorRed(cellValue) : cellValue;
    }
    
    private static string ColorRed(string cellValue)
        => $"\e\u005b\u0033\u0031\u006d{cellValue}\e[0m";
}