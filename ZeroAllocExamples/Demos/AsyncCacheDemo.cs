
using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;

namespace ZeroAllocExamples.Demos;

[MemoryDiagnoser]
public class AsyncCacheDemo : IDemo
{
    private ConcurrentDictionary<string, string> _cache = new();
    private string _key = "existing_key";

    [GlobalSetup]
    public void Setup()
    {
        _cache.TryAdd(_key, "value");
    }

    [Benchmark(Baseline = true)]
    public async Task<string?> GetDataByTask()
    {
        if (_cache.TryGetValue(_key, out var value))
        {
            return value;
        }
        return await GetFromDb();
    }

    [Benchmark]
    public async ValueTask<string?> GetDataByTaskAsync() 
    {
        if (_cache.TryGetValue(_key, out var value))
        {
            return value;
        }
        return await GetFromDb();
    }

    private async Task<string> GetFromDb()
    {
        return await Task.FromResult(_key);
    }
}
