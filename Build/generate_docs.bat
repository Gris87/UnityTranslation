@echo off

set ORIGINAL_PATH=%CD%

set DOXYFILE=%ORIGINAL_PATH%\ForDoxygen\Doxyfile

rem ------------------------------------------------

cd ..\Development\Assets\Scripts\UnityTranslation

rmdir /S /Q docs

doxygen %DOXYFILE%

rem ------------------------------------------------

cd %ORIGINAL_PATH%

call update_sources.bat /q

if [%1]==[] (
    pause
)
