using System.Diagnostics;
using CsvHelper.Configuration.Attributes;
using SudokuSolver.Contracts;

namespace SudokuSolver.SudokuLoader;

public static class SudokuLoader
{
    private const int FileBufferSize = 1 << 16;

    public static Sudoku LoadSudoku(
        string? fileLocation,
        int index = 0,
        bool hasHeader = true)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(index);

        var timer = Stopwatch.StartNew();
        var path = fileLocation ?? Path.Combine(
            Directory.GetCurrentDirectory(),
            "Data",
            "sudoku.csv");

        using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            FileBufferSize);

        var headerLine = hasHeader
            ? ReadHeader(stream)
            : null;

        var lineIndex = (long)index + (hasHeader ? 1 : 0);

        var puzzleLine = CsvLineReader.ReadLine(
            stream,
            lineIndex,
            hasHeader);
        Console.WriteLine($"Sudoku read ({timer.ElapsedMilliseconds} ms)");

        var record = SudokuCsvParser.Parse(
            puzzleLine,
            headerLine);
        Console.WriteLine($"Sudoku parsed ({timer.ElapsedMilliseconds} ms)");

        return OneLineSudokuConverter.ConvertToSudoku(record);
    }

    private static string ReadHeader(Stream stream)
    {
        stream.Position = 0;

        using var reader = new StreamReader(
            stream,
            leaveOpen: true);

        return reader.ReadLine()
               ?? throw new InvalidDataException(
                   "The CSV file does not contain a header.");
    }

    public class OneLineSudoku
    {
        [Name("puzzle"), Index(0)]
        public required string Puzzle { get; set; }

        [Name("solution"), Index(1)]
        public required string Solution { get; set; }
    }
}
