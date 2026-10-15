namespace SudokuSolver.CommandLine;

public sealed record CommandLineOptions(
    int SudokuIndex = 0,
    bool HasHeader = true,
    bool ShowHelp = false)
{
    public static CommandLineOptions Parse(string[] args)
    {
        if (args.Contains("--help") || args.Contains("-h"))
            return new CommandLineOptions(ShowHelp: true);

        var options = new CommandLineOptions();

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-i" or "--sudokuIndex":
                    options = options with
                    {
                        SudokuIndex = ParseIndex(
                            args.ElementAtOrDefault(++i) 
                            ?? throw new ArgumentException("Missing Sudoku index.")
                            )
                    };
                    break;
                case "--header":
                    var headerValue = args.ElementAtOrDefault(++i) 
                                      ?? throw new ArgumentException("Missing header value.");
                    if (!bool.TryParse(headerValue, out var hasHeader))
                        throw new ArgumentException("Header value must be 'true' or 'false'.");
                    options = options with { HasHeader = hasHeader };
                    break;
                default:
                    options = options with { SudokuIndex = ParseIndex(args[i]) };
                    break;
            }
        }

        return options;
    }

    private static int ParseIndex(string value)
    {
        if (!int.TryParse(value, out var index) || index < 0)
            throw new ArgumentException("Sudoku index must be a non-negative integer.");

        return index;
    }

    public static void PrintHelp()
    {
        Console.WriteLine("""
                          Usage: dotnet run -- [options]

                          Options:
                            -i, --sudokuIndex <index>  Sudoku index (default: 0)
                            --header <true|false>      Whether the CSV file has a header row (default: true)
                            -h, --help                 Show this help message

                          Examples:
                            dotnet run -- 5
                            dotnet run -- --sudokuIndex 5
                            dotnet run -- --sudokuIndex 5 --header false
                          """);
    }
}
