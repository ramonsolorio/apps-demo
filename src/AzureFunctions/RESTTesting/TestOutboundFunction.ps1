param(
  [string]$crPlaza = '10LGA',  # ORACLE_SUPERIOR
  [string]$crTienda = '50FBH', # ORACLE_CR
  [string]$source = 'POS',
  [string]$pvDocName = 'PRM10LGA50FBH20241210T0107324928056Z_dab0f1ca-538b-4d26-9886-e627316ed7d7.json',
  [string]$restUri = 'https://oxzfuncprmap01.azurewebsites.net/api/outbound?code=0LJglav8Dv6ZtWk2LYPq2raDo-_7cjpInLnATmBudwjdAzFuOhLj9w%3D%3D'
)

[string]$body = "
{
    `"CRPlaza`": `"$crPlaza`",
    `"CRTienda`": `"$crTienda`",
    `"source`": `"$source`",
    `"documents`": [
      {
        `"PVDocName`": `"$pvDocName`",
        `"PVDocType`": `"PRM`"
      }
    ] 
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


