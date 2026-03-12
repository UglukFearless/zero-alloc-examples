using BenchmarkDotNet.Attributes;

namespace ZeroAllocExamples.Demos;

[MemoryDiagnoser]
public class CsvParsingDemo : IDemo
{
    private string _csvLine = "";

    [GlobalSetup]
    public void Setup()
    {
        _csvLine = "10;20;30;40;50;60;70;80;90;100;110;120";
    }

    [Benchmark(Baseline = true)]
    public int ParseWithSplit()
    {
        var parts = _csvLine.Split(';');
        int sum = 0;
        foreach (var part in parts)
        {
            sum += int.Parse(part);
        }
        return sum;
    }

    [Benchmark]
    public int CountFieldsWithSpan()
    {
        ReadOnlySpan<char> span = _csvLine;
        int sum = 0;

        while (span.Length > 0)
        {
            int index = span.IndexOf(';');
            ReadOnlySpan<char> field = index == -1 ? span : span.Slice(0, index);

            sum += int.Parse(field);

            if (index == -1)
                break;

            span = span.Slice(index + 1);
        }
        return sum;
    }
}

public struct DummyStruct { public int Id; }
