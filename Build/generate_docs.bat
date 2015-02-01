@echo off

set ORIGINAL_PATH=%CD%

rem ------------------------------------------------

rmdir /S /Q ..\Development\Assets\Scripts\UnityTranslation\.docs

cd ForDoxygen

doxygen

rem ------------------------------------------------

cd %ORIGINAL_PATH%

call update_sources.bat /q

if [%1]==[] (
    pause
)
