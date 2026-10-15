// See https://aka.ms/new-console-template for more information

using SudokuSolver;
using SudokuSolver.SudokuLoader;

int sudokuIndex = args.Length > 0 ? int.Parse(args[0]) : 0;
var originalSudoku = SudokuLoader.LoadSudoku(null, sudokuIndex);

SolveEntireSudoku.AttemptToSolve(originalSudoku);
