#requires -Version 5.1
<#
.SYNOPSIS
Build the .NET backend and npm frontend under src, then open each in its own command prompt.
.EXAMPLE
.\run.ps1
.EXAMPLE
.\run.ps1 -Watch
.EXAMPLE
.\run.ps1 -BuildOnly
.EXAMPLE
.\run.ps1 -Restore
.EXAMPLE
.\run.ps1 -LaunchProfile https
.EXAMPLE
.\run.ps1 -BackendProject src\Api\Api.csproj -FrontendPath src\Frontend
.NOTES
Requires Windows, the project's .NET SDK, and Node.js/npm.
Prefers a Project launch profile with HTTPS unless -LaunchProfile is specified.
Uses the existing frontend start/dev script.
Frontend dependencies are reused unless node_modules is missing or the package
files or local .npmrc changed. -Restore forces installation. An existing install
without a dependency fingerprint is restored once to establish that fingerprint.
Frontend runs with its development server, including after a normal build.
The launcher returns after opening the windows. Stop each service with Ctrl+C
in its window or close that window. Windows remain open if a service exits.
#>
[CmdletBinding()]
param(
    [switch] $Watch,
    [switch] $BuildOnly,
    [switch] $Restore,
    [string] $BackendProject,
    [string] $FrontendPath,
    [string] $FrontendScript,
    [string] $LaunchProfile,
    [ValidateSet('Debug', 'Release')]
    [string] $Configuration = 'Debug'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Invoke-Checked {
    param([string] $Command, [string[]] $Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "Command failed (exit $LASTEXITCODE): $Command $($Arguments -join ' ')"
    }
}

function Resolve-ProjectPath {
    param([string] $Path)
    if (-not [IO.Path]::IsPathRooted($Path)) {
        $Path = Join-Path $PSScriptRoot $Path
    }
    return (Resolve-Path -LiteralPath $Path).Path
}

function Find-SourceFile {
    param([string] $Directory, [string] $Name)
    # Prune dependency and build folders instead of recursively scanning them.
    Get-ChildItem -LiteralPath $Directory -File -Filter $Name
    foreach ($child in Get-ChildItem -LiteralPath $Directory -Directory) {
        if ($child.Name -notin @('node_modules', 'bin', 'obj', 'dist', '.git', '.angular', '.pnpm')) {
            Find-SourceFile -Directory $child.FullName -Name $Name
        }
    }
}

function Quote-PowerShellString {
    param([string] $Value)
    return "'" + $Value.Replace("'", "''") + "'"
}

function Get-FrontendDependencyFingerprint {
    param([string] $Directory)
    # Content hashes also detect dependency changes after switching branches.
    # Include missing files so adding or deleting a lockfile invalidates the install.
    $parts = foreach ($name in @('package.json', 'package-lock.json', 'npm-shrinkwrap.json', '.npmrc')) {
        $path = Join-Path $Directory $name
        if (Test-Path -LiteralPath $path -PathType Leaf) {
            $hash = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash
            "${name}:$hash"
        } else {
            "${name}:missing"
        }
    }
    return ($parts -join '|')
}

function Start-ServiceProcess {
    param([string] $Name, [string] $Directory, [string] $Command, [string[]] $Arguments)
    $quotedArguments = @($Arguments | ForEach-Object { Quote-PowerShellString $_ })
    $worker = @"
`$ErrorActionPreference = 'Stop'
try {
    [Console]::Title = $(Quote-PowerShellString "FileApi - $Name")
    Set-Location -LiteralPath $(Quote-PowerShellString $Directory)
    Write-Host $(Quote-PowerShellString "Starting $Name. Press Ctrl+C in this window to stop it.")
    & $(Quote-PowerShellString $Command) $($quotedArguments -join ' ')
    exit `$LASTEXITCODE
} catch {
    [Console]::Error.WriteLine(`$_.Exception.Message)
    exit 1
}
"@
    $encoded = [Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($worker))
    # Only the shell executable and a Base64 payload enter cmd's command text.
    # Project paths and arguments are quoted inside the encoded PowerShell worker.
    # /K keeps the command prompt open after an exit so startup errors remain visible.
    $cmdArguments = '/D /K ""' + $powerShellExe + '" -NoProfile -EncodedCommand ' + $encoded + '"'
    Start-Process -FilePath (Join-Path $env:SystemRoot 'System32\cmd.exe') `
        -WindowStyle Normal -ArgumentList $cmdArguments -WorkingDirectory $Directory
}

$failed = $false

try {
    if ($env:OS -ne 'Windows_NT') {
        throw 'This launcher requires Windows to open command prompt windows.'
    }
    $dotnet = (Get-Command dotnet -CommandType Application -ErrorAction Stop).Source
    $npm = (Get-Command npm.cmd -CommandType Application -ErrorAction Stop).Source
    $null = Get-Command node -CommandType Application -ErrorAction Stop
    $powerShellExe = Join-Path $PSHOME 'powershell.exe'
    if (-not (Test-Path -LiteralPath $powerShellExe)) {
        $powerShellExe = Join-Path $PSHOME 'pwsh.exe'
    }

    $sourceDirectory = Join-Path $PSScriptRoot 'src'
    if ((-not $BackendProject -or -not $FrontendPath) -and -not (Test-Path -LiteralPath $sourceDirectory -PathType Container)) {
        throw "Source folder does not exist: $sourceDirectory. Specify -BackendProject and -FrontendPath explicitly."
    }
    if (-not $BackendProject) {
        $candidates = @(Find-SourceFile $sourceDirectory '*.csproj' | Where-Object {
            $projectText = Get-Content -LiteralPath $_.FullName -Raw
            $projectText -match 'Microsoft\.NET\.Sdk\.Web'
        })
        if ($candidates.Count -ne 1) {
            $candidateNames = @($candidates | ForEach-Object { $_.FullName }) -join ', '
            throw "Found $($candidates.Count) .NET web projects under src. Specify -BackendProject <path.csproj>. Candidates: $candidateNames"
        }
        $BackendProject = $candidates[0].FullName
    }
    $BackendProject = Resolve-ProjectPath $BackendProject
    if ([IO.Path]::GetExtension($BackendProject) -ne '.csproj') {
        throw '-BackendProject must point to a .csproj file.'
    }
    if (-not $BuildOnly) {
        $launchSettingsPath = Join-Path (Split-Path -Parent $BackendProject) 'Properties\launchSettings.json'
        if (Test-Path -LiteralPath $launchSettingsPath) {
            $launchSettings = Get-Content -LiteralPath $launchSettingsPath -Raw | ConvertFrom-Json
            $profilesProperty = $launchSettings.PSObject.Properties['profiles']
            if ($null -ne $profilesProperty) {
                $projectProfiles = @($profilesProperty.Value.PSObject.Properties | Where-Object {
                    $commandName = $_.Value.PSObject.Properties['commandName']
                    $null -ne $commandName -and $commandName.Value -eq 'Project'
                })
                if (-not $LaunchProfile) {
                    $httpsProfiles = @($projectProfiles | Where-Object {
                        $applicationUrl = $_.Value.PSObject.Properties['applicationUrl']
                        $null -ne $applicationUrl -and $applicationUrl.Value -match '(^|;)\s*https://'
                    })
                    if ($httpsProfiles.Count -gt 0) {
                        $LaunchProfile = $httpsProfiles[0].Name
                    } elseif ($projectProfiles.Count -gt 0) {
                        $LaunchProfile = $projectProfiles[0].Name
                    }
                }
                if ($LaunchProfile) {
                    $selectedProfiles = @($projectProfiles | Where-Object { $_.Name -eq $LaunchProfile })
                    if ($selectedProfiles.Count -ne 1) {
                        throw "Launch profile '$LaunchProfile' must be a Project profile in $launchSettingsPath."
                    }
                    Write-Host "Backend launch profile: $LaunchProfile"
                    $applicationUrl = $selectedProfiles[0].Value.PSObject.Properties['applicationUrl']
                    if ($null -ne $applicationUrl) {
                        Write-Host "Configured backend URLs: $($applicationUrl.Value)"
                    }
                }
            }
        } elseif ($LaunchProfile) {
            throw "Cannot select launch profile '$LaunchProfile': $launchSettingsPath does not exist."
        }
    }

    if (-not $FrontendPath) {
        $candidates = @(Find-SourceFile $sourceDirectory 'package.json' | Where-Object {
            $package = Get-Content -LiteralPath $_.FullName -Raw | ConvertFrom-Json
            $scriptsProperty = $package.PSObject.Properties['scripts']
            $null -ne $scriptsProperty -and $null -ne $scriptsProperty.Value.PSObject.Properties['build'] -and (
                $null -ne $scriptsProperty.Value.PSObject.Properties['start'] -or
                $null -ne $scriptsProperty.Value.PSObject.Properties['dev']
            )
        })
        if ($candidates.Count -ne 1) {
            $candidateNames = @($candidates | ForEach-Object { $_.FullName }) -join ', '
            throw "Found $($candidates.Count) frontend packages under src. Specify -FrontendPath <directory>. Candidates: $candidateNames"
        }
        $FrontendPath = $candidates[0].DirectoryName
    }
    $FrontendPath = Resolve-ProjectPath $FrontendPath
    $packagePath = Join-Path $FrontendPath 'package.json'
    $package = Get-Content -LiteralPath $packagePath -Raw | ConvertFrom-Json
    $scriptsProperty = $package.PSObject.Properties['scripts']
    if ($null -eq $scriptsProperty -or $null -eq $scriptsProperty.Value.PSObject.Properties['build']) {
        throw "Frontend package must define a build script: $packagePath"
    }
    $packageManager = $package.PSObject.Properties['packageManager']
    if ($null -ne $packageManager -and $packageManager.Value -notlike 'npm@*') {
        throw "Frontend uses $($packageManager.Value). This launcher currently supports npm."
    }
    $npmLock = (Test-Path -LiteralPath (Join-Path $FrontendPath 'package-lock.json')) -or
        (Test-Path -LiteralPath (Join-Path $FrontendPath 'npm-shrinkwrap.json'))
    if (-not $npmLock) {
        foreach ($lock in @('pnpm-lock.yaml', 'yarn.lock', 'bun.lock', 'bun.lockb')) {
            if (Test-Path -LiteralPath (Join-Path $FrontendPath $lock)) {
                throw "Found $lock. Use the frontend's package manager; this launcher currently supports npm."
            }
        }
    }
    if (-not $BuildOnly) {
        if (-not $FrontendScript) {
            if ($null -ne $scriptsProperty.Value.PSObject.Properties['start']) {
                $FrontendScript = 'start'
            } elseif ($null -ne $scriptsProperty.Value.PSObject.Properties['dev']) {
                $FrontendScript = 'dev'
            }
        }
        if (-not $FrontendScript -or $null -eq $scriptsProperty.Value.PSObject.Properties[$FrontendScript]) {
            throw 'Specify -FrontendScript with an existing frontend start/dev script.'
        }
    }

    Write-Host "Backend:  $BackendProject"
    Write-Host "Frontend: $FrontendPath"
    Write-Host 'Building backend...'
    Invoke-Checked $dotnet @('build', $BackendProject, '--configuration', $Configuration)
    Push-Location -LiteralPath $FrontendPath
    try {
        $dependencyDirectory = Join-Path $FrontendPath 'node_modules'
        $dependencyStampPath = Join-Path $dependencyDirectory '.fileapi-dependencies.fingerprint'
        $currentFingerprint = Get-FrontendDependencyFingerprint $FrontendPath
        $installedFingerprint = $null
        if (Test-Path -LiteralPath $dependencyStampPath -PathType Leaf) {
            $installedFingerprint = ([string](Get-Content -LiteralPath $dependencyStampPath -Raw)).Trim()
        }
        $needsInstall = $Restore -or
            -not (Test-Path -LiteralPath $dependencyDirectory -PathType Container) -or
            $installedFingerprint -ne $currentFingerprint
        if ($needsInstall) {
            # Invalidate the old stamp before installing, including a forced restore.
            # A failed or interrupted install must be retried on the next run.
            if (Test-Path -LiteralPath $dependencyStampPath -PathType Leaf) {
                Remove-Item -LiteralPath $dependencyStampPath -Force
            }
            if ($npmLock) {
                Write-Host 'Installing frontend dependencies from lockfile...'
                Invoke-Checked $npm @('ci')
            } else {
                Write-Host 'Installing frontend dependencies...'
                Invoke-Checked $npm @('install')
            }
            if (-not (Test-Path -LiteralPath $dependencyDirectory -PathType Container)) {
                $null = New-Item -ItemType Directory -Path $dependencyDirectory
            }
            # npm install can create or update the lockfile; record the final inputs.
            $installedFingerprint = Get-FrontendDependencyFingerprint $FrontendPath
            Set-Content -LiteralPath $dependencyStampPath -Value $installedFingerprint -Encoding ASCII
        } else {
            Write-Host 'Reusing installed frontend dependencies. Use -Restore to force installation.'
        }
        Write-Host 'Building frontend...'
        Invoke-Checked $npm @('run', 'build')
    } finally {
        Pop-Location
    }

    if (-not $BuildOnly) {
        $backendArguments = @('run', '--project', $BackendProject, '--configuration', $Configuration, '--no-build')
        if ($Watch) {
            $backendArguments = @('watch', '--project', $BackendProject, 'run', '--configuration', $Configuration)
        }
        if ($LaunchProfile) {
            $backendArguments += @('--launch-profile', $LaunchProfile)
        }
        Start-ServiceProcess 'backend' (Split-Path -Parent $BackendProject) $dotnet $backendArguments
        Start-ServiceProcess 'frontend' $FrontendPath $npm @('run', $FrontendScript)
        Write-Host 'Opened separate backend and frontend command prompts. Each window shows its own logs.'
        Write-Host 'Stop each service with Ctrl+C in its window, or close that window.'
    } else {
        Write-Host 'Both builds completed successfully.'
    }
} catch {
    $failed = $true
    [Console]::Error.WriteLine($_.Exception.Message)
}
if ($failed) {
    exit 1
}
