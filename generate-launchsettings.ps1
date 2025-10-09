$repoRoot = $PSScriptRoot

$envFile = Join-Path $repoRoot ".env"

if (-not (Test-Path $envFile)) {
    Write-Host "⚠️  Файл .env не найден в корне репозитория. Пропускаем генерацию launchSettings.json."
    exit 0
}

Write-Host "📥 Загружаем переменные из .env..."
$envVars = @{}
Get-Content $envFile | ForEach-Object {
    $line = $_.Trim()
    if ($line -and -not $line.StartsWith("#") -and $line.Contains("=")) {
        $parts = $line -split "=", 2
        $key = $parts[0].Trim()
        $value = $parts[1].Trim().Trim('"').Trim("'")
        $envVars[$key] = $value
        [System.Environment]::SetEnvironmentVariable($key, $value)
    }
}

Write-Host "🔍 Ищем *.Server проекты..."
$serverProjects = Get-ChildItem -Path $repoRoot -Directory -Recurse | Where-Object {
    $_.Name -like "*.Server"
}

if ($serverProjects.Count -eq 0) {
    Write-Host "⚠️  Не найдено ни одного проекта с суффиксом .Server"
    exit 0
}

foreach ($project in $serverProjects) {
    $propertiesDir = Join-Path $project.FullName "Properties"
    $templatePath = Join-Path $propertiesDir "launchSettings.Template.json"
    $outputPath = Join-Path $propertiesDir "launchSettings.json"

    if (-not (Test-Path $templatePath)) {
        Write-Host "⏭️  Шаблон не найден: $templatePath — пропускаем"
        continue
    }

    Write-Host "⚙️  Обрабатываем: $($project.Name)"

    $content = Get-Content $templatePath -Raw

    foreach ($key in $envVars.Keys) {
        $placeholder = "\$\{$key\}"
        $content = $content -replace $placeholder, $envVars[$key]
    }

    if ($content -match "\$\{[A-Za-z_][A-Za-z0-9_]*\}") {
        Write-Host "⚠️  В шаблоне остались неразрешённые переменные: $($project.Name)"
    }

    Set-Content -Path $outputPath -Value $content -Encoding UTF8
    Write-Host "✅ Сгенерировано: $outputPath"
}

Write-Host "🎉 Генерация launchSettings.json завершена."