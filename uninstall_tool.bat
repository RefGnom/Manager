@echo off

dotnet tool uninstall --global Manager.Tool

if /i not "%~1"=="nopause" pause