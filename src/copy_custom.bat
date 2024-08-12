@echo off

set "vpBasePath=%VPBase5GitPath%"

echo.
echo *******************************************************************
echo *   VPBase Custom Copy
echo *
echo *   VPBase5 Git Path                    : %VPBase5GitPath%
echo *
echo *******************************************************************
echo.

echo Please continue to start copying custom files from the specified VPBase5 Git Path

pause

timeout 1

echo.
echo Deleting old custom files

rmdir /s /q custom

set "vpBaseCustomFolderPath=%vpBasePath%\src\custom"

echo.
echo Copying files from VPBase custom folder: '%vpBaseCustomFolderPath%'
echo.

xcopy %vpBaseCustomFolderPath% custom /E /H /C /I

echo.
echo Copying other important files such as Directory.Packages.props, nuget.config and tools

xcopy /Y "%vpBasePath%\Directory.Packages.props" "..\Directory.Packages.props*"
xcopy /Y "%vpBasePath%\nuget.config" "..\nuget.config*"

set "vpBaseToolsFolderPath=%vpBasePath%\src\tools"

xcopy %vpBaseToolsFolderPath% tools /E /H /C /I /Y

echo.
echo Delete some copied files:
echo - VPBase.Custom.Core bin and obj files

rmdir /s /q custom\VPBase.Custom.Core\bin
rmdir /s /q custom\VPBase.Custom.Core\obj

echo - VPBase.Custom.Server bin and obj files

rmdir /s /q custom\VPBase.Custom.Server\bin
rmdir /s /q custom\VPBase.Custom.Server\obj
rmdir /s /q custom\VPBase.Custom.Server\wwwroot\base

echo - VPBase.Custom.Test bin and obj files

rmdir /s /q custom\VPBase.Custom.Tests\bin
rmdir /s /q custom\VPBase.Custom.Tests\obj

echo - Third party vpbase files
echo.

rmdir /s /q custom\thirdparty\vpbase

echo - Switch files
echo.

del /q custom\reference_replacement\*_project_reference_same_git.txt
del /q custom\reference_replacement\*_thirdparty.txt
del /q custom\switch_references_to_base_using_project_reference_same_git.bat
del /q custom\switch_references_to_base_using_thirdparty.bat

echo.

pause
