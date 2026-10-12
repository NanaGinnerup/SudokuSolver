// See https://aka.ms/new-console-template for more information

using SudokuSolver;
using SudokuSolver.SudokuLoader;


var originalSudoku = SudokuLoader.LoadSudoku(null, 2);

SolveEntireSudoku.AttemptToSolve(originalSudoku);