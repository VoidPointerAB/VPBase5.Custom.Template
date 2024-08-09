@echo off

set serverProjectPath=VPBase.Custom.Server\VPBase.Custom.Server.csproj
set environmentName=Staging.vpazurelinux
set environmentMode=Staging
set ignorePauseInBuild=false
set runTime=linux-x64

set runDeployFileName=run_azure_local_git_deploy.bat
set "appname=VPBase5.Custom.Develop.Staging.VPAzure.Linux2.Server"
set "giturl=https://app-vp-linuxtest2.scm.azurewebsites.net:443/app-vp-linuxtest2.git"
set "webbAppUrl=https://app-vp-linuxtest2.azurewebsites.net"

call ..\tools\VPBase4.Build\build_dist_template.bat

echo.

:choice
set /P c=Do you want to deploy automatically [y/n]?
if /I "%c%" EQU "y" goto :deployauto
if /I "%c%" EQU "n" goto :deployexit
goto :choice

:deployauto
echo.
call dist\\%dirname%\\%runDeployFileName%
exit

:deployexit
