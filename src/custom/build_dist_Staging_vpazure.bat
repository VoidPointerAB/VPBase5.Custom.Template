@echo off

set serverProjectPath=VPBase.Custom.Server\VPBase.Custom.Server.csproj
set environmentName=Staging.vpazure
set environmentMode=Staging
set ignorePauseInBuild=false

set runDeployFileName=run_azure_local_git_deploy.bat
set "appname=VPBase4.Custom.Develop.Staging.VPAzure.Server"
set "giturl=https://app-vpbasecustom-develop-web-stage.scm.azurewebsites.net:443/app-vpbasecustom-develop-web-stage.git"
set "webbAppUrl=https://app-vpbasecustom-develop-web-stage.azurewebsites.net"

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
