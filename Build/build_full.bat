@echo off

set ORIGINAL_PATH=%CD%

rem ------------------------------------------------

call generate_docs.bat /q

cd %ORIGINAL_PATH%\..\Demo
call build.bat /q

rem ------------------------------------------------

pause
