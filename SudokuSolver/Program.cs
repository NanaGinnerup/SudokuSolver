// See https://aka.ms/new-console-template for more information

using SudokuSolver;
using SudokuSolver.SudokuLoader;

const int i = 0;
var originalSudoku = SudokuLoader.LoadSudoku(null, i);

SolveEntireSudoku.AttemptToSolve(originalSudoku);
