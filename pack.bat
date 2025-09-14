@echo off
setlocal EnableDelayedExpansion

:: Pack script - no command line arguments. Uses Release configuration and the default output folder.
:: Default output: two levels up from script folder in `.publish`.

:: Determine script root and defaults
set "SCRIPT_ROOT=%~dp0"
set "SRC_DIR=%SCRIPT_ROOT%src"
set "CONFIG=Release"
set "OUTPUT=%SCRIPT_ROOT%..\..\.publish"
set "FAILED=0"

:: Ensure output exists
if not exist "%OUTPUT%" mkdir "%OUTPUT%"
call :log "Packing run started. Configuration=%CONFIG% Output=%OUTPUT%"

:: Iterate projects under src folder
for /D %%G in ("%SRC_DIR%\*") do (
	set "PROJECT_DIR=%%~fG"
	set "PROJECT_NAME=%%~nxG"
	set "PROJECT_FILE=%%~fG\%%~nxG.csproj"
	if exist "!PROJECT_FILE!" (
		call :log "Found project !PROJECT_NAME!"
		call :ProcessProject "!PROJECT_DIR!" || (
			set "FAILED=1"
			call :log "Failed to package !PROJECT_NAME!."
		)
	) else (
		call :log "Skipping %%~nxG - project file not found: !PROJECT_FILE!"
	)
)

if "%FAILED%"=="1" (
	call :log "One or more projects failed to package."
	echo.
	echo Pack completed with errors.
	pause
	exit /b 1
) else (
	call :log "All projects packaged successfully. Output: %OUTPUT%"
	echo.
	echo Pack completed successfully. Output: %OUTPUT%
	pause
	exit /b 0
)

:: -------------------- Subroutines --------------------
:ProcessProject
:: %1 = project directory (full path)
setlocal EnableDelayedExpansion
set "P_DIR=%~1"
for %%A in ("%~1") do set "P_NAME=%%~nxA"
set "P_FILE=%P_DIR%\%P_NAME%.csproj"
call :log "Starting: %P_NAME%"
pushd "%P_DIR%"

:: Clean
call :log "Cleaning %P_NAME%"
dotnet clean "%P_FILE%" -c %CONFIG% --nologo
if errorlevel 1 (
	call :log "ERROR: clean failed for %P_NAME%"
	popd
	endlocal
	exit /b 1
)

:: Restore
call :log "Restoring %P_NAME%"
dotnet restore "%P_FILE%" --verbosity minimal
if errorlevel 1 (
	call :log "ERROR: restore failed for %P_NAME%"
	popd
	endlocal
	exit /b 1
)

:: Build
call :log "Building %P_NAME%"
dotnet build "%P_FILE%" -c %CONFIG% --no-restore --nologo
if errorlevel 1 (
	call :log "ERROR: build failed for %P_NAME%"
	popd
	endlocal
	exit /b 1
)

:: Pack
call :log "Packing %P_NAME% to %OUTPUT%"
dotnet pack "%P_FILE%" -c %CONFIG% -o "%OUTPUT%" --include-symbols --include-source --no-build --no-restore --nologo
if errorlevel 1 (
	call :log "ERROR: pack failed for %P_NAME%"
	popd
	endlocal
	exit /b 1
)

call :log "Success: %P_NAME%"
popd
endlocal
exit /b 0

:log
:: Writes timestamped message to console. Use: call :log "message"
set "MSG=%~1"
call :timestamp TS
echo [%TS%] %MSG%
exit /b 0

:timestamp
:: Returns current date/time in %1 variable (name passed as first arg)
:: Use Windows environment variables for simplicity
set "%1=%DATE% %TIME%"
exit /b 0