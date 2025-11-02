# Create PDF from USER_MANUAL.md using Microsoft Word
# Requires Microsoft Word installed

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "MediaNews Intercom - PDF Manual Creator" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if Word is installed
$wordInstalled = $false
try {
    $word = New-Object -ComObject Word.Application
    $wordInstalled = $true
    $word.Quit()
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($word) | Out-Null
} catch {
    $wordInstalled = $false
}

if (-not $wordInstalled) {
    Write-Host "ERROR: Microsoft Word is not installed!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Alternative options:" -ForegroundColor Yellow
    Write-Host "1. Install pandoc: https://pandoc.org/installing.html" -ForegroundColor White
    Write-Host "2. Use online converter: https://www.markdowntopdf.com/" -ForegroundColor White
    Write-Host "3. Open USER_MANUAL.md in Typora or similar and export to PDF" -ForegroundColor White
    Write-Host ""
    Write-Host "Press any key to create HTML version instead..."
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

    # Create HTML version as fallback
    Write-Host ""
    Write-Host "Creating HTML version..." -ForegroundColor Yellow

    $markdown = Get-Content "USER_MANUAL.md" -Raw

    $html = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>MediaNews Intercom v1.1 - User Manual</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            max-width: 900px;
            margin: 0 auto;
            padding: 20px;
            color: #333;
        }
        h1 {
            color: #2c3e50;
            border-bottom: 3px solid #3498db;
            padding-bottom: 10px;
        }
        h2 {
            color: #34495e;
            border-bottom: 2px solid #95a5a6;
            padding-bottom: 5px;
            margin-top: 30px;
        }
        h3 {
            color: #555;
            margin-top: 20px;
        }
        code {
            background: #f4f4f4;
            padding: 2px 6px;
            border-radius: 3px;
            font-family: 'Courier New', monospace;
        }
        pre {
            background: #f4f4f4;
            padding: 15px;
            border-radius: 5px;
            overflow-x: auto;
            border-left: 4px solid #3498db;
        }
        blockquote {
            border-left: 4px solid #3498db;
            padding-left: 15px;
            margin-left: 0;
            color: #555;
        }
        table {
            border-collapse: collapse;
            width: 100%;
            margin: 20px 0;
        }
        th, td {
            border: 1px solid #ddd;
            padding: 12px;
            text-align: left;
        }
        th {
            background-color: #3498db;
            color: white;
        }
        tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .toc {
            background: #f8f9fa;
            padding: 20px;
            border-radius: 5px;
            margin: 20px 0;
        }
        .toc ul {
            list-style-type: none;
            padding-left: 0;
        }
        .toc li {
            margin: 5px 0;
        }
        @media print {
            body {
                max-width: 100%;
            }
            .no-print {
                display: none;
            }
        }
    </style>
</head>
<body>
<div class="no-print" style="background: #3498db; color: white; padding: 20px; margin: -20px -20px 20px -20px; border-radius: 5px;">
    <h1 style="color: white; border: none; margin: 0;">MediaNews Intercom v1.1 - User Manual</h1>
    <p style="margin: 10px 0 0 0;">Professional 8-Channel Audio Intercom System</p>
    <p style="margin: 5px 0 0 0; font-size: 0.9em;">To save as PDF: Press Ctrl+P and select "Save as PDF"</p>
</div>
<pre>$markdown</pre>
</body>
</html>
"@

    $html | Out-File "USER_MANUAL.html" -Encoding UTF8

    Write-Host "HTML created: USER_MANUAL.html" -ForegroundColor Green
    Write-Host ""
    Write-Host "To create PDF:" -ForegroundColor Yellow
    Write-Host "1. Open USER_MANUAL.html in your browser" -ForegroundColor White
    Write-Host "2. Press Ctrl+P (Print)" -ForegroundColor White
    Write-Host "3. Select 'Save as PDF' or 'Microsoft Print to PDF'" -ForegroundColor White
    Write-Host "4. Save as USER_MANUAL.pdf" -ForegroundColor White
    Write-Host ""
    Write-Host "Opening HTML in default browser..." -ForegroundColor Yellow
    Start-Process "USER_MANUAL.html"

    exit
}

Write-Host "Microsoft Word found! Creating PDF..." -ForegroundColor Green
Write-Host ""

