param([string] $version)

if(!$version) {
	"Version parameter is required. Exiting..."
	exit 1
}

try {
	Push-Location $PSScriptRoot
	
	dotnet pack -c Release

	dotnet nuget push --source "iti-azure-devops" --api-key az --interactive ./ITI.TypeScriptDtoGenerator/bin/Release/ITI.TypeScriptDtoGenerator.$version.nupkg
} finally {
	Pop-Location
}