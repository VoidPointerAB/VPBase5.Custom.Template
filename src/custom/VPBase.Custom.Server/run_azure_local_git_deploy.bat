:: General Deploy file to Azure Web Application Local Git Strategy

@echo off

IF [%appname%] == [] goto :noAppName
IF [%giturl%] == [] goto :noAppName

echo *******************************************************************
echo *   Deploy to Azure using Web Application Local Git Strategy v1.2
echo *   Appname: %appname%
echo *   Giturl: %giturl%
echo *******************************************************************


:: Set Temp Folder Path
set "tp=%temp%\vp_azure_deploy\%appname%"

:choice
set /P c=Are you sure you want to deploy [Y/N]?
if /I "%c%" EQU "Y" goto :deploy
if /I "%c%" EQU "N" goto :abort
goto :choice

:deploy

echo Deploying

@echo off
for %%a in ("%~dp0\.") do set "parent=%%~nxa"

set /p comment="Enter deploy comment: "

set gitDir=%tp%

IF not exist %gitDir% (
	mkdir %gitDir%
	git clone %giturl% %gitDir% 
) ELSE (
	cd /D %gitDir%
	git pull
	cd ..
)

for /d %%i in ("%gitDir%\*") do if /i not "%%~nxi"==".git" del /s /q "%%i"
del /q %gitDir%\*.*

xcopy /s %~dp0. %gitDir%

cd /D %gitDir%

echo TODO: Check number of files copied before continue!!!! Should be at least 100 files. Otherwise abort!
pause

git add -A
git commit -m "%comment% - %parent%"
git push

echo Deploy complete! :)

IF [%webbAppUrl%] == [] goto :exitscript

:choiceBrowse
set /P c=Do you want to browse to the application [Y/N]?
if /I "%c%" EQU "Y" goto :browse
if /I "%c%" EQU "N" goto :exitscript
goto :choiceBrowse

:browse
set browser=chrome.exe
set wait_time=2
start %browser% -new-window %webbAppUrl%

:exitscript
pause
exit


:noAppName
echo No appname present. Please check environment config.
goto :abort

:abort
echo No deploy, exiting...

pause
