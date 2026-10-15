namespace SudokuSolver.CommandLine;

public sealed record CommandLineOptions(
    int SudokuIndex = 0,
    bool ShowHelp = false)
{
    public static CommandLineOptions Parse(string[] args)
    {
        if (args.Contains("--help") || args.Contains("-h"))
            return new CommandLineOptions(ShowHelp: true);

        if (args.Length == 0)
            return new CommandLineOptions();

        var value = args[0] is "-i" or "--sudokuIndex"
            ? args.ElementAtOrDefault(1)
              ?? throw new ArgumentException("Missing Sudoku index.")
            : args[0];

        if (!int.TryParse(value, out var index) || index < 0)
            throw new ArgumentException("Sudoku index must be a non-negative integer.");

        return new CommandLineOptions(SudokuIndex: index);
    }

    public static void PrintHelp()
    {
        Console.WriteLine("""
                          Usage: dotnet run -- [options]

                          Options:
                            -i, --sudokuIndex <index>  Sudoku index (default: 0)
                            -h, --help                 Show this help message

                          Examples:
                            dotnet run -- 5
                            dotnet run -- --sudokuIndex 5
                          """);
    }
}