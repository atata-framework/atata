using NUnit.Framework;

[assembly: LevelOfParallelism(4)]
[assembly: Parallelizable(ParallelScope.Fixtures)]
