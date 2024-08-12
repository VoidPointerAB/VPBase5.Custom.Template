@echo off

echo.
echo Replace references in Directory.Packages.props
echo.

..\tools\VBase.Text.Command\publish\VPBase.Text.Command.exe -replacesectionfile "..\..\Directory.Packages.props" "VPBASE-REFERENCES START" "VPBASE-REFERENCES END" "reference_replacement\core_vpbase_package_nuget.txt"

echo.
echo Remove static base content
echo.

rmdir /s /q VPBase.Custom.Server\wwwroot\base


echo.
echo Replace references in VPBase.Custom.Core.csproj
echo.

..\tools\VBase.Text.Command\publish\VPBase.Text.Command.exe -replacesectionfile "VPBase.Custom.Core\VPBase.Custom.Core.csproj" "VPBASE-REFERENCES START" "VPBASE-REFERENCES END" "reference_replacement\core_base_nuget.txt"

echo.
echo Replace references in VPBase.Custom.Server.csproj
echo.

..\tools\VBase.Text.Command\publish\VPBase.Text.Command.exe -replacesectionfile "VPBase.Custom.Server\VPBase.Custom.Server.csproj" "VPBASE-REFERENCES START" "VPBASE-REFERENCES END" "reference_replacement\core_server_nuget.txt"

echo.
echo Replace references in VPBase.Custom.Tests.csproj
echo.

..\tools\VBase.Text.Command\publish\VPBase.Text.Command.exe -replacesectionfile "VPBase.Custom.Tests\VPBase.Custom.Tests.csproj" "VPBASE-REFERENCES START" "VPBASE-REFERENCES END" "reference_replacement\core_tests_nuget.txt"

echo.
echo Replace references in VPBase.Custom.sln
echo.

..\tools\VBase.Text.Command\publish\VPBase.Text.Command.exe -replacesection "VPBase.Custom.sln" "VPBASE-REFERENCES START" "VPBASE-REFERENCES END" ";"

echo.
echo. Finished!

pause