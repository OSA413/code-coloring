dotnet tool install -g dotnet-stryker
cd CodeColoring\CodeColoring_Tests
del /S /Q StrykerOutput
dotnet stryker

for /F "delims==" %%f in ('where /F /R StrykerOutput mutation-report.html') DO %%f