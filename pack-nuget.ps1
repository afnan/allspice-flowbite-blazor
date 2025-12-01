#!/usr/bin/env pwsh

Write-Host "Creating NuGet packages..." -ForegroundColor Cyan

# Create nuget-local directory if it doesn't exist
if (-not (Test-Path nuget-local)) {
    New-Item -ItemType Directory -Path nuget-local | Out-Null
}

Write-Host "`n1. Packing AllSpice.Flowbite.Blazor..." -ForegroundColor Yellow
dotnet pack src/Flowbite/Flowbite.csproj -c Release -o nuget-local
if ($LASTEXITCODE -ne 0) { 
    Write-Host "Error packing AllSpice.Flowbite.Blazor" -ForegroundColor Red
    exit 1 
}

Write-Host "`n2. Packing AllSpice.Flowbite.Blazor.ExtendedIcons..." -ForegroundColor Yellow
dotnet pack src/Flowbite.ExtendedIcons/Flowbite.ExtendedIcons.csproj -c Release -o nuget-local
if ($LASTEXITCODE -ne 0) { 
    Write-Host "Error packing AllSpice.Flowbite.Blazor.ExtendedIcons" -ForegroundColor Red
    exit 1 
}

Write-Host "`n✓ NuGet packages created in .\nuget-local\" -ForegroundColor Green
Get-ChildItem nuget-local\*.nupkg | ForEach-Object { 
    Write-Host "   - $($_.Name)" -ForegroundColor Gray
}

Write-Host "`nTo publish to NuGet.org, use: .\publish-to-nuget.ps1" -ForegroundColor Cyan