# Create Word instance
$word = New-Object -ComObject Word.Application
$word.Visible = $false

try {
    # Read markdown content
    $markdownPath = Join-Path $PSScriptRoot "USER_MANUAL.md"
    $pdfPath = Join-Path $PSScriptRoot "USER_MANUAL.pdf"

    Write-Host "Reading: $markdownPath" -ForegroundColor White

    # Create temporary HTML with better formatting
    $markdown = Get-Content $markdownPath -Raw

    # Convert markdown to basic HTML (simple conversion)
    $html = $markdown
    $html = $html -replace '#{6}\s+(.+)', '<h6>$1</h6>'
    $html = $html -replace '#{5}\s+(.+)', '<h5>$1</h5>'
    $html = $html -replace '#{4}\s+(.+)', '<h4>$1</h4>'
    $html = $html -replace '#{3}\s+(.+)', '<h3>$1</h3>'
    $html = $html -replace '#{2}\s+(.+)', '<h2>$1</h2>'
    $html = $html -replace '#{1}\s+(.+)', '<h1>$1</h1>'
    $html = $html -replace '\*\*(.+?)\*\*', '<strong>$1</strong>'
    $html = $html -replace '`(.+?)`', '<code>$1</code>'
    $html = $html -replace '^-\s+(.+)', '<li>$1</li>'
    $html = $html -replace '\n\n', '<br><br>'

    $fullHtml = @"
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <style>
        body { font-family: Calibri, Arial, sans-serif; font-size: 11pt; line-height: 1.5; }
        h1 { color: #2c3e50; font-size: 24pt; page-break-before: always; }
        h1:first-of-type { page-break-before: avoid; }
        h2 { color: #34495e; font-size: 18pt; margin-top: 20pt; }
        h3 { color: #555; font-size: 14pt; margin-top: 15pt; }
        h4 { color: #666; font-size: 12pt; }
        code { background-color: #f4f4f4; padding: 2px 4px; font-family: Consolas, monospace; }
        pre { background-color: #f4f4f4; padding: 10px; border-left: 3px solid #3498db; }
        table { border-collapse: collapse; width: 100%; }
        th, td { border: 1px solid #ddd; padding: 8px; }
        th { background-color: #3498db; color: white; }
    </style>
</head>
<body>
$html
</body>
</html>
"@

    $tempHtmlPath = Join-Path $PSScriptRoot "temp_manual.html"
    $fullHtml | Out-File $tempHtmlPath -Encoding UTF8

    Write-Host "Converting to PDF..." -ForegroundColor Yellow

    # Open HTML in Word
    $doc = $word.Documents.Open($tempHtmlPath)

    # Set up page layout
    $doc.PageSetup.TopMargin = $word.CentimetersToPoints(2)
    $doc.PageSetup.BottomMargin = $word.CentimetersToPoints(2)
    $doc.PageSetup.LeftMargin = $word.CentimetersToPoints(2)
    $doc.PageSetup.RightMargin = $word.CentimetersToPoints(2)

    # Save as PDF (format 17 = wdFormatPDF)
    Write-Host "Saving: $pdfPath" -ForegroundColor White
    $doc.SaveAs([ref]$pdfPath, [ref]17)

    # Close document
    $doc.Close($false)

    # Clean up temp file
    Remove-Item $tempHtmlPath -ErrorAction SilentlyContinue

    Write-Host ""
    Write-Host "========================================" -ForegroundColor Green
    Write-Host "SUCCESS!" -ForegroundColor Green
    Write-Host "========================================" -ForegroundColor Green
    Write-Host ""
    Write-Host "PDF created: USER_MANUAL.pdf" -ForegroundColor Green
    Write-Host "File size: $([math]::Round((Get-Item $pdfPath).Length / 1MB, 2)) MB" -ForegroundColor White
    Write-Host ""
    Write-Host "Opening PDF..." -ForegroundColor Yellow
    Start-Process $pdfPath

} catch {
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host ""
    Write-Host "Stack trace:" -ForegroundColor Yellow
    Write-Host $_.ScriptStackTrace -ForegroundColor Gray
} finally {
    # Clean up
    $word.Quit()
    [System.Runtime.Interopservices.Marshal]::ReleaseComObject($word) | Out-Null
    [System.GC]::Collect()
    [System.GC]::WaitForPendingFinalizers()
}

Write-Host ""
Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
