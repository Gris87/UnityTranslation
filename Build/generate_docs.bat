@echo off

set ORIGINAL_PATH=%CD%

rem ------------------------------------------------

rmdir /S /Q ..\Development\Assets\Scripts\UnityTranslation\.docs
rmdir /S /Q ..\Development\Assets\Scripts\UnityTranslation\.latex

cd ForDoxygen

doxygen

rem ------------------------------------------------

cd ..\..\Development\Assets\Scripts\UnityTranslation\.latex
call make.bat

cd ..
move /Y .latex\refman.pdf docs.pdf
rmdir /S /Q .latex

"C:\Program Files\7-Zip\7z" a -y docs.zip .docs
rmdir /S /Q .docs

rem ------------------------------------------------

cd %ORIGINAL_PATH%

call update_sources.bat /q

if [%1]==[] (
    pause
)
