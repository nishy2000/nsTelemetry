@ECHO OFF
REM Copyright 2020, 2021, 2023 nishy software

SETLOCAL EnableDelayedExpansion

SET BAT_FOLDER=%~d0%~p0

SET EXIT_ERR=0
SET DATE_TIME=%DATE:/=%_%TIME::=%
SET DATE_TIME=%DATE_TIME: =0%
SET SRC_FOLDER=%BAT_FOLDER%..
SET WORK_FOLDERNAME=Package_%DATE_TIME%
SET WORK_FOLDER=%BAT_FOLDER%%WORK_FOLDERNAME%
SET SIGNED_FOLER=%WORK_FOLDER%\signed
SET UNSIGNED_FOLER=%WORK_FOLDER%\unsigned

SET CERT_SUBJECT_NAME=nishy software
SET TIME_STAMP_RFC3161_URL=http://timestamp.digicert.com/?alg=sha256

SET NO_BUILD=
SET NO_PAUSE=
IF "x-%1"=="x-/?" GOTO HELP
IF "x-%1"=="x-/h" GOTO HELP
IF "x-%1"=="x--h" GOTO HELP
IF "x-%1"=="x--help" GOTO HELP
IF "x-%1" == "x---no-build" SET NO_BUILD=true& SHIFT
IF "x-%1" == "x--no-build" SET NO_BUILD=true& SHIFT
IF "x-%1" == "x-/no-build" SET NO_BUILD=true& SHIFT
IF "x-%1" == "x---no-pause" SET NO_PAUSE=true& SHIFT
IF "x-%1" == "x--no-pause" SET NO_PAUSE=true& SHIFT
IF "x-%1" == "x-/no-pause" SET NO_PAUSE=true& SHIFT
IF "x-%1" == "x---no-build" SET NO_BUILD=true& SHIFT
IF "x-%1" == "x--no-build" SET NO_BUILD=true& SHIFT
IF "x-%1" == "x-/no-build" SET NO_BUILD=true& SHIFT

SET TARGET_CONFIG=Release
IF NOT "x-%1" == "x-" SET TARGET_CONFIG=%1

SET BIN_FOLDER=bin\%TARGET_CONFIG%
SET OBJ_FOLDER=obj\%TARGET_CONFIG%
SET RUNTIME_ID_FRM=net452
SET RUNTIME_ID_NET=netstandard2.0

PUSHD "%SRC_FOLDER%"

MKDIR "%WORK_FOLDER%"
IF ERRORLEVEL 1 GOTO ERROR

