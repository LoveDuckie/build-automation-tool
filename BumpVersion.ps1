#!/usr/bin/env pwsh
param (
    [string]$ProjectPath = ".\Tool\BuildAutomationTool\BuildAutomationTool",        # Default to current directory
    [switch]$UploadPackage = $false,    # Optional parameter to upload package
    [ValidateSet("major", "minor", "patch")]
    [string]$VersionType = "patch"
)

# Convert the path specified to an absolute path
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

# Function to read the current version from the .csproj file
function Get-CurrentVersion {
    param (
        [string]$csprojPath
    )

    $csprojContent = Get-Content $csprojPath
    $versionLine = $csprojContent | Select-String -Pattern '<Version>'
    
    if ($versionLine) {
        $currentVersion = [regex]::Match($versionLine, '\d+\.\d+\.\d+').Value
        return $currentVersion
    }
    else {
        Write-Host "Version not found in the .csproj file." -ForegroundColor Red
        exit 1
    }
}

# Function to bump the patch version (semantic versioning: Major.Minor.Patch)
function Bump-PatchVersion {
    param (
        [string]$CurrentVersion,
        [ValidateSet("major", "minor", "patch")]
        [string]$VersionType = "patch"
    )

    $versionParts = $CurrentVersion -split '\.'
    $major = [int]$versionParts[0]
    $minor = [int]$versionParts[1]
    $patch = [int]$versionParts[2]

    Write-Host "Updating version type ""$VersionType""." -ForegroundColor Yellow

    switch ($VersionType) {
        "major" {
            $major = [int]$versionParts[0] + 1
        }
        "minor" {
            $minor = [int]$versionParts[1] + 1
        }
        "patch" {
            $patch = [int]$versionParts[2] + 1
        }
        default {
            # Code to execute if no case matches
        }
    }

    return "$major.$minor.$patch"
}

# Function to update the version in the .csproj file
function Update-VersionInCsproj {
    param (
        [string]$csprojPath,
        [string]$newVersion
    )

    $csprojContent = Get-Content $csprojPath
    $updatedContent = $csprojContent -replace '<Version>\d+\.\d+\.\d+</Version>', "<Version>$newVersion</Version>"

    # Write the updated content back to the .csproj file
    Set-Content $csprojPath $updatedContent
    Write-Host "Updated version in $csprojPath to $newVersion" -ForegroundColor Green
}

# Function to commit version changes to git
function Commit-VersionChange {
    param (
        [string]$newVersion
    )

    git add .
    git commit -m "Bump version to $newVersion"
    git push
    Write-Host "Committed version change to git: $newVersion" -ForegroundColor Green
}

$ProjectPath = Convert-ToAbsolutePath -PathToCheck $ProjectPath

$csprojPath = "$ProjectPath\BuildAutomationTool.csproj"

Write-Host "Starting version bump process..." -ForegroundColor Cyan

$currentVersion = Get-CurrentVersion -csprojPath $csprojPath
Write-Host "Current version: $currentVersion" -ForegroundColor Yellow

$newVersion = Bump-PatchVersion -VersionType $VersionType -CurrentVersion $currentVersion
Write-Host "New version: $newVersion" -ForegroundColor Yellow

Update-VersionInCsproj -csprojPath $csprojPath -newVersion $newVersion

# Commit-VersionChange -newVersion $newVersion

Write-Host "Version bump process completed." -ForegroundColor Cyan
