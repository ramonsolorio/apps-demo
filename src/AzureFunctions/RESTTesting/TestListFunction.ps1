param(
  [string]$crPlaza = '10LGA',  # ORACLE_SUPERIOR
  [string]$crTienda = '50FBH', # ORACLE_CR
  [string]$source = 'POS',
  [string]$restUri = 'https://oxzfuncprmap01.azurewebsites.net/api/list'
)

[string]$body = "
{
    `"CRPlaza`": `"$crPlaza`",
    `"CRTienda`": `"$crTienda`",
    `"source`": `"$source`"
}"

try
{
  (Invoke-RestMethod -Uri $restUri -Method Post -Body $body) | ConvertTo-Json -Depth 5
}
catch
{
  $respStream = $_.Exception.Response.GetResponseStream()
  $reader = New-Object System.IO.StreamReader($respStream)
  $reader.ReadToEnd();
}
