# Create professional HTML from USER_MANUAL.md
# Can be printed to PDF from browser

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "MediaNews Intercom - HTML Manual Creator" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$markdownPath = "USER_MANUAL.md"
$htmlPath = "USER_MANUAL.html"

Write-Host "Reading markdown file..." -ForegroundColor Yellow
$content = Get-Content $markdownPath -Raw -Encoding UTF8

# Simple markdown to HTML conversion
$html = $content

# Convert headers
$html = $html -replace '(?m)^# (.+)$', '<h1>$1</h1>'
$html = $html -replace '(?m)^## (.+)$', '<h2>$1</h2>'
$html = $html -replace '(?m)^### (.+)$', '<h3>$1</h3>'
$html = $html -replace '(?m)^#### (.+)$', '<h4>$1</h4>'

# Convert bold and italic
$html = $html -replace '\*\*(.+?)\*\*', '<strong>$1</strong>'
$html = $html -replace '\*(.+?)\*', '<em>$1</em>'

# Convert inline code
$html = $html -replace '`([^`]+)`', '<code>$1</code>'

# Convert code blocks
$html = $html -replace '(?ms)```(\w+)?\s*\r?\n(.+?)\r?\n```', '<pre><code>$2</code></pre>'

# Convert links
$html = $html -replace '\[([^\]]+)\]\(([^\)]+)\)', '<a href="$2">$1</a>'

# Convert lists
$html = $html -replace '(?m)^- (.+)$', '<li>$1</li>'
$html = $html -replace '(?m)^(\d+)\. (.+)$', '<li>$2</li>'

# Wrap consecutive <li> items in <ul>
$html = $html -replace '(<li>.*?</li>(\s*<li>.*?</li>)+)', '<ul>$1</ul>'

# Convert line breaks
$html = $html -replace '(?m)\r?\n\r?\n', '</p><p>'
$html = "<p>$html</p>"

# Create full HTML document
$fullHtml = @"
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>MediaNews Intercom v1.1 - User Manual</title>
    <style>
        @page {
            size: A4;
            margin: 2cm;
        }

        * {
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', 'Calibri', Arial, sans-serif;
            font-size: 11pt;
            line-height: 1.6;
            color: #333;
            max-width: 210mm;
            margin: 0 auto;
            padding: 20px;
            background: white;
        }

        h1 {
            color: #2c3e50;
            font-size: 28pt;
            font-weight: bold;
            margin-top: 40pt;
            margin-bottom: 20pt;
            page-break-after: avoid;
            border-bottom: 3px solid #3498db;
            padding-bottom: 10pt;
        }

        h1:first-of-type {
            margin-top: 0;
        }

        h2 {
            color: #34495e;
            font-size: 20pt;
            font-weight: bold;
            margin-top: 30pt;
            margin-bottom: 15pt;
            page-break-after: avoid;
            border-bottom: 2px solid #95a5a6;
            padding-bottom: 5pt;
        }

        h3 {
            color: #555;
            font-size: 16pt;
            font-weight: bold;
            margin-top: 20pt;
            margin-bottom: 10pt;
            page-break-after: avoid;
        }

        h4 {
            color: #666;
            font-size: 13pt;
            font-weight: bold;
            margin-top: 15pt;
            margin-bottom: 8pt;
            page-break-after: avoid;
        }

        p {
            margin: 10pt 0;
            text-align: justify;
        }

        code {
            background-color: #f4f4f4;
            padding: 2pt 4pt;
            border-radius: 3px;
            font-family: 'Consolas', 'Courier New', monospace;
            font-size: 10pt;
            color: #c7254e;
        }

        pre {
            background-color: #f8f8f8;
            border-left: 4px solid #3498db;
            padding: 12pt;
            margin: 15pt 0;
            overflow-x: auto;
            border-radius: 4px;
            page-break-inside: avoid;
        }

        pre code {
            background: none;
            padding: 0;
            color: #333;
            display: block;
            white-space: pre-wrap;
            word-wrap: break-word;
        }

        ul, ol {
            margin: 10pt 0;
            padding-left: 30pt;
        }

        li {
            margin: 5pt 0;
        }

        table {
            border-collapse: collapse;
            width: 100%;
            margin: 15pt 0;
            page-break-inside: avoid;
        }

        th, td {
            border: 1px solid #ddd;
            padding: 8pt;
            text-align: left;
        }

        th {
            background-color: #3498db;
            color: white;
            font-weight: bold;
        }

        tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        strong {
            font-weight: bold;
            color: #2c3e50;
        }

        em {
            font-style: italic;
        }

        a {
            color: #3498db;
            text-decoration: none;
        }

        a:hover {
            text-decoration: underline;
        }

        .cover-page {
            text-align: center;
            padding: 100pt 0;
            page-break-after: always;
        }

        .cover-title {
            font-size: 36pt;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 20pt;
        }

        .cover-subtitle {
            font-size: 24pt;
            color: #7f8c8d;
            margin-bottom: 40pt;
        }

        .cover-version {
            font-size: 18pt;
            color: #95a5a6;
            margin-bottom: 60pt;
        }

        .cover-info {
            font-size: 12pt;
            color: #7f8c8d;
            line-height: 2;
        }

        .no-print {
            background: #3498db;
            color: white;
            padding: 15pt;
            margin: -20px -20px 30pt -20px;
            border-radius: 5px;
            text-align: center;
        }

        .print-button {
            background: white;
            color: #3498db;
            border: 2px solid white;
            padding: 10pt 20pt;
            font-size: 14pt;
            cursor: pointer;
            border-radius: 5px;
            margin-top: 10pt;
            display: inline-block;
        }

        .print-button:hover {
            background: #ecf0f1;
        }

        @media print {
            body {
                max-width: 100%;
                padding: 0;
            }

            .no-print {
                display: none !important;
            }

            h1, h2, h3, h4 {
                page-break-after: avoid;
            }

            pre, table {
                page-break-inside: avoid;
            }

            a {
                color: #000;
                text-decoration: underline;
            }
        }

        @media screen {
            body {
                box-shadow: 0 0 20px rgba(0,0,0,0.1);
                margin: 20px auto;
            }
        }
    </style>
