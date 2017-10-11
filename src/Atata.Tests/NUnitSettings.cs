using NUnit.Framework;

[assembly: LevelOfParallelism(4)]
[assembly: Parallelizable(ParallelScope.Fixtures)]

#if !NETCOREAPP2_0
[assembly: SetCulture("en-us")]
[assembly: SetUICulture("en-us")]
#endif
