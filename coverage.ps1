Remove-Item -Recurse -Force EersteWebNetApplicatie.Tests\TestResults, coveragereport
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:coveragereport -reporttypes:Html -classfilters:"-EersteWebNetApplicatie.Migrations.*;-Microsoft.*;-System.*"