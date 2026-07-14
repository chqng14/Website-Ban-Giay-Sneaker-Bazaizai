[CmdletBinding()]
param(
    [switch]$Pull,
    [switch]$NoBuild,
    [ValidateRange(60, 1800)]
    [int]$TimeoutSeconds = 300
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectRoot = $PSScriptRoot
$envPath = Join-Path $projectRoot '.env'
$envExamplePath = Join-Path $projectRoot '.env.example'
$dockerReady = $false

function Write-Step([string]$Message) {
    Write-Host "`n==> $Message" -ForegroundColor Cyan
}

function Read-DotEnv([string]$Path) {
    $values = @{}
    foreach ($line in [System.IO.File]::ReadAllLines($Path)) {
        $trimmed = $line.Trim()
        if ([string]::IsNullOrWhiteSpace($trimmed) -or $trimmed.StartsWith('#')) {
            continue
        }

        $separator = $trimmed.IndexOf('=')
        if ($separator -lt 1) {
            continue
        }

        $name = $trimmed.Substring(0, $separator).Trim()
        $value = $trimmed.Substring($separator + 1).Trim()
        if ($value.Length -ge 2 -and
            (($value.StartsWith('"') -and $value.EndsWith('"')) -or
             ($value.StartsWith("'") -and $value.EndsWith("'")))) {
            $value = $value.Substring(1, $value.Length - 2)
        }
        $values[$name] = $value
    }
    return $values
}

function Get-EnvValue([hashtable]$Values, [string]$Name, [string]$Default = '') {
    if ($Values.ContainsKey($Name)) {
        return [string]$Values[$Name]
    }
    return $Default
}

function Set-DotEnvValue([string]$Path, [string]$Name, [string]$Value) {
    $content = [System.IO.File]::ReadAllText($Path)
    $pattern = "(?m)^$([regex]::Escape($Name))=.*$"
    $replacement = "$Name=$Value"
    if ([regex]::IsMatch($content, $pattern)) {
        $content = [regex]::Replace($content, $pattern, $replacement)
    }
    else {
        $content = $content.TrimEnd() + [Environment]::NewLine + $replacement + [Environment]::NewLine
    }
    [System.IO.File]::WriteAllText(
        $Path,
        $content,
        [System.Text.UTF8Encoding]::new($false))
}

function Invoke-Compose([string[]]$Arguments) {
    & docker compose @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "docker compose failed (exit code $LASTEXITCODE)."
    }
}

