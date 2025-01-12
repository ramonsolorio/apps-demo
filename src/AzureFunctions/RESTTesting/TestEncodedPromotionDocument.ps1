param(
  [string]$encodedString = 'H4sIAAAAAAAACnWSy07DMBBFf6XKGpCdJgK6K4VSJEp5LSohFo49pBaNbfyoVFD/nbFxq3RBNomPb+7c8fin0LZlSn4zL7Wa6KC83RajYr4sTo62HlgHyKc385fxYKI7sFxq1DTBSQXO5f3FcrlAKsAw6ztQ/k4gpeT+dozYeW0hkZpMr2ZIVOgasIuPZ+DaChe1UQfM8lUSkn+eg+qa+Vi4JGV1SstTWr2SckSHo7o+qy6HKDNWdzq2gO5vP8Ua4z6kqrnYh15jJzESHQ5JdYFIeugm6NvqdBhI/NbEKiQaOo+99cpSSivEoMQRLEl2evkMCM/RnxJalzUpa9zhR/4YciMF2Ne/OkgsfAVpQTwFprz025xWSMfjlGKY3jL/9xinZjxObJPiplE4bqWJB4AEJ2csG8wCbPTgas0UxxfjK41vWprvfeTQHOdLENo40R4IjeszEWy+RiI30VodTDrpGAW/+J84rvY5FyqL3cGI8Rw3XppG9gw7vGqshUfXxvXuffcLsIJAzsECAAA='
)

[System.IO.MemoryStream]$compressedStream = New-Object -TypeName System.IO.MemoryStream -ArgumentList @(,([System.Convert]::FromBase64String($encodedString)))
[System.IO.MemoryStream]$decompressStream = New-Object System.IO.MemoryStream
[System.IO.Compression.GZipStream]$gZipStream = New-Object -TypeName System.IO.Compression.GZipStream -ArgumentList ($compressedStream, [System.IO.Compression.CompressionMode]::Decompress)
$gZipStream.CopyTo($decompressStream)
$decompressStream.Seek(0, [System.IO.SeekOrigin]::Begin)
[System.IO.StreamReader]$reader = New-Object -TypeName System.IO.StreamReader -ArgumentList @(,$decompressStream)
ConvertFrom-Json $reader.ReadToEnd() | ConvertTo-Json -Depth 3
