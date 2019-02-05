del %CD%\unpkgs\*.nupkg
dotnet build -c release
dotnet pack --no-build -o %CD%\unpkgs
set arg1=%1%
for /r . %%a in (unpkgs\*.nupkg) do (
	if %arg1% == local (
		dotnet nuget push "%%a" -s "C:\\Program Files (x86)\\Microsoft SDKs\\NuGetPackages\\"
	) else (
		dotnet nuget push "%%a" -s https://api.nuget.org/v3/index.json -k %1%
	)
)
del %CD%\unpkgs\*.nupkg