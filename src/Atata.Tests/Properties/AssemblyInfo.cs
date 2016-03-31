using Atata;
using NUnit.Framework;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Atata.Tests")]
[assembly: Guid("9d0aa4f2-4987-4395-be95-76abc329b7a0")]

[assembly: LevelOfParallelism(4)]
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: Atata.Culture("en-us")]
[assembly: VerifyTitleSettings(StringFormat = "{0} - Atata")]