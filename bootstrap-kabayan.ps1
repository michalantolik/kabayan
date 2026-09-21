[CmdletBinding()]
param(
    [string]$ProjectName = "Kabayan",
    [string]$Destination = (Get-Location).Path,
    [switch]$Force
)

$ErrorActionPreference = "Stop"

Write-Host "Kabayan repository bootstrap" -ForegroundColor Cyan
Write-Host "Project: $ProjectName"

$SourceRoot = $PSScriptRoot
$DestinationRoot = [System.IO.Path]::GetFullPath($Destination)
$Files = @(
    ".github/workflows/deploy-pages.yml",
    "AGENTS.md",
    "README.md",
    "bootstrap-kabayan.ps1",
    "docs/vision.md",
    "docs/discovery.md",
    "docs/survey.md",
    "docs/design.md",
    "docs/writing.md",
    "docs/brand-and-market-strategy.md",
    "docs/decisions.md",
    "docs/knowledge-and-evidence.md",
    "docs/roadmap.md",
    "index.html",
    "script.js",
    "styles.css"
)

$Missing = $Files | Where-Object {
    -not (Test-Path -LiteralPath (Join-Path $SourceRoot $_) -PathType Leaf)
}
if ($Missing.Count -gt 0) {
    Write-Error ("Bootstrap source is missing required files: " + ($Missing -join ", "))
}

foreach ($RelativePath in $Files) {
    $SourcePath = Join-Path $SourceRoot $RelativePath
    $DestinationPath = Join-Path $DestinationRoot $RelativePath

    if ([System.IO.Path]::GetFullPath($SourcePath) -eq [System.IO.Path]::GetFullPath($DestinationPath)) {
        Write-Host "Already present: $DestinationPath" -ForegroundColor DarkGray
        continue
    }

    if ((Test-Path -LiteralPath $DestinationPath) -and -not $Force) {
        Write-Host "Skipped existing file: $DestinationPath" -ForegroundColor Yellow
        continue
    }

    $Parent = Split-Path -Parent $DestinationPath
    if (-not (Test-Path -LiteralPath $Parent)) {
        New-Item -ItemType Directory -Force -Path $Parent | Out-Null
    }

    Copy-Item -LiteralPath $SourcePath -Destination $DestinationPath -Force
    Write-Host "Created: $DestinationPath" -ForegroundColor Green
}

Write-Host "Kabayan repository bootstrap complete." -ForegroundColor Cyan
