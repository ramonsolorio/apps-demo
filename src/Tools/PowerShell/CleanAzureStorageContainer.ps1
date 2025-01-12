params(
  [string]$tenantId = '<tenant id>',
  [string]$subscriptionId = '<subscription id>',
  [string]$storageAccountName = 'oxzsterdddap02',
  [string]$containerName = 'ct-json-mappedjsonstorepromotiondata'
)

# Get Tenant ID from Azure CLI
# az account tenant list

Connect-AzAccount -Tenant $tenantId -SubscriptionId $subscriptionId

$stgContext = New-AzStorageContext -StorageAccountName $storageAccountName
Get-AzStorageBlob -Container $containerName -Context $stgContext | Remove-AzStorageBlob


