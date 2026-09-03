using System.Globalization;
using SudokuSolver.Contracts;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace SudokuSolver;

public class SudokuLoader
{
    public SudokuLoader(string? fileLocation, int index = 0, bool header = true)
    {
        fileLocation ??= "C:\\Users\\nanag\\RiderProjects\\SudokuSolver\\SudokuSolver\\Data/sudoku.csv";
        var reader = new StreamReader(fileLocation);
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = header
        };
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var i = 0;
        while (csv.Read())
        {
            if (i == index)
            {
                var oneLineSudoku = csv.GetRecord<OneLineSudoku>();
                var sudoku = ConvertToSudoku(oneLineSudoku);
                break;
            }
            i += 1;
        }
    }

    private Sudoku ConvertToSudoku(OneLineSudoku oneLineSudoku)
    {
        var n = GetSudokuSize(oneLineSudoku);
        var legalValues = Enumerable.Range(1, n).ToList();
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
                    SquareValue = 1 + 3 * rowIntegerDivision + colIntegerDivision,
                    Value = cellValue,
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

    private int GetSudokuSize(OneLineSudoku oneLineSudoku)
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
    
    public class OneLineSudoku
    {
        [Name("puzzle")]
        public required string Puzzle { get; set; }
        [Name("solution")]
        public required string Solution { get; set; }
    }
}