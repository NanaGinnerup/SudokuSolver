using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using SudokuSolver.Contracts;

namespace SudokuSolver.SudokuLoader;

public static class SudokuLoader
{
    public static Sudoku LoadSudoku(string? fileLocation, int index = 0, bool header = true)
    {
        var timer = Stopwatch.StartNew();
        
        if (fileLocation == null)
        {
            var directory = Directory.GetCurrentDirectory();
            fileLocation = $"{directory}/Data/sudoku.csv";
        }
        var reader = new StreamReader(fileLocation);
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = header
        };
        var csv = new CsvReader(reader, config);

        var allSudoku = csv
            .GetRecords<OneLineSudoku>()
            .ToList();
        Console.WriteLine($"Sudokus read ({timer.ElapsedMilliseconds} ms)");
        
        var numberSudoku = allSudoku.Count;
        if (index >= numberSudoku)
        {
            throw new IndexOutOfRangeException(
                $"Requested index {index} exceeds the maximum index in data set ({numberSudoku - 1})");
        }
        var oneLineSudoku = allSudoku
            .Skip(index)
            .FirstOrDefault();
        
        var sudoku = OneLineSudokuConverter.ConvertToSudoku(oneLineSudoku!);
        Console.WriteLine($"Sudoku loaded ({timer.ElapsedMilliseconds} ms)");
        return sudoku;
    }
    
    public class OneLineSudoku
    {
        [Name("puzzle")]
        public required string Puzzle { get; set; }
        [Name("solution")]
        public required string Solution { get; set; }
    }
}