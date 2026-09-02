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
        // fileLocation ??= "/Data/sudoku.csv";
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
                break;
            }
            i += 1;
        }
    }
    
    public class OneLineSudoku
    {
        [Name("puzzle")]
        public required string Puzzle { get; set; }
        [Name("solution")]
        public required string Solution { get; set; }
    }
}