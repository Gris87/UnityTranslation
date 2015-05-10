@echo off

set ORIGINAL_PATH=%CD%

rem ------------------------------------------------

cd ..\Demo\Assets\ImportedAssets\UnityTranslation

del /F /Q *

robocopy %ORIGINAL_PATH%\..\Development\Assets\Scripts\UnityTranslation\                         .\    *
copy %ORIGINAL_PATH%\..\Development\Assets\Scripts\UnityTranslation\Generated\Language.cs        Generated\Language.cs
copy %ORIGINAL_PATH%\..\Development\Assets\Scripts\UnityTranslation\Generated\PluralsRules.cs    Generated\PluralsRules.cs

rem ------------------------------------------------

cd %ORIGINAL_PATH%

if [%1]==[] (
    pause
)
