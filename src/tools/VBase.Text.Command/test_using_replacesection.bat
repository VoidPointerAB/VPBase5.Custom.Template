@echo off

for /f "tokens=2 delims==" %%i in ('"wmic os get localdatetime /value"') do set datetime=%%i
set currentDateTime=%datetime:~0,4%-%datetime:~4,2%-%datetime:~6,2%_%datetime:~8,2%-%datetime:~10,2%-%datetime:~12,2%

publish\VPBase.Text.Command.exe -replacesection "testfile.txt" "TEST START" "TEST END" %currentDateTime%

pause