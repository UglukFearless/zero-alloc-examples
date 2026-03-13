
using BenchmarkDotNet.Attributes;
using System.Text;

namespace ZeroAllocExamples.Demos;

[MemoryDiagnoser]
public class StringBuildingDemo : IDemo
{
    private const int IterationCount = 1000;
    private readonly string _part = "LogEntryData";

    [GlobalSetup]
    public void Setup() {}

    [Benchmark(Baseline = true)]
    public string BuildWithStringBuilder()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < IterationCount; i++) 
        {
            sb.Append(_part);
            sb.Append("_");
        }
        return sb.ToString();
    }

    [Benchmark]
    public string BuildWithStringConcat()
    {
        var result = "";
        for (int i = 0; i < IterationCount; i++)
        {
            result += _part + "_";
        }
        return result;
    }

    [Benchmark]
    public string BuildWithStringCreate()
    {
        int totalLength = (_part.Length + 1) * IterationCount;
        return string.Create(totalLength, (_part, IterationCount), (span, state) =>
        {
            for (int i = 0; i < IterationCount; i++)
            {
                state._part.AsSpan().CopyTo(span);
                span = span.Slice(state._part.Length);
                span[0] = '_';
                span = span.Slice(1);
            }
        });
    }
}
