using System.ComponentModel;
using JevDotNet;
using JevDotNet.Models;
using SudokuSolver.Contracts;

namespace SudokuSolver.HintDecision;

public class HintDecision(JevClient client, int shortlistSize = 8)
{
    public async Task<Cell?> PickCellToRevealAsync(Sudoku sudoku)
    {
        var empty = sudoku.Cells.Where(c => c.CellValue is null).ToList();
        if (empty.Count == 0)
        {
            return null;
        }

        // A cell with a single candidate is one the solver will find on its own,
        // so revealing it wastes the hint. Fall back to all cells if none remain.
        var eligible = empty.Where(c => c.PossibleValues.Count > 1).ToList();
        if (eligible.Count == 0)
        {
            eligible = empty;
        }

        // Cheap deterministic shortlist: cells that touch the most unresolved peers.
        var shortlist = eligible
            .OrderByDescending(c => EmptyPeers(sudoku, c).Count)
            .ThenBy(c => c.PossibleValues.Count)
            .Take(shortlistSize)
            .ToList();

        // Let Jev rate each shortlisted cell.
        var rated = await Task.WhenAll(shortlist.Select(async cell =>
        {
            JevResponse<HintAssessment> response =
                await client.EvaluateAsync<HintAssessment>(Describe(sudoku, cell));
            return (Cell: cell, Score: response.Result.Helpfulness ?? 0.0);
        }));

        return rated
            .OrderByDescending(x => x.Score)
            .ThenByDescending(x => EmptyPeers(sudoku, x.Cell).Count) // deterministic tie-break
            .First()
            .Cell;
    }

    private static List<Cell> EmptyPeers(Sudoku sudoku, Cell cell) =>
        sudoku.Cells
            .Where(c => c != cell && c.CellValue is null &&
                        (c.Row == cell.Row || c.Column == cell.Column || c.Square == cell.Square))
            .ToList();

    private static string Describe(Sudoku sudoku, Cell cell)
    {
        var peers = EmptyPeers(sudoku, cell);
        var affected = peers.Count(p => p.PossibleValues.Overlaps(cell.PossibleValues));
        var emptyTotal = sudoku.Cells.Count(c => c.CellValue is null);

        return $"""
            Sudoku of size {sudoku.SudokuSize}x{sudoku.SudokuSize} with {emptyTotal} empty cells.
            Candidate hint cell: row {cell.Row}, column {cell.Column}, square {cell.Square}.
            It has {cell.PossibleValues.Count} possible values: {string.Join(", ", cell.PossibleValues.OrderBy(v => v))}.
            It shares a row, column or square with {peers.Count} other empty cells.
            {affected} of those cells have at least one candidate in common with it.
            """;
    }
}

public sealed class HintAssessment
{
    [JevScoreQuestion<HintValue>("How useful would revealing this cell be to help a solver finish the puzzle on their own?")]
    public double? Helpfulness { get; set; }
}

public enum HintValue
{
    [Description("Solver would find this cell easily anyway; the hint is wasted")]
    Wasted = 0,
    [Description("Resolves this cell but unlocks little else")]
    Minor = 1,
    [Description("Narrows candidates in several connected cells")]
    Helpful = 2,
    [Description("Likely unlocks a chain of further deductions")]
    Pivotal = 3
}