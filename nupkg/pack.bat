REM "..\tools\gitlink\GitLink.exe" ..\ -u https://github.com/osoykan/FluentAssemblyScanner -c release

REM @ECHO OFF
REM SET /P VERSION_SUFFIX=Please enter version-suffix (can be left empty): 

dotnet "pack" "..\src\FluentAssemblyScanner" -c "Release" -o "."

pause
