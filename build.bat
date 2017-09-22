@Echo OFF
SET PATH=D:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin

MSbuild "src\Atata\Atata.csproj" /p:Configuration=Release
Echo Build process completed

..\nuget pack src\Atata.nuspec
Echo NuGet package created

Set /p Wait=Press Enter to continue...