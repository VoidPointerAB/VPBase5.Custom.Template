@echo off

echo In serverProjectPath=%serverProjectPath% environmentName=%environmentName% environmentMode=%environmentMode% ignorePauseInBuild=%ignorePauseInBuild% runDeployFileName=%runDeployFileName% publishProfile=%publishProfile% runtime=%runTime%

echo.
echo *******************************************************************
echo *   VPBase Distribution Template Build Started v1.3
echo *
echo *   ServerProjectPath           : %serverProjectPath%
echo *   EnvironmentName             : %environmentName% 
echo *   EnvironmentMode             : %environmentMode%
echo *   RunDeployFileName           : %runDeployFileName%
echo *   IgnorePauseInBuild          : %ignorePauseInBuild%
echo *   PublishProfile              : %publishProfile%
echo *   RunTime                     : %runTime%
echo *******************************************************************
echo.

timeout 3

set errorMessage="ERROR!"

if defined serverProjectPath goto :arg_serverPath_exists
set errorMessage="ServerProjectPath is missing!"
goto :error

:arg_serverPath_exists

if defined environmentName goto :arg_environmentName_exists
set errorMessage="EnvironmentName is missing!"
goto :error

:arg_environmentName_exists

if defined environmentMode goto :arg_environmentMode_exists
set errorMessage="EnvironmentMode is missing!"
goto :error

:arg_environmentMode_exists

if defined runDeployFileName goto :arg_runDeployFileName_exists
set runDeployFileName="run.deploy.test.bat"
echo Create default run deploy file name: %runDeployFileName%

:arg_runDeployFileName_exists

if defined publishProfile goto :arg_runTime_exists
set publishProfile="Release"
echo Default PublishProfile: %publishProfile%

:arg_runTime_exists

if not defined runTime goto :start
set runTimeArgs="/p:runtime=%runTime%"
echo RunTimeArgs: %runTimeArgs%

:start

echo Remove old dist folder
rmdir /q /s "dist"

echo Create dist folder
set tempdate=%DATE:/=%
set temptime=%TIME::=%
set temptime=%temptime: =0%

for /f %%i in ('git rev-parse --short HEAD') do set currgit=%%i
set environmentNameFolderPart=%environmentName:.=_%

set dirname="%tempdate:~0,4%%tempdate:~5,2%%tempdate:~8,2%_%temptime:~0,4%_%currgit%_%environmentNameFolderPart%"

mkdir dist
mkdir dist\\%dirname%

dotnet clean

echo *******************************************************************
echo *	Build environment - START
echo *******************************************************************

dotnet publish %serverProjectPath% /p:PublishProfile=%publishProfile% /p:EnvironmentName=%environmentName% -o dist\\%dirname% %runTimeArgs%

echo *******************************************************************
echo *	Build environment - END
echo *******************************************************************

timeout 2

echo ExecutingPath: %~dp0
%~dp0\VoidPointer.Build.Command\VoidPointer.Build.Command.exe dist\\%dirname%

echo.
echo *******************************************************************
echo *   VPBase Distribution Build Finished
echo *   EnvironmentName: %environmentName% 
echo *******************************************************************
echo.

timeout 3

echo.
echo Clean up started...
echo.

rename dist\\%dirname%\\appsettings.%environmentName%.json appsettings.tempenvname.json
rename dist\\%dirname%\\appsettings.%environmentMode%.json appsettings.tempenvmode.json
rename dist\\%dirname%\\%runDeployFileName% runtempDeploy.bat

del /s dist\\%dirname%\\appsettings.Production*.json
del /s dist\\%dirname%\\appsettings.Stage*.json
del /s dist\\%dirname%\\appsettings.Staging*.json
del /s dist\\%dirname%\\appsettings.Development*.json
del /s dist\\%dirname%\\appsettings.Demo*.json
del /s dist\\%dirname%\\run_deploy*.bat

rename dist\\%dirname%\\appsettings.tempenvname.json appsettings.%environmentName%.json
rename dist\\%dirname%\\appsettings.tempenvmode.json appsettings.%environmentMode%.json
rename dist\\%dirname%\\runtempDeploy.bat %runDeployFileName%

del /q /s dist\\%dirname%\\wwwroot\\AdvancedFilterIndexes\\*.*
del /q /s dist\\%dirname%\\wwwroot\\SearchIndexes\\*.*

mkdir dist\\\%dirname%\\logs

echo.
echo *******************************************************************
echo *   VPBase Distribution Build and Cleanup finished!
echo *******************************************************************

goto :exitProgram

:error 
echo ERROR! %errorMessage%
goto :exitProgram

:exitProgram

echo.
echo Escaping distribution build template script!
echo.

if defined ignorePauseInBuild goto :exit
pause

:exit

