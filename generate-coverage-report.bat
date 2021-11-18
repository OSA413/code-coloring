cd CodeColoring
dotnet list package
dotnet test --collect:"XPlat Code Coverage"
%userprofile%\.nuget\packages\reportgenerator\5.0.0\tools\net5.0\ReportGenerator.exe -reports:CodeColoring_Tests\TestResults\*\*.xml -targetdir:..\CoverageReport
..\CoverageReport\index.html

