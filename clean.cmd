@echo off
taskkill /im VBCSCompiler.exe /f
cd src

rem What are the ERRORLEVEL values set by internal cmd.exe commands?
rem https://stackoverflow.com/questions/34987885/what-are-the-errorlevel-values-set-by-internal-cmd-exe-commands
if %errorlevel% neq 0 (
    goto erro
) 

for /r /d %%A in (bin) do if exist %%A\NUL rmdir /s /q "%%A"
for /r /d %%A in (obj) do if exist %%A\NUL rmdir /s /q "%%A"
cd ..

goto fim

:erro
pause

:fim