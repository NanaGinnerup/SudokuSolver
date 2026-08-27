using System.Globalization;
using SudokuSolver.Contracts;
using CsvHelper;

namespace SudokuSolver;

public class SudokuLoader
{
    public SudokuLoader(string? fileLocation, int index = 0)
    {
        // fileLocation ??= "/Data/sudoku.csv";
        fileLocation ??= "C:\\Users\\nanag\\RiderProjects\\SudokuSolver\\SudokuSolver\\Data/sudoku.csv";
        var reader = new StreamReader(fileLocation);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var i = 0;
        while (csv.Read())
        {
            // if (
            //     (csv.GetField<string>(0) == "puzzle") 
            //     || (csv.GetField<string>(1) == "solution")
            //     )
            // {
            //     var test = 0;
            // }
            // var fullRecord = csv.GetRecord<object>();
            if (i == index)
            {
                var fullRecord = csv.GetRecord<object>();
                break;
            }
            i += 1;
            // var all = csv.GetRecords<object>();
            // var puzzle = csv.GetField<string>(0);
            // var solution = csv.GetField<string>(1);
        }

        var header = csv.GetType();
        var sudokus = csv.GetRecord<string>();
        // var sudokus = csv.GetRecords<Sudoku>().ToList();
        // return sudokus;
    }
}