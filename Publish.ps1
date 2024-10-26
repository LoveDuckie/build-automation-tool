#!/usr/bin/env pwsh
param (
    [string]$ProjectPath = ".",                   # Path to the .NET project or solution
    [string]$NuGetServerUrl = "http://localhost:5555/v3/index.json",  # URL of the locally hosted NuGet server
    [string]$OutputDirectory = "bin/Release",     # Output directory for the package (relative to project path)
    [string]$ApiKey = "your-api-key-here"         # API key for your local NuGet server (if required)
)

# Convert the path specified to absolute
function Convert-ToAbsolutePath {
    param (
        [string]$PathToCheck
    )

    # Check if the path is already absolute
    if ([System.IO.Path]::IsPathRooted($PathToCheck)) {
        # If it's already absolute, return it as is
        return $PathToCheck
    } else {
        # If it's relative, resolve it to an absolute path
        $absolutePath = Resolve-Path $PathToCheck
        return $absolutePath
    }
}

# Function to check if .NET CLI is installed
function Check-Dotnet {
    $dotnetInstalled = (Get-Command dotnet -ErrorAction SilentlyContinue)
    if (-not $dotnetInstalled) {
        Write-Host ".NET CLI is not installed. Please install .NET SDK to proceed." -ForegroundColor Red
        exit 1
    } else {
        Write-Host ".NET CLI is installed." -ForegroundColor Green
    }
}

# Function to build the project using dotnet
function Build-Project {
    param (
        [string]$ProjectPath,
        [string]$OutputDirectory
    )

    Write-Host "Building project at: $ProjectPath" -ForegroundColor Yellow
    dotnet build $ProjectPath --configuration Release

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Project built successfully." -ForegroundColor Green
    } else {
        Write-Host "Build failed." -ForegroundColor Red
        exit 1
    }
}

# Function to pack the project into a NuGet package
function Pack-Project {
    param (
        [string]$ProjectPath,
        [string]$OutputDirectory
    )

    Write-Host "Packing project into a NuGet package..." -ForegroundColor Yellow
    dotnet pack $ProjectPath --configuration Release --output $OutputDirectory

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Project packaged successfully." -ForegroundColor Green
    } else {
        Write-Host "Packaging failed." -ForegroundColor Red
        exit 1
    }
}

# Function to push the NuGet package to the server
function Push-NuGetPackage {
    param (
        [string]$PackagePath,
        [string]$NuGetServerUrl,
        [string]$ApiKey
    )

    Write-Host "Pushing package to NuGet server: $NuGetServerUrl" -ForegroundColor Yellow
    dotnet nuget push $PackagePath --api-key $ApiKey --source $NuGetServerUrl

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Package pushed to NuGet server successfully." -ForegroundColor Green
    } else {
        Write-Host "Failed to push package." -ForegroundColor Red
        exit 1
    }
}

# Main Script

Write-Host "Starting the publish process..." -ForegroundColor Cyan

# Ensure .NET CLI is installed
Check-Dotnet

# Convert project path to an absolute path
$absoluteProjectPath = Resolve-Path $ProjectPath
Write-Host "Resolved project path: $absoluteProjectPath" -ForegroundColor Cyan

# Build the project
Build-Project -ProjectPath $absoluteProjectPath -OutputDirectory $OutputDirectory

# Pack the project
Pack-Project -ProjectPath $absoluteProjectPath -OutputDirectory $OutputDirectory

# Find the NuGet package in the output directory
$package = Get-ChildItem -Path $OutputDirectory -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1
if (-not $package) {
    Write-Host "No NuGet package found in the output directory." -ForegroundColor Red
    exit 1
}

$packagePath = $package.FullName
Write-Host "Found package: $packagePath" -ForegroundColor Green

# Push the package to the NuGet server
Push-NuGetPackage -PackagePath $packagePath -NuGetServerUrl $NuGetServerUrl -ApiKey $ApiKey

Write-Host "Publish process completed." -ForegroundColor Cyan