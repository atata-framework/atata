Write-Output "Started";

$folderPath = $PSScriptRoot + "\Atata\Extensions\";
$sourceFile = $folderPath + "Clickable1Extensions.cs";
Write-Output "Source File=" + $folderPath;
$sourceContent = Get-Content $sourceFile;

New-Item ($folderPath + "Clickable2Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Clickable2Extensions").
	replace("_Clickable<TOwner>", "_Clickable<TNavigateTo, TOwner>").
	replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
	replace("where TOwner : PageObject<TOwner>", "where TOwner : PageObject<TOwner>`r`n            where TNavigateTo : PageObject<TNavigateTo>") |
	Set-Content ($folderPath + "Clickable2Extensions.cs") -force;

New-Item ($folderPath + "Link1Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Link1Extensions").
	replace("_Clickable<TOwner>", "_Link<TOwner>") |
	Set-Content ($folderPath + "Link1Extensions.cs") -force;

New-Item ($folderPath + "Link2Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Link2Extensions").
	replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
	replace("_Clickable<TOwner>", "_Link<TNavigateTo, TOwner>").
	replace("where TOwner : PageObject<TOwner>", "where TOwner : PageObject<TOwner>`r`n            where TNavigateTo : PageObject<TNavigateTo>") |
	Set-Content ($folderPath + "Link2Extensions.cs") -force;

Write-Output "Finished";
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")