@echo off

echo Start cleaning arifacts

for /r /d %%i in (obj.*, bin.*, nupkg.*) do (
    rmdir /s /q %%i
    echo clear folder %%i
)

echo cleaning finished
if /i not "%~1"=="nopause" pause