function Get-HttpStatus([string]$Uri, [hashtable]$Headers = @{}) {
    try {
        $response = Invoke-WebRequest `
            -Uri $Uri `
            -Headers $Headers `
            -Method Get `
            -TimeoutSec 20 `
            -UseBasicParsing
        return [int]$response.StatusCode
    }
    catch {
        if ($null -ne $_.Exception.Response) {
            return [int]$_.Exception.Response.StatusCode
        }
        throw
    }
}

try {
    Set-Location $projectRoot

    Write-Step 'Checking Docker and Docker Compose'
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw 'Docker is not installed or is not available in PATH.'
    }
    & docker info --format '{{.ServerVersion}}' | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw 'Docker Engine is not running or the current account cannot access it.'
    }
    & docker compose version | Out-Null
    if ($LASTEXITCODE -ne 0) {
        throw 'Docker Compose v2 (docker compose) is required.'
    }
    $dockerReady = $true

    Write-Step 'Checking .env configuration'
    if (-not (Test-Path -LiteralPath $envPath)) {
        if (-not (Test-Path -LiteralPath $envExamplePath)) {
            throw 'Both .env and .env.example are missing.'
        }
        Copy-Item -LiteralPath $envExamplePath -Destination $envPath
        throw 'Created .env from .env.example. Replace all placeholder secrets, then run this script again.'
    }

    $envValues = Read-DotEnv $envPath
    $saPassword = Get-EnvValue $envValues 'SA_PASSWORD'
    $internalApiKey = Get-EnvValue $envValues 'INTERNAL_API_KEY'
    $adminEmail = Get-EnvValue $envValues 'ADMIN_EMAIL'
    $adminUserName = Get-EnvValue $envValues 'ADMIN_USERNAME' 'Admin'
    $adminPassword = Get-EnvValue $envValues 'ADMIN_PASSWORD'
    $ensureAdminPassword = (Get-EnvValue $envValues 'ADMIN_ENSURE_PASSWORD' 'false').ToLowerInvariant()
    $apiPort = Get-EnvValue $envValues 'API_PORT' '80'
    $viewPort = Get-EnvValue $envValues 'VIEW_PORT' '8080'

    if ([string]::IsNullOrWhiteSpace($saPassword) -or $saPassword.StartsWith('ChangeMe_')) {
        throw 'SA_PASSWORD is empty or still uses the placeholder value.'
    }
    if ($saPassword.Length -lt 12 -or
        $saPassword -cnotmatch '[A-Z]' -or
        $saPassword -cnotmatch '[a-z]' -or
        $saPassword -notmatch '\d' -or
        $saPassword -notmatch '[^a-zA-Z0-9]') {
        throw 'SA_PASSWORD must contain at least 12 characters, uppercase, lowercase, a digit, and a symbol.'
    }
    if ([string]::IsNullOrWhiteSpace($internalApiKey) -or
        $internalApiKey -eq 'Generate_A_Long_Random_Value_Here' -or
        $internalApiKey.Length -lt 32) {
        throw 'INTERNAL_API_KEY must be a random secret with at least 32 characters.'
    }
    if (($adminEmail -and -not $adminPassword) -or ($adminPassword -and -not $adminEmail)) {
        throw 'ADMIN_EMAIL and ADMIN_PASSWORD must both be set or both be empty.'
    }
    if ($adminEmail -and [string]::IsNullOrWhiteSpace($adminUserName)) {
        throw 'ADMIN_USERNAME cannot be empty when seeding an Admin account.'
    }
    if ($ensureAdminPassword -notin @('true', 'false')) {
        throw 'ADMIN_ENSURE_PASSWORD must be true or false.'
    }
    foreach ($portItem in @(@('API_PORT', $apiPort), @('VIEW_PORT', $viewPort))) {
        $parsedPort = 0
        if (-not [int]::TryParse($portItem[1], [ref]$parsedPort) -or
            $parsedPort -lt 1 -or $parsedPort -gt 65535) {
            throw "$($portItem[0]) is not a valid port."
        }
    }

    Invoke-Compose @('--env-file', $envPath, 'config', '--quiet')

    if ($Pull) {
        Write-Step 'Pulling the latest base images'
        Invoke-Compose @('--env-file', $envPath, 'pull', '--ignore-buildable')
    }

    Write-Step 'Building and deploying all services'
    $upArguments = @('--env-file', $envPath, 'up', '-d', '--remove-orphans', '--wait', '--wait-timeout', "$TimeoutSeconds")
    if (-not $NoBuild) {
        $upArguments += '--build'
    }
    Invoke-Compose $upArguments

    Write-Step 'Checking website, API, and API protection'
    $viewUrl = "http://localhost:$viewPort/"
    $apiHealthUrl = "http://localhost:$apiPort/api/Health"
    $apiDetailedUrl = "http://localhost:$apiPort/api/Health/detailed"

    if ((Get-HttpStatus $viewUrl) -ne 200) {
        throw "Website did not return HTTP 200: $viewUrl"
    }
    if ((Get-HttpStatus $apiHealthUrl) -ne 200) {
        throw "API health did not return HTTP 200: $apiHealthUrl"
    }
    if ((Get-HttpStatus $apiDetailedUrl) -ne 401) {
        throw 'Protected API did not reject a request without the internal key using HTTP 401.'
    }
    if ((Get-HttpStatus $apiDetailedUrl @{ 'X-Internal-Api-Key' = $internalApiKey }) -ne 200) {
        throw 'Protected API did not accept the configured INTERNAL_API_KEY.'
    }

    if ($adminEmail -and $ensureAdminPassword -eq 'true') {
        Write-Step 'Finalizing one-time Admin seed and disabling automatic password reset'
        Set-DotEnvValue $envPath 'ADMIN_ENSURE_PASSWORD' 'false'
        Invoke-Compose @(
            '--env-file', $envPath,
            'up', '-d', '--no-deps', '--force-recreate',
            '--wait', '--wait-timeout', "$TimeoutSeconds",
            'app-view')
        if ((Get-HttpStatus $viewUrl) -ne 200) {
            throw 'Website is unhealthy after finalizing the Admin seed.'
        }
    }

    Write-Step 'Deployment completed successfully'
    Invoke-Compose @('--env-file', $envPath, 'ps')
    Write-Host "Website: $viewUrl" -ForegroundColor Green
    Write-Host "API health: $apiHealthUrl" -ForegroundColor Green
}
catch {
    Write-Host "`nDEPLOYMENT FAILED: $($_.Exception.Message)" -ForegroundColor Red
    if ($dockerReady -and (Test-Path -LiteralPath $envPath)) {
        Write-Host "`nRecent logs:" -ForegroundColor Yellow
        & docker compose --env-file $envPath logs --tail 80
    }
    exit 1
}
finally {
    Set-Location $projectRoot
}
