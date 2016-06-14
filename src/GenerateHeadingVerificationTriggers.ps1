Write-Output "Started";

$folderPath = $PSScriptRoot + "\Atata\Attributes\Triggers\";
$sourceFile = $folderPath + "VerifyH1Attribute.cs";
Write-Output "Source File=" $sourceFile;
$sourceContent = Get-Content $sourceFile;

New-Item ($folderPath + "VerifyH2Attribute.cs") -type file -force;
$sourceContent.
	replace("H1", "H2").
	replace("h1", "h2") |
	Set-Content ($folderPath + "VerifyH2Attribute.cs") -force;

New-Item ($folderPath + "VerifyH3Attribute.cs") -type file -force;
$sourceContent.
	replace("H1", "H3").
	replace("h1", "h3") |
	Set-Content ($folderPath + "VerifyH3Attribute.cs") -force;

New-Item ($folderPath + "VerifyH4Attribute.cs") -type file -force;
$sourceContent.
	replace("H1", "H4").
	replace("h1", "h4") |
	Set-Content ($folderPath + "VerifyH4Attribute.cs") -force;

New-Item ($folderPath + "VerifyH5Attribute.cs") -type file -force;
$sourceContent.
	replace("H1", "H5").
	replace("h1", "h5") |
	Set-Content ($folderPath + "VerifyH5Attribute.cs") -force;

New-Item ($folderPath + "VerifyH6Attribute.cs") -type file -force;
$sourceContent.
	replace("H1", "H6").
	replace("h1", "h6") |
	Set-Content ($folderPath + "VerifyH6Attribute.cs") -force;

Write-Output "Finished";
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")