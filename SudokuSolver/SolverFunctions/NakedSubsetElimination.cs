using SudokuSolver.Contracts;

namespace SudokuSolver.SolverFunctions;

public class NakedSubsetElimination : ISolverFunction
{
    public Sudoku Execute(Sudoku sudoku)
    {
        var outputSudoku = sudoku;
        outputSudoku = EliminateCandidatesFromSubset(outputSudoku, c => c.Row);
        outputSudoku = EliminateCandidatesFromSubset(outputSudoku, c => c.Column);
        outputSudoku = EliminateCandidatesFromSubset(outputSudoku, c => c.Square);
        if (outputSudoku != sudoku)
            outputSudoku = Execute(outputSudoku);
        return outputSudoku;
    }
    

    private static Sudoku EliminateCandidatesFromSubset(Sudoku sudoku, Func<Cell, int> cellUnitSelector)
    {
        var remainingValuesByCell = new Dictionary<Cell, HashSet<int>>(ReferenceEqualityComparer.Instance);

        foreach (var unit in sudoku.Cells
                .Where(c => c.CellValue == null)
                .GroupBy(cellUnitSelector)
            )
        {
            var nakedCandidates =  unit
                .GroupBy(c => c.PossibleValues, new HashSetComparer())
                .Where(g => g.Count() == g.Key.Count);

            foreach (var candidate in nakedCandidates)
            {
                var candidateValues = candidate.Key;
                var otherCells = unit.Except(candidate);
                
                foreach (var cell in otherCells)
                {
                    if (!cell.PossibleValues.Overlaps(candidateValues))
                        continue;
                    if (!remainingValuesByCell.TryGetValue(cell, out var remainingValues))
                    {
                        remainingValues = new HashSet<int>(cell.PossibleValues);
                        remainingValuesByCell[cell] = remainingValues;
                    }
                    remainingValues.ExceptWith(candidateValues);
                }
            }
        }

        if (remainingValuesByCell.Count == 0)
            return sudoku;

        return sudoku with
        {
            Cells = sudoku.Cells
                .Select(c => remainingValuesByCell.TryGetValue(c, out var remainingValues)
                    ? c with { PossibleValues = remainingValues }
                    : c)
                .ToList()
        };
    }
    
    private class HashSetComparer : IEqualityComparer<HashSet<int>>
    {
        public bool Equals(HashSet<int>? x, HashSet<int>? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null || y is null) return false;

            return x.SetEquals(y);
        }

        public int GetHashCode(HashSet<int> obj)
        {
            var hash = 0;

            foreach (var value in obj)
                hash ^= value.GetHashCode();

            return hash;
        }
    }
}