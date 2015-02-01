@echo off

set ORIGINAL_PATH=%CD%

rem ------------------------------------------------

cd ..\Demo\Assets\ImportedAssets\UnityTranslation

del /F /Q *

robocopy %ORIGINAL_PATH%\..\Development\Assets\Scripts\UnityTranslation\      .\ *
robocopy %ORIGINAL_PATH%\..\Development\Assets\Scripts\UnityTranslation\.docs .docs /MIR

rem ------------------------------------------------

cd %ORIGINAL_PATH%

if [%1]==[] (
    pause
)
