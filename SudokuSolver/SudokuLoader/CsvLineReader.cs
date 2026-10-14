namespace SudokuSolver.SudokuLoader;

public static class CsvLineReader
{
    private const int BufferSize = 1 << 20;

    public static string ReadLine(Stream stream, long lineIndex)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(lineIndex);

        stream.Position = 0;

        var lineStart = FindLineStart(
            stream,
            lineIndex,
            out var totalLines);

        if (lineStart < 0)
        {
            throw new IndexOutOfRangeException(
                $"Requested line {lineIndex} does not exist. " +
                $"The file contains {totalLines} lines.");
        }

        stream.Position = lineStart;

        using var reader = new StreamReader(
            stream,
            leaveOpen: true);

        return reader.ReadLine()
            ?? throw new EndOfStreamException();
    }

    private static long FindLineStart(
        Stream stream,
        long lineIndex,
        out long totalLines)
    {
        totalLines = 0;

        if (lineIndex == 0)
        {
            totalLines = stream.Length > 0 ? 1 : 0;
            return stream.Length > 0 ? 0 : -1;
        }

        var buffer = new byte[BufferSize];

        long newlinesSeen = 0;
        long bytesProcessed = 0;

        var lastByte = (byte)'\n';

        int bytesRead;

        while ((bytesRead = stream.Read(buffer)) > 0)
        {
            var chunk = buffer.AsSpan(0, bytesRead);
            var newlineCount = chunk.Count((byte)'\n');

            if (newlinesSeen + newlineCount >= lineIndex)
            {
                var offset = 0;

                while (newlinesSeen < lineIndex)
                {
                    var nextNewline =
                        chunk[offset..].IndexOf((byte)'\n');

                    offset += nextNewline + 1;
                    newlinesSeen++;
                }

                var lineStart = bytesProcessed + offset;

                if (lineStart < stream.Length)
                    return lineStart;

                totalLines = newlinesSeen;
                return -1;
            }

            newlinesSeen += newlineCount;
            bytesProcessed += bytesRead;
            lastByte = chunk[^1];
        }

        totalLines = newlinesSeen +
            (lastByte == (byte)'\n' ? 0 : 1);

        return -1;
    }
}