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
        using var reader = new StreamReader(fileLocation);

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = header
        };
        using var csv = new CsvReader(reader, config);

        if (header)
        {
            csv.Read();
            csv.ReadHeader();
        }

        // Only parse the requested row; skip the rest without creating records
        var rowsRead = 0;
        var found = false;
        while (csv.Read())
        {
            if (rowsRead == index)
            {
                found = true;
                break;
            }
            rowsRead++;
        }
        if (!found)
        {
            throw new IndexOutOfRangeException(
                $"Requested index {index} exceeds the maximum index in data set ({rowsRead - 1})");
        }
        var oneLineSudoku = csv.GetRecord<OneLineSudoku>();
        Console.WriteLine($"Sudoku read ({timer.ElapsedMilliseconds} ms)");

        var sudoku = OneLineSudokuConverter.ConvertToSudoku(oneLineSudoku);
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