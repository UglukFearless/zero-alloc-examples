using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace ZeroAllocExamples.Demos;

[MemoryDiagnoser]
public class LinqFilterDemo : IDemo
{
    private readonly int[] _data = new int[1000];
    private readonly int _threshold = 500;

    private sealed class PooledIntLease : IDisposable
    {
        private int[]? _buffer;

        public int Count { get; }

        public ReadOnlySpan<int> Span => _buffer is null
            ? throw new ObjectDisposedException(nameof(PooledIntLease))
            : _buffer.AsSpan(0, Count);

        public PooledIntLease(int[] buffer, int count)
        {
            _buffer = buffer;
            Count = count;
        }

        public void Dispose()
        {
            if (_buffer is null)
                return;

            ArrayPool<int>.Shared.Return(_buffer);
            _buffer = null;
        }
    }

    [GlobalSetup]
    public void Setup()
    {
        for (int i = 0; i < _data.Length; i++)
            _data[i] = i;
    }

    [Benchmark(Baseline = true)]
    public int FilterWithLinq()
    {
        var result = _data.Where(i => i > _threshold).ToArray();
        return Consume(result);
    }

    [Benchmark]
    public int FilterWithClassLease()
    {
        using var lease = FilterWithPoolLease();
        return Consume(lease.Span);
    }

    [Benchmark]
    public int FilterWithSpan()
    {
        var buffer = ArrayPool<int>.Shared.Rent(_data.Length);
        try
        {
            int count = FilterInto(buffer);
            return Consume(buffer.AsSpan(0, count));
        }
        finally
        {
            ArrayPool<int>.Shared.Return(buffer);
        }
    }

    private PooledIntLease FilterWithPoolLease()
    {
        var buffer = ArrayPool<int>.Shared.Rent(_data.Length);
        int count = FilterInto(buffer);
        return new PooledIntLease(buffer, count);
    }

    private int FilterInto(Span<int> destination)
    {
        int count = 0;
        for (int i = 0; i < _data.Length; i++)
        {
            if (_data[i] > _threshold)
                destination[count++] = _data[i];
        }

        return count;
    }

    private static int Consume(ReadOnlySpan<int> data)
    {
        int checksum = 0;
        for (int i = 0; i < data.Length; i++)
        {
            checksum += data[i];
        }

        return checksum;
    }
}