</head>
<body>
    <div class="no-print">
        <h2 style="margin: 0; color: white; border: none;">MediaNews Intercom v1.1 - User Manual</h2>
        <p style="margin: 10pt 0 0 0;">Ready to save as PDF</p>
        <button class="print-button" onclick="window.print()">📄 Save as PDF (Ctrl+P)</button>
    </div>

    <div class="cover-page">
        <div class="cover-title">MediaNews Intercom</div>
        <div class="cover-subtitle">User Manual</div>
        <div class="cover-version">Version 1.1.0</div>
        <div class="cover-info">
            Professional 8-Channel Audio Intercom System<br>
            with Remote Web Control<br>
            <br>
            November 2025<br>
            <br>
            Copyright © 2025 Roberto Musso<br>
            MIT License
        </div>
    </div>

    $html

    <hr style="margin: 40pt 0; border: none; border-top: 2px solid #ecf0f1;">
    <p style="text-align: center; color: #95a5a6; font-size: 10pt;">
        <strong>MediaNews Intercom v1.1 - User Manual</strong><br>
        Last Updated: November 2025<br>
        Copyright © 2025 Roberto Musso - MIT License<br>
        <a href="https://github.com/robertomusso/medianews-intercom">https://github.com/robertomusso/medianews-intercom</a>
    </p>
</body>
</html>
"@

Write-Host "Creating HTML file..." -ForegroundColor Yellow
$fullHtml | Out-File $htmlPath -Encoding UTF8

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "SUCCESS!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "HTML created: $htmlPath" -ForegroundColor Green
Write-Host ""
Write-Host "To create PDF:" -ForegroundColor Yellow
Write-Host "  1. Opening in browser..." -ForegroundColor White
Write-Host "  2. Click the 'Save as PDF' button OR press Ctrl+P" -ForegroundColor White
Write-Host "  3. Select 'Save as PDF' or 'Microsoft Print to PDF'" -ForegroundColor White
Write-Host "  4. Save as 'USER_MANUAL.pdf'" -ForegroundColor White
Write-Host ""
Write-Host "Opening HTML in browser..." -ForegroundColor Cyan

Start-Process $htmlPath

Write-Host ""
Write-Host "Done! Browser should open automatically." -ForegroundColor Green
Write-Host ""
