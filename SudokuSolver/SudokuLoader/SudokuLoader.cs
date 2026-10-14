using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using SudokuSolver.Contracts;

namespace SudokuSolver.SudokuLoader;

public static class SudokuLoader
{
    public static Sudoku LoadSudoku(string? fileLocation, int index = 0, bool hasHeader = true)
    {
        var timer = Stopwatch.StartNew();

        if (fileLocation == null)
        {
            var directory = Directory.GetCurrentDirectory();
            fileLocation = $"{directory}/Data/sudoku.csv";
        }
        using var stream = new FileStream(fileLocation, FileMode.Open, FileAccess.Read, FileShare.Read, 1 << 16);

        string? headerLine = null;
        if (hasHeader)
        {
            using var headerReader = new StreamReader(stream, leaveOpen: true);
            headerLine = headerReader.ReadLine();
            stream.Position = 0;
        }

        // Find the requested row by counting raw newline bytes, so skipped rows are never parsed
        var lineNumber = (long)index + (hasHeader ? 1 : 0);
        var lineStart = FindLineStart(stream, lineNumber, out var totalLines);
        if (lineStart < 0)
        {
            var maxIndex = totalLines - (hasHeader ? 1 : 0) - 1;
            throw new IndexOutOfRangeException(
                $"Requested index {index} exceeds the maximum index in data set ({maxIndex})");
        }
        stream.Position = lineStart;
        using var lineReader = new StreamReader(stream, leaveOpen: true);
        var line = lineReader.ReadLine();

        var csvText = headerLine == null ? line : $"{headerLine}\n{line}";
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = hasHeader
        };
        using var csv = new CsvReader(new StringReader(csvText!), config);
        var oneLineSudoku = csv.GetRecords<OneLineSudoku>().First();
        Console.WriteLine($"Sudoku read ({timer.ElapsedMilliseconds} ms)");

        var sudoku = OneLineSudokuConverter.ConvertToSudoku(oneLineSudoku);
        Console.WriteLine($"Sudoku loaded ({timer.ElapsedMilliseconds} ms)");
        return sudoku;
    }

    /// <summary>
    /// Returns the byte offset where the given (0-based) line starts, or -1 if the file has fewer lines.
    /// totalLines is the number of lines in the file when the line is not found.
    /// </summary>
    private static long FindLineStart(Stream stream, long lineNumber, out long totalLines)
    {
        totalLines = 0;
        if (lineNumber == 0) 
            return stream.Length > 0 ? 0 : -1;

        var buffer = new byte[1 << 20];
        long newlinesSeen = 0;
        long position = 0;
        var lastByte = (byte)'\n';
        int bytesRead;
        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            var span = buffer.AsSpan(0, bytesRead);
            var newlines = span.Count((byte)'\n');
            if (newlinesSeen + newlines >= lineNumber)
            {
                var offset = 0;
                while (true)
                {
                    offset += span[offset..].IndexOf((byte)'\n') + 1;
                    if (++newlinesSeen == lineNumber) 
                        break;
                }
                var lineStart = position + offset;
                if (lineStart < stream.Length) 
                    return lineStart;
                // The newline was the last byte of the file, so there is no line after it
                totalLines = newlinesSeen;
                return -1;
            }
            newlinesSeen += newlines;
            position += bytesRead;
            lastByte = span[^1];
        }
        // A final line without a trailing newline still counts as a line
        totalLines = newlinesSeen + (lastByte == (byte)'\n' ? 0 : 1);
        return -1;
    }

    public class OneLineSudoku
    {
        [Name("puzzle")]
        public required string Puzzle { get; set; }
        [Name("solution")]
        public required string Solution { get; set; }
    }
}
