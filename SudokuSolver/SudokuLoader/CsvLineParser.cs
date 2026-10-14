namespace SudokuSolver.SudokuLoader;

using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

public static class SudokuCsvParser
{
    public static SudokuLoader.OneLineSudoku Parse(
        string puzzleLine,
        string? headerLine)
    {
        var csvText = headerLine is null
            ? puzzleLine
            : $"{headerLine}\n{puzzleLine}";

        var config = new CsvConfiguration(
            CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = headerLine is not null
        };

        using var reader = new StringReader(csvText);
        using var csv = new CsvReader(reader, config);

        return csv
            .GetRecords<SudokuLoader.OneLineSudoku>()
            .Single();
    }
}
