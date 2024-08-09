@echo off

echo "Remove nuget content files from the server projekt"

rmdir /q /s "VPBase.Custom.Server/wwwroot/fonts"
rmdir /q /s "VPBase.Custom.Server/wwwroot/images"
rmdir /q /s "VPBase.Custom.Server/wwwroot/js"
rmdir /q /s "VPBase.Custom.Server/wwwroot/lib"
rmdir /q /s "VPBase.Custom.Server/wwwroot/css"

pause
