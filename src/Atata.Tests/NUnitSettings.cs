using NUnit.Framework;

[assembly: LevelOfParallelism(4)]
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: SetCulture("en-us")]
[assembly: SetUICulture("en-us")]
