<#

   Return Codes:
     1 - Already processing promotions
     n - Response value from processing
     
#>
param(
  [string]$resourceGroupName = 'OXZRGERDDD01',
  [string]$workflowUri = "https://oxzlgerdddap01.azurewebsites.net:443/api/WF_PRM_StartCopy/triggers/When_a_HTTP_request_is_received/invoke?api-version=2022-05-01&sp=%2Ftriggers%2FWhen_a_HTTP_request_is_received%2Frun&sv=1.0&sig=6NSn4BXVbq8fdLqSYkD4y-P4CvQcrSIwIqakyHYeWVw",
  [string]$sqlServerName = 'oxzsqldberddddb03.public.5c4c56b87998.database.windows.net,3342',
  [string]$databaseName = 'MDB_ERDD_PromotionCloud',
  [string]$sqlUsername = 'sql user',
  [string]$sqlPassword = 'sql password',
  [double]$maxRunTime = 7, # Hours
  [string]$useMock = 'Y'
)

# Check to see if the promotion batches can be triggered
$isRunning = Invoke-Sqlcmd -ServerInstance $sqlServerName -Database $databaseName -Username $sqlUsername -Password $sqlPassword -query 'SELECT top 1 IsRunning FROM [CO_PRM_CONFIGD]'

if ($isRunning -eq $null)
{
    Invoke-Sqlcmd -ServerInstance $sqlServerName -Database $databaseName -Username $sqlUsername -Password $sqlPassword -query 'DELETE FROM [dbo].[CO_PRM_DAT_DTL]'
    Invoke-Sqlcmd -ServerInstance $sqlServerName -Database $databaseName -Username $sqlUsername -Password $sqlPassword -query "INSERT INTO [dbo].[CO_PRM_CONFIGD] ([CREATED_DATE],[RUN_IDENTIFIER],[IsRunning],[ReturnValue],[Description]) VALUES (GETDATE(),NEWID(),'N',1,'Initial')"
}
elseif ($isRunning.IsRunning -ne 'N')
{
    return 1
}

# Format the workflow request body
[string]$body = "{
   `"UseMock`": `"$($useMock)`"
}"

# Get the promotion batch start time
$startTime = Get-Date

# Trigger the promotion batch workflow
Invoke-WebRequest -Method Post -Uri $workflowUri -Body $body -ContentType 'application/json' | Out-Null

# Wait until the pipeline completes
do
{
    $isRunning = Invoke-Sqlcmd -ServerInstance $sqlServerName -Database $databaseName -Username $sqlUsername -Password $sqlPassword -query 'SELECT IsRunning FROM [dbo].[CO_PRM_CONFIGD]'
    $runningTime = Get-Date
    Start-Sleep -Seconds 2
} while ((($runningTime - $startTime).TotalHours -lt $maxRunTime) -and ($isRunning.IsRunning -ne 'N'))

if ($isRunning.isRunning -ne 'N')
{
    # Timed out
    return 1
}

$returnValue = Invoke-Sqlcmd -ServerInstance $sqlServerName -Database $databaseName -Username $sqlUsername -Password $sqlPassword -query 'SELECT ReturnValue FROM [dbo].[CO_PRM_CONFIGD]'

return $returnValue.ReturnValue
