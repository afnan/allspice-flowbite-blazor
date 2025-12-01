Write-Host "Creating NuGet packages..."
Write-Host ""

# Create nuget-local directory if it doesn't exist
$outputDir = "nuget-local"
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
    Write-Host "Created $outputDir directory"
}

Write-Host "Packing AllSpice.Flowbite.Blazor..."
dotnet pack src/Flowbite/Flowbite.csproj -c Release -o $outputDir
if ($LASTEXITCODE -ne 0) { 
    Write-Host "Error: Failed to pack Flowbite"
    exit 1 
}

Write-Host "Packing AllSpice.Flowbite.Blazor.ExtendedIcons..."
dotnet pack src/Flowbite.ExtendedIcons/Flowbite.ExtendedIcons.csproj -c Release -o $outputDir
if ($LASTEXITCODE -ne 0) { 
    Write-Host "Error: Failed to pack ExtendedIcons"
    exit 1 
}

Write-Host ""
Write-Host "Verifying packages..."

# Verify directory exists
if (-not (Test-Path $outputDir)) {
    Write-Host "ERROR: $outputDir directory was not created!"
    exit 1
}

# Check for packages
$packages = Get-ChildItem "$outputDir\*.nupkg" -ErrorAction SilentlyContinue
if ($packages) {
    Write-Host "Packages created in $outputDir folder:"
    $packages | ForEach-Object { 
        $sizeKB = [math]::Round($_.Length / 1KB, 2)
        Write-Host "  $($_.Name) ($sizeKB KB)" 
    }
    Write-Host ""
    Write-Host "Full path: $(Resolve-Path $outputDir)"
} else {
    Write-Host "WARNING: No .nupkg files found in $outputDir!"
    Write-Host "Directory contents:"
    Get-ChildItem $outputDir -ErrorAction SilentlyContinue | ForEach-Object {
        Write-Host "  $($_.Name)"
    }
}

Write-Host ""
Write-Host "To publish to NuGet.org, run: .\publish-to-nuget.ps1"
Write-Host ""
