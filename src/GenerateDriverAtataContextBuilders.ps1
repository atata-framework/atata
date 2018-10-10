Write-Output "Started";

$folderPath = $PSScriptRoot + "\Atata\Context\";
$sourceFile = $folderPath + "ChromeAtataContextBuilder.cs";
Write-Output "Source File=" $sourceFile;
$sourceContent = Get-Content $sourceFile;

New-Item ($folderPath + "FirefoxAtataContextBuilder.cs") -type file -force;
$sourceContent.
	replace("Chrome", "Firefox") |
	Set-Content ($folderPath + "FirefoxAtataContextBuilder.cs") -force;

New-Item ($folderPath + "EdgeAtataContextBuilder.cs") -type file -force;
$sourceContent.
	replace("Chrome", "Edge") |
	Set-Content ($folderPath + "EdgeAtataContextBuilder.cs") -force;

New-Item ($folderPath + "OperaAtataContextBuilder.cs") -type file -force;
$sourceContent.
	replace("Chrome", "Opera") |
	Set-Content ($folderPath + "OperaAtataContextBuilder.cs") -force;

New-Item ($folderPath + "SafariAtataContextBuilder.cs") -type file -force;
$sourceContent.
	replace("Chrome", "Safari") |
	Set-Content ($folderPath + "SafariAtataContextBuilder.cs") -force;

New-Item ($folderPath + "InternetExplorerAtataContextBuilder.cs") -type file -force;
$sourceContent.
	replace("OpenQA.Selenium.Chrome", "OpenQA.Selenium.IE").
	replace("Chrome", "InternetExplorer") |
	Set-Content ($folderPath + "InternetExplorerAtataContextBuilder.cs") -force;

Write-Output "Finished";
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")