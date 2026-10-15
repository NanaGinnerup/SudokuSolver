// See https://aka.ms/new-console-template for more information

using SudokuSolver;
using SudokuSolver.CommandLine;
using SudokuSolver.SudokuLoader;

var options = CommandLineOptions.Parse(args);

if (options.ShowHelp)
{
    CommandLineOptions.PrintHelp();
    return;
}
var originalSudoku = SudokuLoader.LoadSudoku(null, options.SudokuIndex, options.HasHeader);

SolveEntireSudoku.AttemptToSolve(originalSudoku);
