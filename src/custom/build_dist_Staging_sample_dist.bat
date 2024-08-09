@echo off

set serverProjectPath=VPBase.Custom.Server\VPBase.Custom.Server.csproj
set environmentName=Staging.sample
set environmentMode=Staging

call ..\tools\VPBase4.Build\build_dist_template.bat


