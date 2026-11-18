param(
    [string]$ProjectPath = (Join-Path $PSScriptRoot '../../AshClicker.csproj')
)

$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent (Resolve-Path -LiteralPath $ProjectPath).Path
$numberPattern = '\A[1-9][0-9]*\z'
$corePattern = '(?:0|[1-9][0-9]*)\.(?:0|[1-9][0-9]*)\.(?:0|[1-9][0-9]*)'

if ($env:GITHUB_RUN_ID -notmatch $numberPattern -or
    $env:GITHUB_RUN_NUMBER -notmatch $numberPattern -or
    $env:GITHUB_RUN_ATTEMPT -notmatch $numberPattern) {
    throw 'GitHub Actions must supply positive run ID, run number, and run attempt values.'
}

$commit = git -C $repo rev-parse HEAD
if ($LASTEXITCODE -ne 0 -or $commit -cnotmatch '\A[0-9a-f]{40}(?:[0-9a-f]{24})?\z') {
    throw 'Unable to read the checked-out Git commit.'
}

$suffix = ''
if ($env:GITHUB_EVENT_NAME -eq 'push' -and $env:GITHUB_REF_TYPE -eq 'tag') {
    $tagPattern = "\Av(?<core>${corePattern})(?<suffix>-[A-Za-z0-9-]+(?:\.[A-Za-z0-9-]+)*)?\z"
    if ($env:GITHUB_REF_NAME -cnotmatch $tagPattern) {
        throw "Expected a SemVer tag such as v1.2.0 or v1.2.0-rc.1, got $env:GITHUB_REF_NAME."
    }
    $core = $Matches.core
    $suffix = [string]$Matches['suffix']
    if ($suffix -and ($suffix.Substring(1).Split('.') -match '\A0[0-9]+\z')) {
        throw 'Numeric SemVer prerelease identifiers cannot contain leading zeroes.'
    }
} else {
    $project = [xml](Get-Content -LiteralPath $ProjectPath -Raw)
    $prefix = $project.SelectSingleNode('/Project/PropertyGroup/VersionPrefix').InnerText
    if ($prefix -cnotmatch "\A$corePattern\z") {
        throw 'VersionPrefix must contain three SemVer numbers.'
    }
    $baseVersion = [version]$prefix
    $line = "$($baseVersion.Major).$($baseVersion.Minor)"
    $tags = git -C $repo tag --merged HEAD --list "v$line.*"
    if ($LASTEXITCODE -ne 0) {
        throw "Unable to read stable tags for $line."
    }
    $nextPatch = $baseVersion.Build
    foreach ($tag in $tags) {
        $patch = 0
        if ($tag -cmatch "\Av$([regex]::Escape($line))\.(0|[1-9][0-9]*)\z" -and
            [int]::TryParse($Matches[1], [ref]$patch) -and $patch -lt [uint16]::MaxValue) {
            $nextPatch = [Math]::Max($nextPatch, $patch + 1)
        }
    }
    $core = "$line.$nextPatch"
    $suffix = "-ci.$env:GITHUB_RUN_NUMBER.$env:GITHUB_RUN_ATTEMPT"
}

$numericVersion = $null
if (![version]::TryParse($core, [ref]$numericVersion) -or
    $numericVersion.Major -ge [uint16]::MaxValue -or
    $numericVersion.Minor -ge [uint16]::MaxValue -or
    $numericVersion.Build -ge [uint16]::MaxValue) {
    throw "Version $core exceeds the supported Windows version component range (0 through 65534)."
}

$version = "$core$suffix"
$infoVersion = "$version+run.${env:GITHUB_RUN_ID}.attempt.${env:GITHUB_RUN_ATTEMPT}.sha.$($commit.Substring(0, 12))"
$outputs = @(
    "version=$version"
    "assembly_version=$($numericVersion.Major).$($numericVersion.Minor).0.0"
    "file_version=$core.0"
    "informational_version=$infoVersion"
    "prerelease=$([bool]$suffix)".ToLowerInvariant()
    "artifact=AshClicker-v$version-attempt.$env:GITHUB_RUN_ATTEMPT"
)
$outputs | Add-Content -LiteralPath $env:GITHUB_OUTPUT -Encoding utf8
Write-Host "Building v$infoVersion"
