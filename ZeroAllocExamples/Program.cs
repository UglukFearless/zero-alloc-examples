using BenchmarkDotNet.Running;

Console.WriteLine("=== Starting Zero Allocation Benchmarks ===");

var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

Console.WriteLine("=== Benchmarks Completed ===");
Console.WriteLine("Check the results in the 'BenchmarkDotNet.Artifacts' folder.");
