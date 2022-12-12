@echo off
chcp 65001
setlocal EnableDelayedExpansion
set app=bin\Mao_Good_Transite_x64d.dll
$(ADDITIONAL_PATH)
start "" dotnet "%app%" 