IF NOT "x-%SIGNTOOL%" == "x-" SET SIGNTOOL=%SIGNTOOL:"=%
ECHO signtool: %SIGNTOOL%
IF NOT "x-%SIGNTOOL%" == "x-" (
    IF NOT EXIST "%SIGNTOOL%" (
        ECHO [91m"%SIGNTOOL%" does not exist.[0m
        PAUSE
        GOTO END
    )
) ELSE (
    ECHO [93mIn order to sign automatically, Please SET SIGNTOOL=sign batch file path[0m
)

REM ------------------------
REM  Rebuild modules
REM ------------------------
ECHO =======================
ECHO Rebuild modules
ECHO -----------------------
IF "x-%NO_BUILD%" == "x-" (
	dotnet.exe build -c %TARGET_CONFIG% --no-incremental --no-dependencies nsTelemetryAI.csproj
	IF ERRORLEVEL 1 GOTO ERROR
) ELSE (
	ECHO [93mSkip[0m: rebuild modules
)
:REBUILD_END


REM ------------------------
REM  Backup unsigned modules
REM ------------------------
ECHO =======================
ECHO Backup unsigned modules
ECHO -----------------------

MKDIR "%UNSIGNED_FOLER%"

REM  NishySoftware.Telemetry.ApplicationInsights modules
XCOPY /D /E "%BIN_FOLDER%" "%UNSIGNED_FOLER%\" > nul
IF ERRORLEVEL 1 GOTO ERROR

REM ------------------------
REM  Sign modules
REM ------------------------
ECHO =======================
ECHO Sign modules
ECHO -----------------------
SET SIGN_FILES=
SET SIGN_FILES=%SIGN_FILES% "%BIN_FOLDER%\%RUNTIME_ID_FRM%\nsTelemetryAI.dll"
SET SIGN_FILES=%SIGN_FILES% "%BIN_FOLDER%\%RUNTIME_ID_NET%\nsTelemetryAI.dll"

REM Not signing when including Debug in %TARGET_CONFIG%
IF NOT "x-%TARGET_CONFIG%" == "x-%TARGET_CONFIG:Debug=%" (
	ECHO [93mSkip[0m: sign module files because configuration is '%TARGET_CONFIG%'
	GOTO SIGN_END
)

FOR %%i in (%SIGN_FILES%) do (
    "%BAT_FOLDER%signtool.exe" verify  /all /a /tw /pa %%i
    IF ERRORLEVEL 1 (
        GOTO SIGN_START
    )
)
ECHO [93mSkip[0m: All files are already signed.
GOTO VERIFY_START

:SIGN_START
IF EXIST "%SIGNTOOL%" (
    SETLOCAL
    CALL "%SIGNTOOL%" %SIGN_FILES%
    ENDLOCAL
) ELSE (
    ECHO Please sign files [ %SIGN_FILES% ]
    ECHO Hit any key after siging files
    PAUSE
    COLOR
)

:VERIFY_START
FOR %%i in (%SIGN_FILES%) do (
    ECHO verify: %%i
    "%BAT_FOLDER%signtool.exe" verify  /all /a /tw /pa %%i
    IF ERRORLEVEL 1 (
        COLOR 6F
        ECHO.
        ECHO %%i is not signed.
        GOTO SIGN_START
    )
)
:SIGN_END


REM ------------------------
REM  Backup signed modules
REM ------------------------
ECHO =====================
ECHO Backup signed modules
ECHO -----------------------

MKDIR "%SIGNED_FOLER%"
IF ERRORLEVEL 1 GOTO ERROR
MKDIR "%SIGNED_FOLER%\%RUNTIME_ID_FRM%"
IF ERRORLEVEL 1 GOTO ERROR
MKDIR "%SIGNED_FOLER%\%RUNTIME_ID_NET%"
IF ERRORLEVEL 1 GOTO ERROR

REM  NishySoftware.Telemetry.ApplicationInsights modules
COPY "%BIN_FOLDER%\%RUNTIME_ID_FRM%\nsTelemetryAI.dll" "%SIGNED_FOLER%\%RUNTIME_ID_FRM%\" >nul
IF ERRORLEVEL 1 GOTO ERROR
COPY "%BIN_FOLDER%\%RUNTIME_ID_NET%\nsTelemetryAI.dll" "%SIGNED_FOLER%\%RUNTIME_ID_NET%\" >nul
IF ERRORLEVEL 1 GOTO ERROR

REM ------------------------
REM  Copy signed resource files to obj folder
REM ------------------------
ECHO =======================
ECHO Copy signed resource files to obj folder
ECHO -----------------------

REM  NishySoftware.Telemetry.ApplicationInsights modules
SET COPY_SOURCE=%BIN_FOLDER%\%RUNTIME_ID_FRM%
SET COPY_DEST=%OBJ_FOLDER%\%RUNTIME_ID_FRM%
COPY "%COPY_SOURCE%\nsTelemetryAI.dll" "%COPY_DEST%\" >nul
IF ERRORLEVEL 1 GOTO ERROR

SET COPY_SOURCE=%BIN_FOLDER%\%RUNTIME_ID_NET%
SET COPY_DEST=%OBJ_FOLDER%\%RUNTIME_ID_NET%
COPY "%COPY_SOURCE%\nsTelemetryAI.dll" "%COPY_DEST%\" >nul
IF ERRORLEVEL 1 GOTO ERROR


REM ------------------------
REM  Packaging
REM ------------------------
ECHO =======================
ECHO Packaging
ECHO -----------------------

dotnet.exe pack -c %TARGET_CONFIG% --no-build -o "%WORK_FOLDER%" nsTelemetryAI.csproj
IF ERRORLEVEL 1 GOTO ERROR


REM ------------------------
REM  Backup packages
REM ------------------------
ECHO =======================
ECHO Backup packages
ECHO -----------------------

FOR %%i in (%WORK_FOLDER%\*.nupkg) do (
    COPY "%%i" "%SIGNED_FOLER%" > nul
    MOVE "%%i" "%UNSIGNED_FOLER%\%%~ni_unsigned%%~xi" > nul
)

REM ------------------------
REM  Sign packages
REM ------------------------
ECHO =======================
ECHO Sign packages
ECHO -----------------------

REM Not signing when including Debug in %TARGET_CONFIG%
IF NOT "x-%TARGET_CONFIG%" == "x-%TARGET_CONFIG:Debug=%" (
	ECHO [93mSkip[0m: sign package files because configuration is '%TARGET_CONFIG%'
	GOTO SIGN_PACKAGE_END
)

FOR %%i in ("%WORK_FOLDER%\signed\*.nupkg") do (
    ECHO sign: %%~ni%%~xi
    "%BAT_FOLDER%nuget.exe" sign "%%i" -Verbosity quiet -CertificateSubjectName "%CERT_SUBJECT_NAME%"  -Timestamper "%TIME_STAMP_RFC3161_URL%" 
    IF ERRORLEVEL 1 GOTO ERROR
)
:SIGN_PACKAGE_END

GOTO END

REM ------------------------
REM :ERROR
REM ------------------------
:ERROR

SET EXIT_ERR=%ERRORLEVEL%
ECHO [41m[1mERROR occurred (ERRORLEVEL=%EXIT_ERR%)[0m
IF "x-%NO_PAUSE%" == "x-" (
REM COLOR C1
ECHO.[41m&PAUSE&ECHO [0m
REM COLOR F
)

GOTO END

REM ------------------------
REM :HELP
REM ------------------------
:HELP

ECHO.
ECHO Usage: %BAT_NAME%  [--no-build] [--no-pause] [ConfigName]
ECHO   remarks: Default value of ConfigName is 'Release'
ECHO.

SET EXIT_ERR=

GOTO END

REM ------------------------
REM :END
REM ------------------------
:END

POPD

IF NOT "x-%EXIT_ERR%" == "x-" (
    ECHO =======================
    ECHO Output folder: "%WORK_FOLDERNAME%"

    SET ERR=ERRORLEVEL=%EXIT_ERR%
    IF NOT "x-%EXIT_ERR%" == "x-0" SET ERR=[91m!ERR![0m
    ECHO !ERR!

    ECHO =======================
)

ENDLOCAL & EXIT /B %EXIT_ERR%