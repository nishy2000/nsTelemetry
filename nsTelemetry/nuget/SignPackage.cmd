@ECHO off
REM Copyright 2020, 2023 nishy software

SETLOCAL EnableDelayedExpansion

SET BAT_NAME=%~n0%~x0

SET EXIT_ERR=0
SET CERT_SUBJECT_NAME=nishy software
SET TIME_STAMP_RFC3161_URL=http://timestamp.digicert.com/?alg=sha256

SET NO_PAUSE=
IF "x-%1"=="x-/?" GOTO HELP
IF "x-%1"=="x-/h" GOTO HELP
IF "x-%1"=="x--h" GOTO HELP
IF "x-%1"=="x--help" GOTO HELP
IF "x-%1" == "x---no-pause" SET NO_PAUSE=true& SHIFT
IF "x-%1" == "x--no-pause" SET NO_PAUSE=true& SHIFT
IF "x-%1" == "x-/no-pause" SET NO_PAUSE=true& SHIFT

SET TARGET_FILES=%*

IF "x-" == "x-%TARGET_FILES%" GOTO HELP

REM ------------------------
REM  Sign packages
REM ------------------------
ECHO =======================
ECHO Sign packages
ECHO -----------------------
FOR %%i in (%TARGET_FILES%) do (
    ECHO sign: %%~ni%%~xi
    nuget.exe sign "%%i" -Verbosity quiet -CertificateSubjectName "%CERT_SUBJECT_NAME%" -Timestamper "%TIME_STAMP_RFC3161_URL%"
    IF ERRORLEVEL 1 GOTO ERROR
)
ECHO =======================

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
ECHO Usage: %BAT_NAME%  [--no-pause] nugetPackageFile1 [nugetPackageFile2 [...]]
ECHO   remarks: Each file in NugetPackageFiles ends with .nupkg.
ECHO.

SET EXIT_ERR=

GOTO END

REM ------------------------
REM :END
REM ------------------------
:END

IF NOT "x-%EXIT_ERR%" == "x-" (
    SET ERR=ERRORLEVEL=%EXIT_ERR%
    IF NOT "x-%EXIT_ERR%" == "x-0" SET ERR=[91m!ERR![0m
    ECHO !ERR!
)

ENDLOCAL & EXIT /B %EXIT_ERR%