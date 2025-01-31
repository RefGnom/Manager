@echo off
dotnet pack .\Tool\Manager.Tool
dotnet tool install --global --add-source ./nupkg Manager.Tool
pause