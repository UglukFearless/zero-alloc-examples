using BenchmarkDotNet.Attributes;
using System.Buffers;

namespace ZeroAllocExamples.Demos;

[MemoryDiagnoser]
public class NetworkBufferDemo : IDemo
{
    private const int BufferSize = 4096;

    [GlobalSetup]
    public void Setup() {}

    [Benchmark(Baseline = true)]
    public int ProcessPacketAsNewArray()
    {
        var buffer = new byte[BufferSize];
        int checksum = 0;
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = 1;
            checksum += buffer[i];
        }
        return checksum;
    }

    [Benchmark]
    public int ProcessPacketAsArrayPool()
    {
        var buffer = ArrayPool<byte>.Shared.Rent(BufferSize);
        try
        {
            int checksum = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 1;
                checksum += buffer[i];
            }
            return checksum;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
