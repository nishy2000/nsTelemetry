@ECHO OFF
REM Copyright 2020 nishy software
SETLOCAL enabledelayedexpansion
SET BAT_FOLDER=%~d0%~p0
SET DATE_TIME=%DATE:/=%_%TIME::=%
SET DATE_TIME=%DATE_TIME: =0%
SET SRC_FOLDER=%BAT_FOLDER%..
SET WORK_FOLDERNAME=Package_%DATE_TIME%
SET WORK_FOLDER=%BAT_FOLDER%%WORK_FOLDERNAME%
SET SIGNED_FOLER=%WORK_FOLDER%\signed
SET UNSIGNED_FOLER=%WORK_FOLDER%\unsigned
SET CERT_SUBJECT_NAME=nishy software

COLOR F
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

mkdir "%WORK_FOLDER%"
PUSHD "%SRC_FOLDER%"

REM  Rebuild modules
ECHO =======================
ECHO Rebuild modules
ECHO -----------------------
dotnet.exe build -c Release --no-incremental --no-dependencies nsTelemetryAI.csproj
IF ERRORLEVEL 1 GOTO ERROR


REM  Backup unsigned modules
ECHO =======================
ECHO Backup unsigned modules
ECHO -----------------------

mkdir "%UNSIGNED_FOLER%"

REM  NishySoftware.Telemetry.ApplicationInsights modules
XCOPY /D /E "bin\Release" "%UNSIGNED_FOLER%\" > nul
IF ERRORLEVEL 1 GOTO ERROR


REM  Sign modules
ECHO =======================
ECHO Sign modules
ECHO -----------------------
SET SIGN_FILES=
SET SIGN_FILES=%SIGN_FILES% "bin\Release\net452\nsTelemetryAI.dll"
SET SIGN_FILES=%SIGN_FILES% "bin\Release\netstandard2.0\nsTelemetryAI.dll"

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
FOR %%i in (%SIGN_FILES%) do (
    echo verify: %%i
    "%BAT_FOLDER%signtool.exe" verify  /all /a /tw /pa %%i
    IF ERRORLEVEL 1 (
        COLOR 6F
        ECHO.
        ECHO %%i is not signed.
        GOTO SIGN_START
    )
)
:SIGN_END


REM  Backup signed modules
ECHO =====================
ECHO Backup signed modules
ECHO -----------------------

mkdir "%SIGNED_FOLER%"
mkdir "%SIGNED_FOLER%\net452"
mkdir "%SIGNED_FOLER%\netstandard2.0"

REM  NishySoftware.Telemetry.ApplicationInsights modules
COPY "bin\Release\net452\nsTelemetryAI.dll" "%SIGNED_FOLER%\net452\" >nul
IF ERRORLEVEL 1 GOTO ERROR
COPY "bin\Release\netstandard2.0\nsTelemetryAI.dll" "%SIGNED_FOLER%\netstandard2.0\" >nul
IF ERRORLEVEL 1 GOTO ERROR

REM  Copy signed resource files to obj folder
ECHO =======================
ECHO Copy signed resource files to obj folder
ECHO -----------------------

REM  NishySoftware.Telemetry.ApplicationInsights modules
SET COPY_SOURCE=bin\Release\net452
SET COPY_DEST=obj\Release\net452
COPY "%COPY_SOURCE%\nsTelemetryAI.dll" "%COPY_DEST%\" >nul
IF ERRORLEVEL 1 GOTO ERROR

SET COPY_SOURCE=bin\Release\netstandard2.0
SET COPY_DEST=obj\Release\netstandard2.0
COPY "%COPY_SOURCE%\nsTelemetryAI.dll" "%COPY_DEST%\" >nul
IF ERRORLEVEL 1 GOTO ERROR


REM  Packaging
ECHO =======================
ECHO Packaging
ECHO -----------------------

dotnet.exe pack -c Release --no-build -o "%WORK_FOLDER%" nsTelemetryAI.csproj
IF ERRORLEVEL 1 GOTO ERROR


REM  Backup packages
ECHO =======================
ECHO Backup packages
ECHO -----------------------

FOR %%i in (%WORK_FOLDER%\*.nupkg) do (
    COPY "%%i" "%SIGNED_FOLER%" > nul
    MOVE "%%i" "%UNSIGNED_FOLER%\%%~ni_unsigned%%~xi" > nul
)

REM  Sign packages
ECHO =======================
ECHO Sign packages
ECHO -----------------------

FOR %%i in ("%WORK_FOLDER%\signed\*.nupkg") do (
    echo sign: %%~ni%%~xi
    "%BAT_FOLDER%nuget.exe" sign "%%i" -Verbosity quiet -CertificateSubjectName "%CERT_SUBJECT_NAME%"  -Timestamper "http://sha256timestamp.ws.symantec.com/sha256/timestamp" 
    IF ERRORLEVEL 1 GOTO ERROR
)

GOTO END

:ERROR
ECHO ERROR occurred
COLOR C1
PAUSE
COLOR F

:END
ECHO =======================
ECHO Output folder: "%WORK_FOLDERNAME%"

POPD
ENDLOCAL
EXIT /B