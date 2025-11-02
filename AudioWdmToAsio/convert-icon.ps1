# Script per convertire Icon.png in formato .ico

Add-Type -AssemblyName System.Drawing

$pngPath = "C:\TEMP\INTERCOM_2\Graphic\Icon.png"
$icoPath = "C:\TEMP\INTERCOM_2\AudioWdmToAsio\app.ico"

# Carica l'immagine PNG
$bitmap = [System.Drawing.Bitmap]::FromFile($pngPath)

# Crea una bitmap ridimensionata a 256x256 (dimensione standard per icone)
$size = New-Object System.Drawing.Size(256, 256)
$resized = New-Object System.Drawing.Bitmap($size.Width, $size.Height)
$graphics = [System.Drawing.Graphics]::FromImage($resized)
$graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$graphics.DrawImage($bitmap, 0, 0, $size.Width, $size.Height)
$graphics.Dispose()

# Salva come ICO
$iconStream = New-Object System.IO.FileStream($icoPath, [System.IO.FileMode]::Create)
$icon = [System.Drawing.Icon]::FromHandle($resized.GetHicon())
$icon.Save($iconStream)
$iconStream.Close()

# Cleanup
$icon.Dispose()
$resized.Dispose()
$bitmap.Dispose()

Write-Host "Icona convertita con successo: $icoPath"
