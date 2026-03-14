
using BenchmarkDotNet.Attributes;
using System.Security.Cryptography;

namespace ZeroAllocExamples.Demos;

[MemoryDiagnoser]
public class HashingDemo : IDemo
{
    private byte[] _data = new byte[1024];
    private readonly MD5 _md5 = MD5.Create();

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0;  i < 1024; i++)
            _data[i] = (byte) i;
    }

    [Benchmark(Baseline  = true)]
    public int HashsumWithHeap()
    {
        var hash = _md5.ComputeHash(_data);
        return Consume(hash);
    }

    [Benchmark]
    public int HashsumWithStackAlloc()
    {
        Span<byte> hash = stackalloc byte[16];
        _md5.TryComputeHash(_data, hash, out _);
        return Consume(hash);
    }

    private static int Consume(ReadOnlySpan<byte> data)
    {
        int checksum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            checksum += data[i];
        }
        return checksum;
    }
}
