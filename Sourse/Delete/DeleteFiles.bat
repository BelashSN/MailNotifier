for %%i in (..\..\Release\*) do call :b %%i
goto :fin
:b
for /F "" %%k in (List.txt) do if %~nx1==%%k goto :fin
del %~f1
:fin