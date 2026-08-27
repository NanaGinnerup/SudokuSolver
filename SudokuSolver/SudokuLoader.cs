using System.Globalization;
using SudokuSolver.Contracts;
using CsvHelper;

namespace SudokuSolver;

public class SudokuLoader
{
    public SudokuLoader(string? fileLocation)
    {
        // fileLocation ??= "/Data/sudoku.csv";
        fileLocation ??= "C:\\Users\\nanag\\RiderProjects\\SudokuSolver\\SudokuSolver\\Data/sudoku.csv";
        var reader = new StreamReader(fileLocation);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        var sudokus = csv.GetRecords<Sudoku>().ToList();
        // return sudokus;
    }
}