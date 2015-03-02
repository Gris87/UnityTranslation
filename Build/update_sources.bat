@echo off

set ORIGINAL_PATH=%CD%

rem ------------------------------------------------

cd ..\Demo\Assets\ImportedAssets\UnityTranslation

del /F /Q *

robocopy %ORIGINAL_PATH%\..\Development\Assets\Scripts\UnityTranslation\      .\ *

rem ------------------------------------------------

cd %ORIGINAL_PATH%

if [%1]==[] (
    pause
)
