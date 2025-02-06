Console.WriteLine(
"""
Use template:
using Atata;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<SomeBenchmarks>();

[MemoryDiagnoser]
public class SomeBenchmarks
{
    [Benchmark]
    public void SomeBenchmark()
    {
    }
}
""");
