@echo off

set ORIGINAL_PATH=%CD%

set UNITY_PATH="C:\Program Files\Unity"

if not exist %UNITY_PATH% (
    set UNITY_PATH="C:\Program Files (x86)\Unity"
)

set MCS=%UNITY_PATH%\Editor\Data\MonoBleedingEdge\bin\mcs.bat
set MDOC=%ORIGINAL_PATH%\mdoc-net-2010-01-04\mdoc
set LIBS_DIR=%UNITY_PATH%\Editor\Data\Managed
set EXT_LIBS_DIR=%UNITY_PATH%\Editor\Data\UnityExtensions\Unity\GUISystem\4.6.1

echo ---------------------------------------------------------------
echo Path to mcs  = %MCS%
echo Path to mdoc = %MDOC%
echo Generating documentation
echo ---------------------------------------------------------------
echo.

rem ------------------------------------------------

cd ..\Development\Assets\Scripts\UnityTranslation

rmdir /S /Q docs

call %MCS% -lib:%LIBS_DIR% -lib:%EXT_LIBS_DIR% -r:UnityEngine -r:UnityEngine.UI /doc:doc.xml -target:library -out:lib.dll *.cs Generated\*.cs Generated\UI\*.cs
call %MDOC% update --lib:%LIBS_DIR% --lib:%EXT_LIBS_DIR% -r:UnityEngine -r:UnityEngine.UI -i doc.xml -o doc_stubs lib.dll
call %MDOC% export-html -o docs doc_stubs

rmdir /S /Q doc_stubs
del doc.xml
del lib.dll

rem ------------------------------------------------

cd %ORIGINAL_PATH%

call update_sources.bat /q

if [%1]==[] (
    pause
)
