using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public interface ISolverFunction
{
    Sudoku Execute(Sudoku sudoku);
}