Write-Output "Started";

$folderPath = $PSScriptRoot + "\Atata\Extensions\";
$sourceFile = $folderPath + "Clickable1Extensions.cs";
Write-Output "Source File=" $sourceFile;
$sourceContent = Get-Content $sourceFile;

New-Item ($folderPath + "Clickable2Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Clickable2Extensions").
	replace("Clickable<TOwner>", "Clickable<TNavigateTo, TOwner>").
	replace("ClickableControl<TOwner>", "ClickableControl<TNavigateTo, TOwner>").
	replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
	replace("where TOwner : PageObject<TOwner>", "where TOwner : PageObject<TOwner>`r`n            where TNavigateTo : PageObject<TNavigateTo>") |
	Set-Content ($folderPath + "Clickable2Extensions.cs") -force;

New-Item ($folderPath + "Link1Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Link1Extensions").
	replace("Clickable<TOwner>", "Link<TOwner>").
	replace("ClickableControl<TOwner>", "LinkControl<TOwner>") |
	Set-Content ($folderPath + "Link1Extensions.cs") -force;

New-Item ($folderPath + "Link2Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Link2Extensions").
	replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
	replace("ClickableControl<TOwner>", "LinkControl<TNavigateTo, TOwner>").
	replace("Clickable<TOwner>", "Link<TNavigateTo, TOwner>").
	replace("where TOwner : PageObject<TOwner>", "where TOwner : PageObject<TOwner>`r`n            where TNavigateTo : PageObject<TNavigateTo>") |
	Set-Content ($folderPath + "Link2Extensions.cs") -force;

New-Item ($folderPath + "Button1Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Button1Extensions").
	replace("Clickable<TOwner>", "Button<TOwner>").
	replace("ClickableControl<TOwner>", "ButtonControl<TOwner>") |
	Set-Content ($folderPath + "Button1Extensions.cs") -force;

New-Item ($folderPath + "Button2Extensions.cs") -type file -force;
$sourceContent.
	replace("Clickable1Extensions", "Button2Extensions").
	replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
	replace("ClickableControl<TOwner>", "ButtonControl<TNavigateTo, TOwner>").
	replace("Clickable<TOwner>", "Button<TNavigateTo, TOwner>").
	replace("where TOwner : PageObject<TOwner>", "where TOwner : PageObject<TOwner>`r`n            where TNavigateTo : PageObject<TNavigateTo>") |
	Set-Content ($folderPath + "Button2Extensions.cs") -force;

Write-Output "Finished";
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")