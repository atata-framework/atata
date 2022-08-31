Write-Output "Started";

$folderPath = $PSScriptRoot + "\Atata\Extensions\";
$sourceFile = $folderPath + "ClickableDelegate1Extensions.cs";
Write-Output "Source File=" $sourceFile;
$sourceContent = Get-Content $sourceFile;

New-Item ($folderPath + "ClickableDelegate2Extensions.cs") -type file -force;
$sourceContent.
    replace("ClickableDelegate1Extensions", "ClickableDelegate2Extensions").
    replace("ClickableDelegate<TOwner>", "ClickableDelegate<TNavigateTo, TOwner>").
    replace("Clickable<TOwner>", "Clickable<TNavigateTo, TOwner>").
    replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
    replace("where TOwner : PageObject<TOwner>", "where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>") |
    Set-Content ($folderPath + "ClickableDelegate2Extensions.cs") -force;

New-Item ($folderPath + "LinkDelegate1Extensions.cs") -type file -force;
$sourceContent.
    replace("ClickableDelegate1Extensions", "LinkDelegate1Extensions").
    replace("ClickableDelegate<TOwner>", "LinkDelegate<TOwner>").
    replace("Clickable<TOwner>", "Link<TOwner>") |
    Set-Content ($folderPath + "LinkDelegate1Extensions.cs") -force;

New-Item ($folderPath + "LinkDelegate2Extensions.cs") -type file -force;
$sourceContent.
    replace("ClickableDelegate1Extensions", "LinkDelegate2Extensions").
    replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
    replace("Clickable<TOwner>", "Link<TNavigateTo, TOwner>").
    replace("ClickableDelegate<TOwner>", "LinkDelegate<TNavigateTo, TOwner>").
    replace("where TOwner : PageObject<TOwner>", "where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>") |
    Set-Content ($folderPath + "LinkDelegate2Extensions.cs") -force;

New-Item ($folderPath + "ButtonDelegate1Extensions.cs") -type file -force;
$sourceContent.
    replace("ClickableDelegate1Extensions", "ButtonDelegate1Extensions").
    replace("ClickableDelegate<TOwner>", "ButtonDelegate<TOwner>").
    replace("Clickable<TOwner>", "Button<TOwner>") |
    Set-Content ($folderPath + "ButtonDelegate1Extensions.cs") -force;

New-Item ($folderPath + "ButtonDelegate2Extensions.cs") -type file -force;
$sourceContent.
    replace("ClickableDelegate1Extensions", "ButtonDelegate2Extensions").
    replace("<TOwner>(this", "<TNavigateTo, TOwner>(this").
    replace("Clickable<TOwner>", "Button<TNavigateTo, TOwner>").
    replace("ClickableDelegate<TOwner>", "ButtonDelegate<TNavigateTo, TOwner>").
    replace("where TOwner : PageObject<TOwner>", "where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>") |
    Set-Content ($folderPath + "ButtonDelegate2Extensions.cs") -force;

Write-Output "Finished";
$host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")