@echo off

:: Configuración de rutas del compilador y SDK
set GHS_ROOT=C:/ghs/multi5327
set CAFE_ROOT=C:/Nintendo/WiiUSDK/SDK/cafe_sdk

:: Verificación de existencia de herramientas
if not exist "%GHS_ROOT%/cxppc.exe" (
    echo Error: No se puede encontrar cxppc.exe en %GHS_ROOT%.
    exit /b 1
)

if not exist "%CAFE_ROOT%/system/bin/tool/preprpl64.exe" (
    echo Error: No se puede encontrar preprpl64.exe en %CAFE_ROOT%/system/bin/tool.
    exit /b 1
)

if not exist "%CAFE_ROOT%/system/bin/tool/makerpl64.exe" (
    echo Error: No se puede encontrar makerpl64.exe en %CAFE_ROOT%/system/bin/tool.
    exit /b 1
)

:: Configuración de variables de compilación
set B=build
set CC=%GHS_ROOT%/cxppc.exe
set LINK=%GHS_ROOT%/cxppc.exe
set PREPRPL=%CAFE_ROOT%/system/bin/tool/preprpl64.exe
set MAKERPL=%CAFE_ROOT%/system/bin/tool/makerpl64.exe
set CCFLAGS=-Ogeneral -c -cpu=espresso -c99 --g++ -kanji=shiftjis -sda=none -X332 --no_implicit_include -G -dbg_source_root "." --no_commons -w --no_exceptions
set CCINCLUDES=%CAFE_ROOT%/system/include
set CCDEFINES=-DNDEV=1 -DCAFE=2 -DPLATFORM=CAFE -DEPPC -DWEBPLUG=0 -DFT2_BUILD_LIBRARY -DUNITTEST_FORCE_NO_EXCEPTIONS -DUNITY_RELEASE=1 -DMASTER_BUILD=1 -DENABLE_PROFILER=0

:: Configuración de nombres de salida y archivos objeto
set OUTNAME=SDCardPlugin
set OUTNAME_EXPORT=%OUTNAME%.export.o
set OBJS=%B%/sdcard.o

:: Cambio de directorio al del script
set R=%~dp0
pushd %R%

@echo on
:: Crear directorio de compilación
if not exist build mkdir build

:: Compilación del archivo sdcard.cpp
%CC% %CCFLAGS% -I %CCINCLUDES% %CCDEFINES% -o %B%/sdcard.o sdcard.cpp

:: Preparación y enlace del archivo objeto exportado
%PREPRPL% -xall -log %B%/%OUTNAME%.log -e __rpl_crt -o %B%/%OUTNAME_EXPORT% %B%/sdcard.o

:: Configuración de librerías
set LIBS=-lnn_os.a

:: Enlace del archivo ejecutable final (.elf)
%CC% %B%/%OUTNAME_EXPORT% %OBJS% -e __rpl_crt -relprog_cafe %CAFE_ROOT%/system/include/cafe/eppc.Cafe.rpl.ld -map -sda=none -nostartfile -L %CAFE_ROOT%/system/lib/ghs/cafe/NDEBUG -lrpl.a -lcoredyn.a %LIBS% -o %B%/%OUTNAME%.elf
%MAKERPL% -t BUILD_TYPE=NDEBUG -zx -checknosda -nolib %B%/%OUTNAME%.elf

:: Regresar al directorio original
popd
