@echo off

set "vpBasePath=D:\Code\Github\VPBase5"

echo Deleting old custom files

rmdir /s /q custom

set "vpBaseCustomFolderPath=%vpBasePath%\src\custom"

echo Copy files from VPBase custom folder: '%vpBaseCustomFolderPath%'

xcopy %vpBaseCustomFolderPath% custom /E /H /C /I

xcopy /Y "%vpBasePath%\Directory.Packages.props" "..\Directory.Packages.props*"
xcopy /Y "%vpBasePath%\nuget.config" "..\nuget.config*"

set "vpBaseToolsFolderPath=%vpBasePath%\src\tools"

xcopy %vpBaseToolsFolderPath% tools /E /H /C /I

echo.
echo Deleting VPBase.Custom.Core bin and obj files

rmdir /s /q custom\VPBase.Custom.Core\bin
rmdir /s /q custom\VPBase.Custom.Core\obj

echo Deleting VPBase.Custom.Server bin and obj files

rmdir /s /q custom\VPBase.Custom.Server\bin
rmdir /s /q custom\VPBase.Custom.Server\obj

echo Deleting VPBase.Custom.Test bin and obj files

rmdir /s /q custom\VPBase.Custom.Tests\bin
rmdir /s /q custom\VPBase.Custom.Tests\obj

echo Deleting third party vpbase files

rmdir /s /q custom\thirdparty\vpbase

pause
